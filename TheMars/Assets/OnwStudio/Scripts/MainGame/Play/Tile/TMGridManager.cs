using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Onw.Attribute;
using Onw.Event;
using Onw.GridTile;
using Onw.ServiceLocator;

namespace TM.Grid
{
    public sealed class TMGridManager : MonoBehaviour
    {
        public float TileSize => _gridManager.TileSize;
        public int TileCount => _gridManager.TileCount;

        public IReadOnlyList<IReadOnlyGridRows> ReadOnlyTileList => _gridManager.TileList;

        public IUnityEventListenerModifier<GridTile> OnHighlightTile => _gridManager.OnHighlightTile;
        public IUnityEventListenerModifier<GridTile> OnMouseDownTile => _gridManager.OnMouseDownTile;
        public IUnityEventListenerModifier<GridTile> OnMouseUpTile => _gridManager.OnMouseUpTile;
        public IUnityEventListenerModifier<GridTile> OnExitTile => _gridManager.OnExitTile;

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