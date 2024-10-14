using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Rendering.Universal;
using AYellowpaper.SerializedCollections;
using Onw.Helper;
using Onw.Attribute;
using Onw.Components;
using Onw.Extensions;

namespace Onw.HexGrid
{
    public sealed class GridManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        private const float HEXAGON_RADIUS_MIN = 0.025f;
        private const float HEXAGON_RADIUS_MAX = 0.5f;

        private const float SQUARE_ROOT_THREE = 1.732051f;

        public const int LAYER_MASK_TILE = 1 << 3;

        public event UnityAction<IHexGrid> OnHighlightTile
        {
            add => _onHighlightTile.AddListener(value);
            remove => _onHighlightTile.RemoveListener(value);
        }

        public event UnityAction<IHexGrid> OnExitTile
        {
            add => _onExitTile.AddListener(value);
            remove => _onExitTile.RemoveListener(value);
        }

        public event UnityAction<IHexGrid> OnMouseDownTile
        {
            add => _onMouseDownTile.AddListener(value);
            remove => _onMouseDownTile.RemoveListener(value);
        }

        public event UnityAction<IHexGrid> OnMouseUpTile
        {
            add => _onMouseUpTile.AddListener(value);
            remove => _onMouseUpTile.RemoveListener(value);
        }

        [field: SerializeField, Range(HEXAGON_RADIUS_MIN, HEXAGON_RADIUS_MAX)] public float HexagonRadius { get; set; } = 0.025f;

        public float HexagonWidth => HexagonRadius * _decalProjector.size.x * 2 * _decalProjector.transform.localScale.x;
        
        public bool IsRender
        {
            get => _decalProjector.enabled;
            set => _decalProjector.enabled = value;
        }

        public int TileCount
        {
            get
            {
                int limitDouble = _tileLimit * 2;
                return (OnwMath.GetArithmeticSeriesSum(limitDouble) - OnwMath.GetArithmeticSeriesSum(_tileLimit)) * 2 + limitDouble + 1;
            }
        }

        public int TileLimit
        {
            get => _tileLimit;
            set
            {
                _tileLimit = value;
                _decalProjector.material.SetFloat("_AbsoluteLimit", _tileLimit);
            }
        }

        [Header("Events")]
        [SerializeField] private UnityEvent<IHexGrid> _onHighlightTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onExitTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseUpTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseDownTile = new();

        [SerializeField, SerializedDictionary(isReadOnlyKey: true, isReadOnlyValue: true, isLocked: true)] 
        private SerializedDictionary<AxialCoordinates, HexGrid> _hexGrids;
        
        [SerializeField, InitializeRequireComponent] private DecalProjector _decalProjector;
        [SerializeField, InitializeRequireComponent] private MouseMovementTracker _mouseMovementTracker;

        [FormerlySerializedAs("_qLimit")]
        [SerializeField, Min(0)] private int _tileLimit = 3;
        
        [SerializeField] private Camera _mainCamera = null;
        private ComputeBuffer _hexOptionBuffer;

        private HexGrid _currentHex = null;
        
        private void Awake()
        {
            SendToShaderHexOption();
        }

        private void Start()
        {
            _mouseMovementTracker.OnHoverMouse += onHoverMouse;

            if (!_mainCamera)
            {
                Debug.LogWarning("카메라가 초기화되지 않았으므로 자동으로 메인카메라를 찾습니다");
                _mainCamera = Camera.main;
            }
        }
        
        private void OnDestroy()
        {
            _hexOptionBuffer?.Release();
        }

        private void onHoverMouse(Vector2 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            
            if (TryGetTileDataByRay(ray, out IHexGrid iHex))
            {
                HexGrid hex = (HexGrid)iHex;

                if (_currentHex != hex)
                {
                    HexGrid prevHex = _currentHex;
                    _currentHex = hex;
                    _currentHex.InvokeOnHighlightTile();
                    prevHex?.InvokeOnExitTile();
                }
            }
        }

        public void SetActiveAxialCoordinates(int q, int r, bool isActive)
        {
            if (!_hexGrids.TryGetValue(new(q, r), out HexGrid hex)) return;

            hex.IsActive = isActive;
        }

        public bool TryGetTileDataByRay(in Ray ray, out IHexGrid hexGrid)
        {
            hexGrid = null;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 convertedPosition = new(hit.point.x - _decalProjector.transform.position.x, hit.point.z - _decalProjector.transform.position.z);
                convertedPosition /= _decalProjector.size.x * _decalProjector.GetLocalScaleX();

                Vector2 axialCoordinates = new(
                    2f / 3 * convertedPosition.x / HexagonRadius,
                    (-1f / 3 * convertedPosition.x + SQUARE_ROOT_THREE / 3 * convertedPosition.y) / HexagonRadius);

                float sFloat = -axialCoordinates.x - axialCoordinates.y;

                Vector3Int roundHex = new(
                    Mathf.RoundToInt(axialCoordinates.x),
                    Mathf.RoundToInt(axialCoordinates.y),
                    Mathf.RoundToInt(sFloat));

                Vector3 hexDifferent = new(
                    Mathf.Abs(roundHex.x - axialCoordinates.x),
                    Mathf.Abs(roundHex.y - axialCoordinates.y),
                    Mathf.Abs(roundHex.z - sFloat));

                Vector3Int hexCoordinates = roundHex;
                
                if (hexDifferent.x > hexDifferent.y && hexDifferent.x > hexDifferent.z)
                {
                    hexCoordinates.x = -roundHex.y - roundHex.z;
                }
                else if (hexDifferent.y > hexDifferent.z)
                {
                    hexCoordinates.y = -roundHex.x - roundHex.z;
                }
                else
                {
                    hexCoordinates.z = -roundHex.x - roundHex.y;
                }

                if (_hexGrids.TryGetValue(new(hexCoordinates.x, hexCoordinates.y), out HexGrid hex))
                {
                    hexGrid = hex;
                    return true;
                }
            }

            return false;
        }

        public void CalculateTile()
        {
            Vector3 projectorSize = _decalProjector.size * _decalProjector.GetLocalScaleX();
            Dictionary<AxialCoordinates, HexGrid> hexGrids = _hexGrids.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            List<HexGrid> hexList = new(_hexGrids.Count);
            _hexGrids.NewClear();
            
            for (int q = -_tileLimit; q <= _tileLimit; q++)
            {
                for (int r = -_tileLimit; r <= _tileLimit; r++)
                {
                    int s = -(q + r);
                    if (s >= -_tileLimit && s <= _tileLimit)
                    {
                        Vector2 hexNormalizedPosition = new(
                            HexagonRadius * (1.5f * q),
                            HexagonRadius * (SQUARE_ROOT_THREE * 0.5f * q + SQUARE_ROOT_THREE * r));

                        Vector3 hexPosition = projectorSize.x * hexNormalizedPosition;

                        hexPosition = new(
                            hexPosition.x + _decalProjector.transform.position.x,
                            0.0f,
                            hexPosition.y + _decalProjector.transform.position.z);

                        Ray ray = new(
                            new(
                                hexPosition.x,
                                _decalProjector.transform.position.y + projectorSize.y * 0.5f,
                                hexPosition.z),
                            Vector3.down);

                        if (Physics.Raycast(ray, out RaycastHit hit, projectorSize.y, 1 << 0))
                        {
                            hexPosition = hit.point;
                        }

                        AxialCoordinates key = new(q, r);
                        HexGrid newHex = createTile();

                        if (hexGrids.TryGetValue(key, out HexGrid hex))
                        {
                            hex.Properties.ForEach(newHex.AddProperty);
                            newHex.IsActive = hex.IsActive;
                            newHex.Color = hex.Color;
                        }
                        
                        _hexGrids.NewAdd(key, newHex);
                        hexList.Add(newHex);

                        HexGrid createTile()
                        {
                            HexGrid createdHex = new(q, r, hexPosition, hexNormalizedPosition + new Vector2(0.5f, 0.5f), _tileLimit);
                            createdHex.OnChangedActive += onChangedActiveTile;
                            createdHex.OnChangedColor += onChangedColorTile;
                            createdHex.OnHighlightTile += _onHighlightTile.Invoke;
                            createdHex.OnMouseDownTile += _onMouseDownTile.Invoke;
                            createdHex.OnMouseUpTile += _onMouseUpTile.Invoke;
                            createdHex.OnExitTile += _onExitTile.Invoke;

                            return createdHex;
                        }
                    }
                }
            }

            Debug.Log(hexList.Count);
            sendToShaderHexOption(hexList.Select(toOption).ToArray());
        }

        private void onChangedActiveTile(IHexGrid iHex, bool isActive)
        {
            SendToShaderHexOption();
        }

        private void onChangedColorTile(IHexGrid iHex, Color color)
        {
            SendToShaderHexOption();
        }

        private HexOption toOption(HexGrid hex)
        {
            return new()
            {
                Color = hex.Color.ToVec3(),
                IsActive = hex.IsActive ? 1 : 0
            };
        }

        public void SendToShaderHexOption()
        {
            sendToShaderHexOption(_hexGrids.Values.Select(toOption).ToArray());
        }

        private void sendToShaderHexOption(HexOption[] hexOptions)
        {
            int stride = Marshal.SizeOf<HexOption>();
            _hexOptionBuffer = new(hexOptions.Length, stride);
            _hexOptionBuffer.SetData(hexOptions);
            
            _decalProjector.material.SetInt("_BufferOn", 1);
            _decalProjector.material.SetBuffer("_HexOptions", _hexOptionBuffer);
        }

        public List<IHexGrid> GetGrids() => _hexGrids
            .Values
            .OfType<IHexGrid>()
            .ToList();
        
        public void OnBeforeSerialize()
        {
            SendToShaderHexOption();
        }
        
        public void OnAfterDeserialize()
        {
        }

        #if UNITY_EDITOR
        [OnChangedValueByMethod(nameof(_tileLimit))]
        private void onChangedLimit()
        {
            _decalProjector.material.SetFloat("_AbsoluteLimit", _tileLimit);
        }

        [OnChangedValueByMethod(nameof(HexagonRadius))]
        private void onChangedRadius()
        {
            _decalProjector.material.SetFloat("_Radius", HexagonRadius);
        }
        #endif
    }
}