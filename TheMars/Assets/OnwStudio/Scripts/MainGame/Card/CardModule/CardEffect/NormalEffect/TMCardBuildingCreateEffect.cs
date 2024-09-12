using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Onw.Helper;
using Onw.GridTile;
using Onw.Attribute;
using Onw.Extensions;
using Onw.ServiceLocator;
using TM.Grid;
using TM.Building;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;
using Object = UnityEngine.Object;

namespace TM.Card.Effect
{
    /// <summary>
    /// .. 건물 설치 효과입니다 건물을 배치하고 배치시 코스트 사용, 조건 검사, 재배치시의 관한 로직, 회수등의 관한 로직을 해당 효과가 모두 담당합니다
    /// </summary>
    public sealed class TMCardBuildingCreateEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardBuildingCreateEffectCreator>, IDisposable
    {
        public readonly struct PrevTileData
        {
            public GridTile PrevTile { get; }
            public Vector3 PrevPosition { get; }

            public PrevTileData(GridTile prevTile, in Vector3 prevPosition)
            {
                PrevTile = prevTile;
                PrevPosition = prevPosition;
            }
        }
        
        private const int TILE_FIND_LAYER_MASK = 1 << 3 | 1 << 0;
        private const int TILE_BATCH_LAYER_MASK = 1 << 3;
        
        public string Description => "";

        public TMBuilding Building => _building;

        [SerializeField, ReadOnly] private TMBuilding _building = null;
        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; } = null;

        private TMCardModel _cardModel = null;
        private GridTile _currentTile = null;
        private Camera _mainCamera = null;
        private PrevTileData? _prevTileData = null;

        public void Initialize(TMCardBuildingCreateEffectCreator effectCreator)
        {
            BuildingData = effectCreator.BuildingData;
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            _mainCamera = Camera.main;

            _cardModel = cardModel;
            
            _cardModel.OnEffectEvent += onEffect;
            _cardModel.InputHandler.DownAction += onDownCard;
            _cardModel.InputHandler.DragAction += onDrag;

            if (Application.isPlaying)
            {
                _building = Object
                    .Instantiate(BuildingData.BuildingPrefab.gameObject)
                    .GetComponent<TMBuilding>();
                
                _building.Initialize(BuildingData);
            }

            if (ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager))
            {
                float xWidth = _building.MeshRenderer.bounds.size.x;
                float zWidth = _building.MeshRenderer.bounds.size.z;
                float tileSize = gridManager.TileSize;
                float ratio = tileSize / Mathf.Max(xWidth, zWidth);
                _building.transform.localScale *= ratio * 0.55f; // .. 어떤 건물이든 항상 동일한 폭으로 생성
            }
            
            _building.gameObject.SetActive(false);
        }

        private void onDownCard(PointerEventData eventData)
        {
            if (!ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

            gridManager.OnHighlightTile += setTileHighlight;
            gridManager.OnExitTile += setTileUnHighlight;

            _building.gameObject.SetActive(true);
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

            if (!_cardModel.IsOverTombTransform)
            {
                if (!_building.MeshRenderer.enabled)
                {
                    _building.MeshRenderer.enabled = true;
                    gridManager.OnHighlightTile += setTileHighlight;
                    setCurrentTile(gridManager);
                }

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, TILE_FIND_LAYER_MASK))
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
                if (_building.MeshRenderer.enabled)
                {
                    _building.MeshRenderer.enabled = false;
                    gridManager.OnHighlightTile -= setTileHighlight;

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
            Vector3 keepPosition = _cardModel.transform.localPosition;
            bool hasCardManager = ServiceLocator<TMCardManager>.TryGetService(out TMCardManager cardManager);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, TILE_BATCH_LAYER_MASK) &&
                _prevTileData?.PrevTile != _currentTile &&
                _currentTile &&
                _currentTile.Properties.All(property => property is not "DefaultBuilding" and not "TileOff"))
            {
                _building.transform.SetParent(_currentTile.TileRenderer.transform);
                _building.BatchOnTile();
                gridManager.AddBuilding(_building);
                
                // .. 카드가 코스트를 지불하지 않았고 코스트를 지불 가능한 상태라면?
                if (!_cardModel.WasCostPaid && _cardModel.CanPayCost)
                {
                    _cardModel.WasCostPaid = true; // .. 코스트 지불
                    _cardModel.PayCost(); // .. 실제 코스트 지불 계산
                }
                
                batchBuildingOnTile(_currentTile);
            }
            else
            {
                if (_prevTileData is not null)
                {
                    PrevTileData prevTileData = (PrevTileData)_prevTileData;
                    _building.transform.position = prevTileData.PrevPosition;
                    batchBuildingOnTile(prevTileData.PrevTile);
                }
                else
                {
                    _building.gameObject.SetActive(false);
                }
            }

            _currentTile = null;
            _cardModel.IsHide = false;

            gridManager.OnHighlightTile -= setTileHighlight;
            gridManager.OnExitTile -= setTileUnHighlight;
            gridManager
                .ReadOnlyTileList
                .SelectMany(rows => rows.Rows)
                .ForEach(setTileUnHighlight);
            
            void onMouseDownTile(GridTile tileData)
            {
                if (EventSystem.current.IsPointerOverGameObject() || !hasCardManager) return;

                _prevTileData = new(tileData, _building.transform.position);
                _currentTile = tileData;
                _currentTile.OnMouseDownTile -= onMouseDownTile;
                _currentTile.Properties.Remove("TileOff");
                _cardModel.transform.localPosition = keepPosition;
                cardManager.AddCard(_cardModel);
                _cardModel.TriggerSelectCard();
                onDownCard(null);
            }

            void batchBuildingOnTile(GridTile currentTile)
            {
                currentTile.Properties.Add("TileOff");
                
                _cardModel.transform.localPosition = new(1000, 1000, 100000);
                _cardModel.CardBodyMover.TargetPosition = _cardModel.transform.localPosition;
                    
                if (hasCardManager)
                {
                    cardManager.RemoveCard(_cardModel);
                }
                    
                currentTile.OnMouseDownTile += onMouseDownTile;
                _prevTileData = null;
            }
        }

        private void setTileHighlight(GridTile tile)
        {
            _currentTile = tile;

            tile.TileRenderer.material.color =
                tile.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?
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

            gridManager.OnHighlightTile -= setTileHighlight;
            gridManager.OnExitTile -= setTileUnHighlight;
            gridManager.RemoveBuilding(_building);
            OnwUnityHelper.DestroyObjectByComponent(ref _building);
        }
    }
}