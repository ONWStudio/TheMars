
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Extensions;
using Onw.Attribute;

namespace Onw.HexGrid
{
    public interface IHexCoordinates
    {
        int Q { get; }
        int R { get; }
        int S { get; }

    }
    
    public interface IHexGrid : IHexCoordinates
    {
        IReadOnlyList<string> Properties { get; }

        Vector3 TilePosition { get; }
        Vector2 NormalizedPosition { get; }
        
        event UnityAction<IHexGrid> OnHighlightTile;
        event UnityAction<IHexGrid> OnExitTile;
        event UnityAction<IHexGrid> OnMouseDownTile;
        event UnityAction<IHexGrid> OnMouseUpTile;
        
        void AddProperty(string property);
        bool RemoveProperty(string property);
    }
    
    [System.Serializable]
    public class HexGrid : IHexGrid
    {
        [field: SerializeField, ReadOnly] public int Q { get; private set; }
        [field: SerializeField, ReadOnly] public int R { get; private set; }
        [field: SerializeField, ReadOnly] public int S { get; private set; }
        [field: SerializeField, ReadOnly] public Vector3 TilePosition { get; private set; } = Vector3.zero;
        [field: SerializeField, ReadOnly] public Vector2 NormalizedPosition { get; private set; } = Vector2.zero;
        
        public IReadOnlyList<string> Properties => _properties;

        [SerializeField] private List<string> _properties = new();

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

        [Header("Events")]
        [SerializeField] private UnityEvent<IHexGrid> _onHighlightTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onExitTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseUpTile = new();
        [SerializeField] private UnityEvent<IHexGrid> _onMouseDownTile = new();

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

        public HexGrid(int q, int r)
        {
            Q = q;
            R = r;
            S = -q - r;
        }
        
        public static implicit operator Vector3(HexGrid hexGrid) => new(hexGrid.Q, hexGrid.R, hexGrid.S);
    }
}
