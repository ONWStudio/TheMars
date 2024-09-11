using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Event;
using Onw.GridTile;
using Onw.ServiceLocator;
using UnityEngine.Events;

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
        
        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;

        private void Awake()
        {
            if (ServiceLocator<TMGridManager>.RegisterService(this)) return;
            
            ServiceLocator<TMGridManager>.ChangeService(this);
        }

        private void OnDestroy()
        {
            ServiceLocator<TMGridManager>.ClearService();
        }

        public bool TryGetTileDataByRay(Ray ray, out GridTile tileData) => _gridManager.TryGetTileDataByRay(ray, out tileData);
    }
}