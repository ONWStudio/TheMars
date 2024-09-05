using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Onw.GridTile;
using Onw.Attribute;
using Onw.Extensions;
using Onw.ServiceLocator;
using TM.Grid;
using TM.Building;
using TMCard.Runtime;
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
        }

        private void onDownCard(PointerEventData eventData)
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            gridManager.OnHighlightTile.AddListener(setTileHighlight);
            gridManager.OnExitTile.AddListener(setTileUnHighlight);

            if (!_building)
            {
                _building = Object
                    .Instantiate(BuildingData.BuildingPrefab.gameObject)
                    .GetComponent<TMBuilding>();
            }
            else
            {
                _building.gameObject.SetActive(true);
            }

            MeshRenderer meshRenderer = _building.GetComponent<MeshRenderer>();

            float xWidth = meshRenderer.bounds.size.x;
            float zWidth = meshRenderer.bounds.size.z;
            float tileSize = gridManager.TileSize;
            float ratio = tileSize / Mathf.Max(xWidth, zWidth);
            _building.transform.localScale *= ratio * 0.55f; // .. 어떤 건물이든 항상 동일한 폭으로 생성

            _cardModel.InputHandler.DragAction.AddListener(onDrag);
            _cardModel.IsHide = true;
            
            ExecuteEvents.Execute(_cardModel.InputHandler.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.dragHandler);             
        }

        private void setCurrentTile(TMGridManager gridManager)
        {
            if (!gridManager.TryGetTileDataByRay(_mainCamera.ScreenPointToRay(Input.mousePosition), out GridTile tileData)) return;
        
            _currentTile = tileData;
            setTileHighlight(tileData);
        }

        private void onDrag(PointerEventData dragEventData)
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

                    if (hit.collider.gameObject.name == "Tile" && _currentTile)
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

                    if (_currentTile)
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
                _currentTile.Properties.Add("TileOff");
                _building.transform.SetParent(_currentTile.TileRenderer.transform);
                _building.Initialize(BuildingData);
                _building.BatchOnTile();

                bool hasCardManager = ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager);

                if (hasCardManager)
                {
                    cardManager.RemoveCard(_cardModel);
                }

                _cardModel.gameObject.SetActive(false);
                
                GridTile tile = _currentTile;
                tile.OnMouseDownTile.AddListener(onMouseDownTile);
                
                void onMouseDownTile(GridTile tileData)
                {
                    if (!hasCardManager) return;

                    tile.OnMouseDownTile.RemoveListener(onMouseDownTile);
                    tile.Properties.Remove("TileOff");
                    
                    _cardModel.gameObject.SetActive(true);
                    _cardModel.IsHide = true;

                    cardManager.AddCard(_cardModel);
                    
                    ExecuteEvents.Execute(
                        _cardModel.InputHandler.gameObject, 
                        new PointerEventData(EventSystem.current), 
                        ExecuteEvents.pointerDownHandler);             
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

        private void setTileHighlight(GridTile tile)
        {
            _currentTile = tile;

            tile.TileRenderer.material.color =
                tile.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?
                    Color.yellow :
                    Color.red;
        }

        private static void setTileUnHighlight(GridTile tile)
        {
            tile.TileRenderer.material.color = Color.white;
        }

        public void Dispose()
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            gridManager.OnHighlightTile.RemoveListener(setTileHighlight);
            gridManager.OnExitTile.RemoveListener(setTileUnHighlight);
            Object.Destroy(Building.gameObject);
        }
    }
}