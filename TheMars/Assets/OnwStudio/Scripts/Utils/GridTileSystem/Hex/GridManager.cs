using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using AYellowpaper.SerializedCollections;
using Onw.Attribute;
using Onw.Extensions;
using UnityEngine.Serialization;

namespace Onw.HexGrid
{
    public sealed class GridManager : MonoBehaviour
    {
        private const float HEXAGON_RADIUS_MIN = 0.025f;
        private const float HEXAGON_RADIUS_MAX = 0.5f;

        private const float SQUARE_ROOT_THREE = 1.732051f;

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

        public int TileCount
        {
            get
            {
                int qMin = -_tileLimit;
                int qMax = _tileLimit;
                int rMin = -_tileLimit;
                int rMax = _tileLimit;
                int sMin = -_tileLimit;
                int sMax = _tileLimit;

                int kMin = -sMax;
                int kMax = -sMin;

                int total = 0;

                for (int k = kMin; k <= kMax; k++)
                {
                    int minQ = Mathf.Max(qMin, k - rMax);
                    int maxQ = Mathf.Min(qMax, k - rMin);
                    int numQ = maxQ - minQ + 1;
                    numQ = Mathf.Max(0, numQ);
                    total += numQ;
                }

                return total;
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

        [SerializeField, SerializedDictionary(isReadOnlyKey: true, isReadOnlyValue: true)] 
        private SerializedDictionary<AxialCoordinates, HexGrid> _hexGrids;
        
        [SerializeField, InitializeRequireComponent] private DecalProjector _decalProjector;

        [FormerlySerializedAs("_qLimit")]
        [SerializeField, Min(0)] private int _tileLimit = 3;
        
        private ComputeBuffer _ignoreHexBuffer;
        private ComputeBuffer _hexColorOptionBuffer;

        private void OnDestroy()
        {
            _ignoreHexBuffer?.Release();
            _hexColorOptionBuffer?.Release();
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
            
            for (int q = -_tileLimit; q <= _tileLimit; q++)
            {
                for (int r = -_tileLimit; r <= _tileLimit; r++)
                {
                    int s = -(q + r);
                    if (s >= -_tileLimit && s <= _tileLimit)
                    {
                        if (!_hexGrids.TryGetValue(new(q, r), out HexGrid hex))
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

                            hex = new(q, r, hexPosition, hexNormalizedPosition + new Vector2(0.5f, 0.5f));
                            hex.OnChangedActive += onChangedActiveTile;
                            hex.OnChangedColor += onChangedColorTile;
                            _hexGrids.NewAdd(hex.HexPoint, hex);
                        }
                    }
                }
            }
        }

        private void onChangedActiveTile(IHexGrid iHex, bool isActive)
        {
            Vector2Int[] ignoreHexes = _hexGrids.Values.Select(hex => (Vector2Int)hex.HexPoint).ToArray();

            const int STRIDE = sizeof(int) * 2;
            _ignoreHexBuffer = new(ignoreHexes.Length, STRIDE);
            _ignoreHexBuffer.SetData(ignoreHexes);
            
            _decalProjector.material.SetBuffer("_IgnoreHexGrids", _ignoreHexBuffer);
        }

        private void onChangedColorTile(IHexGrid iHex, Color color)
        {
            HexColorOption[] hexColorOptions = _hexGrids.Values.Select(hex => new HexColorOption()
            {
                Color = hex.Color.ToVec3(),
                Qr = hex.HexPoint
            }).ToArray();

            int stride = Marshal.SizeOf<HexColorOption>();
            _hexColorOptionBuffer = new(hexColorOptions.Length, stride);
            _hexColorOptionBuffer.SetData(hexColorOptions);
            
            _decalProjector.material.SetBuffer("_HexColorOption", _hexColorOptionBuffer);
        }

        public List<IHexGrid> GetGrids() => _hexGrids
            .Values
            .OfType<IHexGrid>()
            .ToList();

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