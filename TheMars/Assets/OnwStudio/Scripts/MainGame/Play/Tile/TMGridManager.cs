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
        
        public IUnityEventListenerModifier<TileData> OnHighlightTile => _gridManager.OnHighlightTile;
        public IUnityEventListenerModifier<TileData> OnMouseDownTile => _gridManager.OnMouseDownTile;
        public IUnityEventListenerModifier<TileData> OnMouseUpTile => _gridManager.OnMouseUpTile;
        public IUnityEventListenerModifier<TileData> OnExitTile => _gridManager.OnExitTile;
        
        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;
        
        private void Awake()
        {
            if (!ServiceLocator<TMGridManager>.RegisterService(this))
            {
                ServiceLocator<TMGridManager>.ChangeService(this);
            }
        }

        private void OnDestroy()
        {
            ServiceLocator<TMGridManager>.ClearService();
        }

        public bool TryGetTileDataByRay(Ray ray, out TileData tileData) => _gridManager.TryGetTileDataByRay(ray, out tileData);
    }
}