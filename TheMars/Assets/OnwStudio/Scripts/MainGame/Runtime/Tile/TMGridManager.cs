using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.HexGrid;
using Onw.Manager;
using Onw.Attribute;
using TM.Building;

namespace TM.Grid
{
    public sealed class TMGridManager : SceneSingleton<TMGridManager>
    {
        protected override string SceneName => "MainGameScene";

        public float TileSize => _gridManager.HexagonWidth;
        public int TileCount => _gridManager.TileCount;

        public event UnityAction<IHexGrid> OnHighlightTile
        {
            add => _gridManager.OnHighlightTile += value;
            remove => _gridManager.OnHighlightTile -= value;
        }

        public event UnityAction<IHexGrid> OnMouseDownTile
        {
            add => _gridManager.OnMouseDownTile += value;
            remove => _gridManager.OnMouseDownTile -= value;
        }

        public event UnityAction<IHexGrid> OnMouseUpTile
        {
            add => _gridManager.OnMouseUpTile += value;
            remove => _gridManager.OnMouseUpTile -= value;
        }

        public event UnityAction<IHexGrid> OnExitTile
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
        
        public event UnityAction<TMBuilding> OnChangedPointBuilding
        {
            add => _onChangedPointBuilding.AddListener(value);
            remove => _onChangedPointBuilding.RemoveListener(value);
        }

        public bool IsRender
        {
            get => _gridManager.IsRender;
            set => _gridManager.IsRender = value;
        }

        public IReadOnlyList<TMBuilding> Buildings => _buildings;        

        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;
        [SerializeField, ReadOnly] private List<TMBuilding> _buildings = new();

        [SerializeField] private UnityEvent<TMBuilding> _onAddedBuilding;
        [SerializeField] private UnityEvent<TMBuilding> _onRemovedBuilding;
        [SerializeField] private UnityEvent<TMBuilding> _onChangedPointBuilding;
        
        protected override void Init() {}

        public void AddBuilding(TMBuilding building)
        {
            _buildings.Add(building);
            _onAddedBuilding.Invoke(building);
        }

        public void AddBuildingWithoutNotify(TMBuilding building)
        {
            _buildings.Add(building);
        }

        public void ChangePointBuilding(TMBuilding building)
        {
            if (_buildings.All(b => building != b)) return;
            
            _onChangedPointBuilding.Invoke(building);
        }

        public void RemoveBuilding(TMBuilding building)
        {
            if (!_buildings.Remove(building)) return;
            _onRemovedBuilding.Invoke(building);
        }

        public List<IHexGrid> GetGrids() => _gridManager.GetGrids();
        
        public bool TryGetTileDataByRay(Ray ray, out (bool, RaycastHit) hitTuple, out IHexGrid hex) 
            => _gridManager.TryGetTileDataByRay(ray, out hitTuple, out hex);
    }
}