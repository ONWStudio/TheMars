using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Onw.Attribute;
using Onw.Extensions;
using Onw.GridTile;
using Onw.Helper;
using Onw.ServiceLocator;
using TM.Grid;
using TM.Building;
using TMCard.Runtime;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;


namespace TMCard.Effect
{
    public sealed class TMCardBuildingCreateEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardBuildingCreateEffectCreator>, IDisposable
    {
        public string Description => "";

        public TMBuilding Building => _building;

        [SerializeField, ReadOnly] private TMBuilding _building = null;
        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; } = null;

        private TMCardModel _cardModel = null;
        private Camera _mainCamera = null;
        private GridTile _currentTile = null;

        public void Initialize(TMCardBuildingCreateEffectCreator effectCreator)
        {
            BuildingData = effectCreator.BuildingData;
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            _mainCamera = Camera.main;

            _cardModel = cardModel;
            _cardModel.OnEffectEvent.AddListener(onEffect);
            _cardModel.InputHandler.DownAction.AddListener(onDownCard);

            _building = Object
                .Instantiate(BuildingData.BuildingPrefab.gameObject)
                .GetComponent<TMBuilding>();
        }

        private void onDownCard(PointerEventData _)
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            gridManager.OnHighlightTile.AddListener(setTileHighlight);
            gridManager.OnExitTile.AddListener(setTileUnHighlight);

            _building.gameObject.SetActive(true);
            MeshRenderer meshRenderer = _building.GetComponent<MeshRenderer>();

            float xWidth = meshRenderer.bounds.size.x;
            float zWidth = meshRenderer.bounds.size.z;
            float tileSize = gridManager.TileSize;
            float ratio = tileSize / Mathf.Max(xWidth, zWidth);
            _building.transform.localScale *= ratio * 0.55f; // .. 어떤 건물이든 항상 동일한 폭으로 생성

            _cardModel.InputHandler.DragAction.AddListener(onDrag);
            _cardModel.IsHide = true;

            setCurrentTile(gridManager);
            onDrag(null);
        }

        private void setCurrentTile(TMGridManager gridManager)
        {
            if (!gridManager.TryGetTileDataByRay(_mainCamera.ScreenPointToRay(Input.mousePosition), out GridTile tileData)) return;

            _currentTile = tileData;
            setTileHighlight(tileData);
        }

        private void onDrag(PointerEventData _)
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!_cardModel.IsOverDeckTransform)
            {
                if (!_building.gameObject.activeSelf)
                {
                    _building.gameObject.SetActive(true);
                    gridManager.OnHighlightTile.AddListener(setTileHighlight);
                    setCurrentTile(gridManager);
                }

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 hitPoint = hit.point;
                    if (_currentTile is not null && hit.collider.gameObject.name == "Tile")
                    {
                        Vector3 tilePosition = _currentTile.TileRenderer.transform.position;
                        float tileHalfSize = gridManager.TileSize * 0.5f;
                        tilePosition.x += tileHalfSize;
                        tilePosition.z += tileHalfSize;
                        hitPoint = new(tilePosition.x, hitPoint.y, tilePosition.z);
                    }

                    _building.transform.position = hitPoint;
                }
            }
            else
            {
                if (_building.gameObject.activeSelf)
                {
                    _building.gameObject.SetActive(false);
                    gridManager.OnHighlightTile.RemoveListener(setTileHighlight);

                    if (_currentTile is not null)
                    {
                        setTileUnHighlight(_currentTile);
                        _currentTile = null;
                    }
                }
            }
        }

        private void onEffect(TMCardModel card)
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) &&
                hit.collider.gameObject.name == "Tile" &&
                (_currentTile?.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?? false))
            {
                GridTile buildingTile = _currentTile;
                buildingTile.Properties.Add("TileOff");
                _building.transform.SetParent(buildingTile.TileRenderer.transform);
                _building.Initialize(BuildingData);
                _building.BatchOnTile();

                bool hasCardManager = ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager);

                if (hasCardManager)
                {
                    cardManager.RemoveCard(_cardModel);
                }

                _cardModel.gameObject.SetActive(false);
                buildingTile.OnMouseDownTile.AddListener(onMouseDownTile);

                void onMouseDownTile(GridTile tileData)
                {
                    if (!hasCardManager) return;

                    _currentTile = buildingTile;
                    _currentTile.Properties.Remove("TileOff");
                    _currentTile.OnMouseDownTile.RemoveListener(onMouseDownTile);
                    _cardModel.gameObject.SetActive(true);

                    cardManager.AddCard(_cardModel);
                    _cardModel.TriggerSelectCard();
                    onDownCard(null);
                }
            }
            else
            {
                _building.gameObject.SetActive(false);
            }

            _currentTile = null;
            _cardModel.IsHide = false;
            _cardModel.InputHandler.DragAction.RemoveListener(onDrag);

            gridManager.OnHighlightTile.RemoveListener(setTileHighlight);
            gridManager.OnExitTile.RemoveListener(setTileUnHighlight);
            gridManager
                .ReadOnlyTileList
                .SelectMany(rows => rows.Rows)
                .ForEach(setTileUnHighlight);
        }

        private void setTileHighlight(GridTile tileArgs)
        {
            _currentTile = tileArgs;

            tileArgs.TileRenderer.material.color =
                tileArgs.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?
                    Color.yellow :
                    Color.red;
        }

        private static void setTileUnHighlight(GridTile tileArgs)
        {
            tileArgs.TileRenderer.material.color = Color.white;
        }

        public void Dispose()
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            gridManager.OnHighlightTile.RemoveListener(setTileHighlight);
            gridManager.OnExitTile.RemoveListener(setTileUnHighlight);
            OnwUnityHelper.DestroyObjectByComponent(ref _building);
        }
    }
}