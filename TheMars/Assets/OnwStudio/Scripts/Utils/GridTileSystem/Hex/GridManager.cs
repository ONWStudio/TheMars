using System;
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
        [SerializeField, InitializeRequireComponent] private MouseInputEvent _mouseInputEvent;

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
            MouseMovementTracker.Instance.OnHoverMouseRuntime += onHoverMouseRuntime;
            _mouseInputEvent.AddListenerDownEvent<OnwMouseLeftInputEvent>(onMouseDownEvent);
            _mouseInputEvent.AddListenerUpEvent<OnwMouseLeftInputEvent>(onMouseUpEvent);

            if (!_mainCamera)
            {
                Debug.LogWarning("카메라가 초기화되지 않았으므로 자동으로 메인카메라를 찾습니다");
                _mainCamera = Camera.main;
            }
        }

        private void OnApplicationQuit()
        {
            _hexOptionBuffer?.Release();
        }

        private void OnDestroy()
        {
            _hexOptionBuffer?.Release();
        }

        private void onMouseDownEvent(Vector2 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (TryGetTileDataByRay(ray, out (bool, RaycastHit) _, out IHexGrid iHex))
            {
                ((HexGrid)iHex).InvokeOnMouseDownTile();
            }
        }

        private void onMouseUpEvent(Vector2 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (TryGetTileDataByRay(ray, out (bool, RaycastHit) _, out IHexGrid iHex))
            {
                ((HexGrid)iHex).InvokeOnMouseUpTile();
            }
        }

        private void onHoverMouseRuntime(Vector2 mousePosition)
        {
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (TryGetTileDataByRay(ray, out (bool, RaycastHit) _, out IHexGrid iHex))
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

        /// <summary>
        /// .. 레이캐스트를 한후 타일에 충돌했는지 검사합니다
        /// 타일에 충돌한 경우 true를 반환하고 hexGrid를 전달합니다
        /// 타일이 아니지만 레이캐스트 충돌이 일어난 경우 TUPLE의 첫번째 아이템이 true를 전달합니다
        /// </summary>
        /// <param name="ray"> .. 검사할 레이 </param>
        /// <param name="hitTuple"> .. 히트 정보를 담는 튜플 </param>
        /// <param name="hexGrid"> .. 충돌한 타일 </param>
        /// <returns></returns>
        public bool TryGetTileDataByRay(in Ray ray, out (bool, RaycastHit) hitTuple, out IHexGrid hexGrid)
        {
            hexGrid = null;
            hitTuple.Item1 = false;
            
            if (Physics.Raycast(ray, out hitTuple.Item2))
            {
                hitTuple.Item1 = true;
                Vector2 convertedPosition = new(hitTuple.Item2.point.x - _decalProjector.transform.position.x, hitTuple.Item2.point.z - _decalProjector.transform.position.z);
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

                switch (hexDifferent)
                {
                    case var _ when hexDifferent.x > hexDifferent.y && hexDifferent.x > hexDifferent.z:
                        hexCoordinates.x = -roundHex.y - roundHex.z;
                        break;
                    case var _ when hexDifferent.y > hexDifferent.z:
                        hexCoordinates.y = -roundHex.x - roundHex.z;
                        break;
                    default:
                        hexCoordinates.z = -roundHex.z - roundHex.y;
                        break;
                }

                if (_hexGrids.TryGetValue(new(hexCoordinates.x, hexCoordinates.y), out HexGrid hex))
                {
                    hexGrid = hex;
                    return hex.IsActive;
                }
            }

            return false;
        }

        public void CalculateTile()
        {
            Vector3 projectorSize = _decalProjector.size * _decalProjector.GetLocalScaleX();

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

                        if (Physics.Raycast(ray, out RaycastHit hit, projectorSize.y, 1 << 3))
                        {
                            hexPosition = hit.point;
                        }

                        AxialCoordinates key = new(q, r);
                        HexGrid newHex = createTile();

                        if (_hexGrids.TryGetValue(key, out HexGrid hex))
                        {
                            hex.Properties.ForEach(newHex.AddProperty);
                            newHex.IsActive = hex.IsActive;
                            newHex.Color = hex.Color;
                            _hexGrids[key] = newHex;
                        }
                        else
                        {
                            _hexGrids.Add(key, newHex);
                        }

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

            SendToShaderHexOption();
        }

        private void onChangedActiveTile(IHexGrid iHex, bool isActive)
        {
            SendToShaderHexOption();
        }

        private void onChangedColorTile(IHexGrid iHex, Color color)
        {
            SendToShaderHexOption();
        }

        public void SendToShaderHexOption()
        {
            HexOption[] hexOptions = _hexGrids.Values.Select(hex => hex.GetShaderOption()).ToArray();
            int stride = Marshal.SizeOf<HexOption>();
            bool isZero = hexOptions.Length > 0;
            
            _decalProjector.material.SetInt("_BufferOn", isZero ? 1 : 0);

            if (isZero)
            {
                _hexOptionBuffer?.Release();
                _hexOptionBuffer = new(hexOptions.Length, stride);
                _hexOptionBuffer.SetData(hexOptions);

                _decalProjector.material.SetBuffer("_HexOptions", _hexOptionBuffer);
            }
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