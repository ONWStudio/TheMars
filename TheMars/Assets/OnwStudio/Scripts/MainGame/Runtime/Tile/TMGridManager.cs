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
        [SerializeField, InitializeRequireComponent] private GridManager _gridManager;
        private readonly Dictionary<HexCoordinates, TMBuilding> _buildings = new();

        [Header("Building Event")]
        [SerializeField] private UnityEvent<TMBuilding> _onAddedBuilding;
        [SerializeField] private UnityEvent<TMBuilding> _onRemovedBuilding;
        [SerializeField] private UnityEvent<TMBuilding> _onEnterBuilding;
        [SerializeField] private UnityEvent<TMBuilding> _onExitBuilding;
        
        public event UnityAction<IHexGrid> OnEnterTile
        {
            add => _gridManager.OnEnterTile += value;
            remove => _gridManager.OnEnterTile -= value;
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
        
        public event UnityAction<TMBuilding> OnEnterBuilding
        {
            add => _onEnterBuilding.AddListener(value);
            remove => _onEnterBuilding.RemoveListener(value);
        }
        
        public event UnityAction<TMBuilding> OnExitBuilding
        {
            add => _onExitBuilding.AddListener(value);
            remove => _onExitBuilding.AddListener(value);
        }
        
        protected override string SceneName => "MainGameScene";

        public float TileSize => _gridManager.HexagonWidth;
        public int TileCount => _gridManager.TileCount;

        public bool IsRender
        {
            get => _gridManager.IsRender;
            set => _gridManager.IsRender = value;
        }

        public IReadOnlyDictionary<HexCoordinates, TMBuilding> Buildings => _buildings;

        protected override void Init()
        {
            _gridManager.OnEnterTile += onEnterTile;
            _gridManager.OnExitTile += onExitTile;
        }

        private void onEnterTile(IHexGrid hex)
        {
            if (!_buildings.TryGetValue(hex.HexPoint, out TMBuilding building)) return;
            
            _onEnterBuilding.Invoke(building);
        }

        private void onExitTile(IHexGrid hex)
        {
            if (!_buildings.TryGetValue(hex.HexPoint, out TMBuilding building)) return;
            
            _onExitBuilding.Invoke(building);
        }

        public void AddBuilding(IHexGrid hex, TMBuilding building)
        {
            hex.AddProperty("TileOff"); // .. 타일에 건물이 설치되었으므로 다른 건물을 배치할 수 없게 속성 추가

            _buildings.Add(hex.HexPoint, building);
            _onAddedBuilding.Invoke(building);
        }

        public void AddBuildingWithoutNotify(IHexGrid hex, TMBuilding building)
        {
            _buildings.Add(hex.HexPoint, building);
        }

        public void RemoveBuilding(IHexGrid hex)
        {
            if (!_buildings.Remove(hex.HexPoint, out TMBuilding building)) return;

            hex.RemoveProperty("TileOff"); // .. 타일에 건물이 설치되었으므로 다른 건물을 배치할 수 없게 속성 추가
            _onRemovedBuilding.Invoke(building);
        }

        public List<IHexGrid> GetGrids() => _gridManager.GetGrids();
        
        public bool TryGetTileDataByRay(Ray ray, out (bool, RaycastHit) hitTuple, out IHexGrid hex) 
            => _gridManager.TryGetTileDataByRay(ray, out hitTuple, out hex);
    }
}