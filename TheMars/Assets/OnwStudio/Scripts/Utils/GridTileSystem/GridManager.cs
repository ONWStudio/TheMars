using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Rendering.Universal;
using AYellowpaper.SerializedCollections;
using Onw.Attribute;
using Onw.Extensions;
using TreeEditor;

namespace Onw.HexGrid
{
    [System.Serializable]
    public struct IgnoreHexCoordinates : IHexCoordinates
    {
        [field: SerializeField] public int Q { get; set; }
        [field: SerializeField] public int R { get; set; }
        [field: SerializeField] public int S { get; set; }

        public static implicit operator Vector3(in IgnoreHexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R, hexCoordinates.S);
        public static implicit operator Vector3Int(in IgnoreHexCoordinates hexCoordinates) => new(hexCoordinates.Q, hexCoordinates.R, hexCoordinates.S);
        public static implicit operator IgnoreHexCoordinates(in Vector3 vec) => new()
        {
            Q = (int)vec.x,
            R = (int)vec.y,
            S = (int)vec.z
        };
        public static implicit operator IgnoreHexCoordinates(in Vector3Int vec) => new()
        {
            Q = vec.x,
            R = vec.y,
            S = vec.z
        };
        public static implicit operator IgnoreHexCoordinates(in Vector2 vec) => new()
        {
            Q = (int)vec.x,
            R = (int)vec.y
        };
        public static implicit operator IgnoreHexCoordinates(in Vector2Int vec) => new()
        {
            Q = vec.x,
            R = vec.y
        };
    }

    public sealed class GridManager : MonoBehaviour
    {
        private const float HEXAGON_RADIUS_MIN = 0.025f;
        private const float HEXAGON_RADIUS_MAX = 0.5f;

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
                int qMin = -_qLimit;
                int qMax = _qLimit;
                int rMin = -_rLimit;
                int rMax = _rLimit;
                int sMin = -_sLimit;
                int sMax = _sLimit;

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

        public int QLimit
        {
            get => _qLimit;
            set
            {
                _qLimit = value;
                _decalProjector.material.SetVector("_AbsoluteQRS", new(_qLimit, _rLimit, _sLimit));
            }
        }

        public int RLimit
        {
            get => _rLimit;
            set
            {
                _rLimit = value;
                _decalProjector.material.SetVector("_AbsoluteQRS", new(_qLimit, _rLimit, _sLimit));
            }
        }

        public int SLimit
        {
            get => _sLimit;
            set
            {
                _sLimit = value;
                _decalProjector.material.SetVector("_AbsoluteQRS", new(_qLimit, _rLimit, _sLimit));
            }
        }

        [Header("Events")]
        [SerializeField] private UnityEvent<IHexGrid> _onHighlightTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onExitTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseUpTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseDownTile = new();

        [FormerlySerializedAs("_tileList")]
        [SerializeField, ReadOnly] private SerializedDictionary<string, HexGrid> _hexGrids = new();
        [SerializeField, InitializeRequireComponent] private DecalProjector _decalProjector;

        [SerializeField, Min(0)] private int _qLimit = 3;
        [SerializeField, Min(0)] private int _rLimit = 3;
        [SerializeField, Min(0)] private int _sLimit = 3;

        [FormerlySerializedAs("_ignoreGridTiles")]
        [SerializeField] private List<IgnoreHexCoordinates> _ignoreHexGrids = new();
        
        public void AddIgnoreGridTile(int q, int r, int s)
        {
            if (_ignoreHexGrids.Any(ignoreGridTile => ignoreGridTile.Q == q && ignoreGridTile.R == r && ignoreGridTile.S == s)) return;

            _ignoreHexGrids.Add(new()
            {
                Q = q,
                R = r,
                S = s
            });
        }

        public void RemoveIgnoreGridTile(int q, int r, int s)
        {
            _ignoreHexGrids.RemoveByConditionOne(tile => tile.Q == q && tile.R == r && tile.S == s);
        }

        public bool TryGetTileDataByRay(in Ray ray, out IHexGrid hexGrid)
        {
            hexGrid = null;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 convertedPosition = new(hit.point.x - _decalProjector.transform.position.x, hit.point.z - _decalProjector.transform.position.z);
                convertedPosition /= _decalProjector.size.x * _decalProjector.GetLocalScaleX();

                Debug.Log(convertedPosition);

                Vector2 axialCoordinates = new(
                    2f / 3 * convertedPosition.x / HexagonRadius,
                    (-1f / 3 * convertedPosition.x + 1.732051f / 3 * convertedPosition.y) / HexagonRadius);

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

                Debug.Log(hexCoordinates);

                if (_hexGrids.TryGetValue($"{hexCoordinates.x}{hexCoordinates.y}{hexCoordinates.z}", out HexGrid hex))
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
            
            for (int q = -_qLimit; q <= _qLimit; q++)
            {
                for (int r = -_rLimit; r <= _rLimit; r++)
                {
                    int s = -(q + r);
                    if (s >= -_sLimit && s <= _sLimit)
                    {
                        string key = $"{q}{r}{s}";

                        if (!_hexGrids.TryGetValue(key, out HexGrid hex))
                        {
                            Vector2 hexNormalizedPosition = new(
                                HexagonRadius * (1.5f * q),
                                HexagonRadius * (1.732051f / 2 * q + 1.732051f * r));

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

                            hex = new(q, r, hexPosition, hexNormalizedPosition);
                            _hexGrids.NewAdd(key, hex);
                        }
                    }
                }
            }
        }

        public List<IHexGrid> GetGrids() => _hexGrids
            .Values
            .OfType<IHexGrid>()
            .ToList();

        #if UNITY_EDITOR
        [OnChangedValueByMethod(nameof(_qLimit), nameof(_rLimit), nameof(_sLimit))]
        private void onChangedLimit()
        {
            _decalProjector.material.SetVector("_AbsoluteQRS", new(_qLimit, _rLimit, _sLimit));
        }

        [OnChangedValueByMethod(nameof(HexagonRadius))]
        private void onChangedRadius()
        {
            _decalProjector.material.SetFloat("_Radius", HexagonRadius);
        }
        #endif
    }
}