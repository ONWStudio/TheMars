
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using Onw.Extensions;
using Onw.Attribute;

namespace Onw.HexGrid
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HexColorOption
    {
        public Vector3Int Qr;
        public Vector3 Color;
    }
    
    [System.Serializable]
    public class HexGrid : IHexGrid
    {
        [field: SerializeField, ReadOnly] public Vector3 TilePosition { get; private set; } = Vector3.zero;
        [field: SerializeField, ReadOnly] public Vector2 NormalizedPosition { get; private set; } = Vector2.zero;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                _onChangedActive.Invoke(this, _isActive);
            }
        }
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                _onChangedColor.Invoke(this, _color);
            }
        }


        public IReadOnlyList<string> Properties => _properties;

        /// <summary>
        /// .. 호출 시 박싱이 발생합니다
        /// </summary>
        public HexCoordinates HexPoint => _hexPoint; 
        
        [SerializeField] private List<string> _properties = new();
        [SerializeField, ReadOnly] private HexCoordinates _hexPoint = new();
        
        [SerializeField] private bool _isActive = true;
        [SerializeField] private Color _color = Color.cyan;
        
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
        
        public event UnityAction<IHexGrid, bool> OnChangedActive
        {
            add => _onChangedActive.AddListener(value); 
            remove => _onChangedActive.RemoveListener(value);
        }
        
        public event UnityAction<IHexGrid, Color> OnChangedColor
        {
            add => _onChangedColor.AddListener(value); 
            remove => _onChangedColor.RemoveListener(value);
        }

        [Header("Interactable Events")]
        [SerializeField] private UnityEvent<IHexGrid> _onHighlightTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onExitTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseUpTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseDownTile = new();
        
        [Header("Value Changed Events")]
        [SerializeField] private UnityEvent<IHexGrid, bool> _onChangedActive = new();
        [SerializeField] private UnityEvent<IHexGrid, Color> _onChangedColor = new();

        public void AddProperty(string property)
        {
            _properties.Add(property);
        }

        public bool RemoveProperty(string property)
        {
            return _properties.RemoveByConditionAll(p => property == p);
        }

        public void InvokeOnHighlightTile()
        {
            _onHighlightTile.Invoke(this);
        }

        public void InvokeOnExitTile()
        {
            _onExitTile.Invoke(this);
        }

        public void InvokeOnMouseUpTile()
        {
            _onMouseUpTile.Invoke(this);
        }

        public void InvokeOnMouseDownTile()
        {
            _onMouseDownTile.Invoke(this);
        }

        public void SetTilePosition(in Vector3 tilePosition)
        {
            TilePosition = tilePosition;
        }
        
        public HexGrid(int q, int r, in Vector3 tilePosition, in Vector2 normalizedPosition) : this(q, r, tilePosition)
        {
            NormalizedPosition = normalizedPosition;
        }
        
        public HexGrid(int q, int r, in Vector3 tilePosition) : this(q, r)
        {
            TilePosition = tilePosition;
        }

        public HexGrid(AxialCoordinates axialCoordinates)
        {
            _hexPoint = axialCoordinates;
        }
        
        public HexGrid(int q, int r)
        {
            _hexPoint = new(q, r);
            _onChangedActive.SetPersistentListenerState(UnityEventCallState.EditorAndRuntime);
            _onChangedColor.SetPersistentListenerState(UnityEventCallState.EditorAndRuntime);
        }

        public int GetHexIndex(int limit)
        {
            int q = _hexPoint.Q;
            int r = _hexPoint.R;

            int rMin = Mathf.Max(-limit, -q - limit);
            int indexInRow = r - rMin;
            int totalTilesBeforeQ = getTotalTilesBeforeQ(q, limit);

            return totalTilesBeforeQ + indexInRow;
            static int getTotalTilesBeforeQ(int q, int limit)
            {
                int adjustedQ = q + limit;
                int totalTilesBeforeQ = adjustedQ * (2 * limit + 1) - adjustedQ * (adjustedQ + 1) / 2;
                return totalTilesBeforeQ;
            }
        }
        
        #if UNITY_EDITOR
        [OnChangedValueByMethod(nameof(_isActive))]
        private void onChangedActive()
        {
            _onChangedActive.Invoke(this, _isActive);
        }

        [OnChangedValueByMethod(nameof(_color))]
        private void onChangedColor()
        {
            _onChangedColor.Invoke(this, _color);
        }
        #endif
    }
}
