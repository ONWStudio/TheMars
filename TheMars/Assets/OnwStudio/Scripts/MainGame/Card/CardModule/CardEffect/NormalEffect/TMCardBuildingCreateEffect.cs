using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Attribute;
using Onw.Extensions;
using Onw.GridTile;
using Onw.ServiceLocator;
using TM.Building;
using TM.Grid;
using TMCard.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMCard.Effect
{
    public sealed class TMCardBuildingCreateEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardBuildingCreateEffectCreator>
    {
        public string Description => "";

        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; } = null;
        [field: SerializeField, ReadOnly] public TMBuilding Building { get; private set; } = null;

        public void ApplyEffect(TMCardModel model, ITMEffectTrigger trigger)
        {
            model.InputHandler.DownAction.AddListener(onDownCard);

            void onDownCard(PointerEventData eventData)
            {
                if (!model.CardViewMover.enabled || !ServiceLocator<TMGridManager>.TryGetService(out TMGridManager gridManager)) return;

                gridManager.OnHighlightTile.AddListener(setTileHighlight);
                gridManager.OnExitTile.AddListener(setTileUnHighlight);

                Camera cardSystemCamera = eventData.enterEventCamera;
                Camera mainCamera = Camera.main;

                if (!Building)
                {
                    Building = Object
                        .Instantiate(model.CardData.BuildingData.BuildingPrefab.gameObject)
                        .GetComponent<TMBuilding>();
                }

                MeshRenderer meshRenderer = Building.GetComponent<MeshRenderer>();
                TileData? currentTile = null;

                float xWidth = meshRenderer.bounds.size.x;
                float zWidth = meshRenderer.bounds.size.z;
                float tileSize = gridManager.TileSize;
                float ratio = tileSize / Mathf.Max(xWidth, zWidth);
                Building.transform.localScale *= ratio * 0.55f; // .. 어떤 건물이든 항상 동일한 폭으로 생성

                model.InputHandler.DragAction.AddListener(onDrag);
                model.InputHandler.UpAction.AddListener(onDragEnd);

                void onDrag(PointerEventData dragEventData)
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        Vector3 hitPoint = hit.point;
                        if (currentTile is not null && hit.collider.gameObject.name == "Tile")
                        {
                            Vector3 tilePosition = ((TileData)currentTile).TileRenderer.transform.position;
                            float tileHalfSize = gridManager.TileSize * 0.5f;
                            tilePosition.x += tileHalfSize;
                            tilePosition.z += tileHalfSize;
                            hitPoint = new(tilePosition.x, hitPoint.y, tilePosition.z);
                        }

                        Building.transform.position = hitPoint;
                    }
                }

                void onDragEnd(PointerEventData dragEndEventData)
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit) &&
                        hit.collider.gameObject.name == "Tile" &&
                        (currentTile?.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?? false))
                    {
                        TileData buildingTile = (TileData)currentTile;
                        buildingTile.Properties.Add("TileOff");
                        Building.transform.SetParent(buildingTile.TileRenderer.transform);
                        Building.Initialize(model.CardData.BuildingData);
                        Building.BatchOnTile();
                    }
                    else
                    {
                        Object.Destroy(Building.gameObject);
                    }

                    currentTile = null;
                    model.InputHandler.DragAction.RemoveListener(onDrag);
                    model.InputHandler.UpAction.RemoveListener(onDragEnd);

                    gridManager.OnHighlightTile.RemoveListener(setTileHighlight);
                    gridManager.OnExitTile.RemoveListener(setTileUnHighlight);
                    gridManager
                        .ReadOnlyTileList
                        .SelectMany(rows => rows.Rows.Select(tile => tile.GetTileData()))
                        .ForEach(setTileUnHighlight);
                }

                void setTileHighlight(TileData tileArgs)
                {
                    currentTile = tileArgs;

                    tileArgs.TileRenderer.material.color =
                        tileArgs.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?
                            Color.yellow :
                            Color.red;
                }

                void setTileUnHighlight(TileData tileArgs)
                {
                    tileArgs.TileRenderer.material.color = Color.white;
                }
            }
        }
        
        public void Initialize(TMCardBuildingCreateEffectCreator effectCreator)
        {
            BuildingData = effectCreator.BuildingData;
        }
    }
}