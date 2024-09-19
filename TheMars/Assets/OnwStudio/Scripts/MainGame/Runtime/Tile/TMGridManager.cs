using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.GridTile;
using TM.Building;

namespace TM.Grid
{
    public sealed class TMGridManager : MonoBehaviour
    {
        public float TileSize => _gridManager.TileSize;
        public int TileCount => _gridManager.TileCount;

        public IReadOnlyList<IReadOnlyGridRows> ReadOnlyTileList => _gridManager.TileList;

        public event UnityAction<GridTile> OnHighlightTile
        {
            add => _gridManager.OnHighlightTile += value;
            remove => _gridManager.OnHighlightTile -= value;
        }
        
        public event UnityAction<GridTile> OnMouseDownTile
        {
            add => _gridManager.OnMouseDownTile += value;
            remove => _gridManager.OnMouseDownTile -= value;
        }
        
        public event UnityAction<GridTile> OnMouseUpTile
        {
            add => _gridManager.OnMouseUpTile += value;
            remove => _gridManager.OnMouseUpTile -= value;
        }
        
        public event UnityAction<GridTile> OnExitTile
        {
            add => _gridManager.OnExitTile += value;
            remove => _gridManager.OnExitTile -= value;
        }

        public event UnityAction<TMBuilding> OnAddedBuilding
        {
            add => _onAddedBuilding.AddListener(value);
            remove => _onAddedBuilding.RemoveListener(value);
        }
        
        public event UnityAction<TMBuilding> OnRemovedBuilding
        {
            add => _onRemovedBuilding.AddListener(value);
            remove => _onRemovedBuilding.RemoveListener(value);
        }
        
        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;
        [SerializeField, ReadOnly] private List<TMBuilding> _buildings = new(); 
        
        [SerializeField, ReadOnly] private UnityEvent<TMBuilding> _onAddedBuilding;
        [SerializeField, ReadOnly] private UnityEvent<TMBuilding> _onRemovedBuilding;

        public void AddBuilding(TMBuilding building)
        {
            _buildings.Add(building);
            _onAddedBuilding.Invoke(building);
        }

        public void RemoveBuilding(TMBuilding building)
        {
            _buildings.Remove(building);
            _onRemovedBuilding.Invoke(building);
        }
        
        public bool TryGetTileDataByRay(Ray ray, out GridTile tileData) => _gridManager.TryGetTileDataByRay(ray, out tileData);
    }
}