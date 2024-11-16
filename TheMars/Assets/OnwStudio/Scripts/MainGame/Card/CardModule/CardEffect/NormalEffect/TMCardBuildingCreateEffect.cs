using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using Onw.Helper;
using Onw.HexGrid;
using Onw.Attribute;
using Onw.Extensions;
using TM.Grid;
using TM.Building;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;
using Object = UnityEngine.Object;
using UnityEngine.InputSystem;

namespace TM.Card.Effect
{
    /// <summary>
    /// .. 건물 설치 효과입니다 건물을 배치하고 배치시 코스트 사용, 조건 검사, 재배치시의 관한 로직, 회수등의 관한 로직을 해당 효과가 모두 담당합니다
    /// </summary>
    public sealed class TMCardBuildingCreateEffect : ITMNormalEffect, ITMCardInitializeEffect<TMCardBuildingCreateEffectCreator>, IDisposable
    {
        public readonly struct PrevTileData
        {
            public IHexGrid Prev { get; }
            public Vector3 PrevPosition { get; }

            public PrevTileData(IHexGrid prev, in Vector3 prevPosition)
            {
                Prev = prev;
                PrevPosition = prevPosition;
            }
        }

        public event LocalizedString.ChangeHandler OnChangedDescription
        {
            add => _localizedDescription.StringChanged += value;
            remove => _localizedDescription.StringChanged -= value;
        }

        private PrevTileData? _prevTileData = null;
        private IHexGrid _currentHex = null;
        private TMCardModel _cardModel = null;
        private Camera _mainCamera = null;
        private float _buildingHalfHeight = 0;

        [SerializeField, ReadOnly] private TMBuilding _building = null;
        [field: SerializeField] private LocalizedString _localizedDescription = new("TM_Card_Effect", "Building_Create_Effect");

        public TMBuilding Building => _building;

        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; } = null;

        public bool CanUseEffect => _currentHex is not null && _prevTileData?.Prev != _currentHex;
        private bool _isFired = false;


        public void Initialize(TMCardBuildingCreateEffectCreator effectCreator)
        {
            BuildingData = effectCreator.BuildingData; // .. 데이터에서 필요한 값 받아오기
        }

        public void ApplyEffect(TMCardModel cardModel)
        {
            if (!Application.isPlaying || !BuildingData) return;

            _mainCamera = Camera.main; // .. 메인 카메라 캐시

            _cardModel = cardModel;                           // .. 효과를 가지고 있는 카드 캐시
            _cardModel.OnSafePointerDownEvent += onSafeDownCard; // .. 카드 클릭 이벤트에 메서드 추가
            _cardModel.OnSafeDragEvent += onSafeDrag;     // .. 드래그 이벤트에 메서드 추가
            _cardModel.OnDragEndCard += onDragEndCard;

            if (BuildingData.BuildingPrefab) // .. 빌딩 프리팹이 존재할때
            {
                _building = Object
                    .Instantiate(BuildingData.BuildingPrefab.gameObject)
                    .GetComponent<TMBuilding>()
                    .Initialize(BuildingData); // .. 프리팹 인스턴스화 후 초기화

                _cardModel.IsOverTombTransform.AddListener(onChangedIsOverTombTransform);

                Vector3 totalSize = VerticesTo.GetTotalSize(_building.gameObject);

                float xWidth = totalSize.x; // .. 빌딩의 너비x
                float zWidth = totalSize.z; // .. 빌딩의 너비z

                float tileSize = TMGridManager.Instance.TileSize;    // .. 현재 타일 사이즈
                float ratio = tileSize / Mathf.Max(xWidth, zWidth) * 0.55f;  // .. 타일 사이즈 나누기 너비로 비율 구하기

                _building.transform.localScale *= ratio; // .. 어떤 건물이든 항상 동일한 폭으로 생성
                _buildingHalfHeight = _building.transform.position.y - VerticesTo.GetMinPoint(_building.gameObject).y;
                _building.SetActiveGameObject(false);           // .. 인스턴스화 된 건물 비활성화

                _localizedDescription.Arguments = new object[]
                {
                    new 
                    {
                        _building.BuildingData.BuildingName,
                        BuildingEffects = _building.LocalizedEffectDescription
                    }
                };

                void onChangedIsOverTombTransform(bool isOverTombTransform)
                {
                    if (isOverTombTransform)
                    {
                        if (_building.IsRender)
                        {
                            _building.IsRender = false;
                        }
                    }
                    else
                    {
                        if (!_building.IsRender)
                        {
                            _building.IsRender = true;
                        }
                    }
                }
            }
        }

        private void onDragEndCard(TMCardModel card)
        {
            if (!_isFired)
            {

                if (card.IsOverCollectTransform.Value)
                {
                    Debug.Log("??");
                    _prevTileData = null;
                }

                undoBatchTile();
            }

            _isFired = false;
        }

        private void onSafeDownCard(TMCardModel card)
        {
            _building.SetActiveGameObject(true);
            _cardModel.IsHide.Value = true;             // .. 카드에 가려보이면 안되므로 카드 렌더링 x

            onSafeDrag(card); // .. 드래그 이벤트
        }

        private void onSafeDrag(TMCardModel card)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); // .. 레이 생성

            if (TMGridManager.Instance.TryGetTileDataByRay(ray, out (bool, RaycastHit) hitTuple, out IHexGrid hex) && hex.IsActive)
            {
                _currentHex = hex.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ? hex : null;
                _building.SetPosition(new(hex.TilePosition.x, hex.TilePosition.y + _buildingHalfHeight, hex.TilePosition.z));
            }
            else
            {
                _currentHex = null;
                if (hitTuple.Item1)
                {
                    Vector3 groundPoint = hitTuple.Item2.point;
                    _building.SetPosition(new(groundPoint.x, groundPoint.y + _buildingHalfHeight, groundPoint.z));
                }
            }
        }

        // .. 효과 발동시
        public void OnEffect(TMCardModel card)
        {
            Debug.Log(card.CanPayCost);
            _building.IsRender = true;
            _building.BatchOnTile();
 
            _prevTileData = null;
            _isFired = true;

            TMGridManager.Instance.AddBuilding(_building); // .. 타일에 해당 건물이 배치되었다 알려주기
            batchBuildingOnTile(_currentHex); // .. 건물이 배치된 타일 설정하기
        }

        private void onMouseDownTile(IHexGrid tileData) // .. 건물이 배치된 타일에 클릭 이벤트 추가
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            _prevTileData = new(tileData, _building.GetPosition()); // .. 설치되었던 타일의 데이터 미리 캐싱

            tileData.OnMouseDownTile -= onMouseDownTile;                 // .. 해당 타일은 이제 건물이 있지 않은 상태이므로 이벤트에서 딜리게이트 제거
            tileData.RemoveProperty("TileOff");                          // .. 타일은 더 이상 설치 불가능한 상태가 아니므로 TileOff속성 제거

            _cardModel.SetActiveGameObject(true);
            _cardModel.TriggerSelectCard();

            TMCardManager.Instance.AddCard(_cardModel);                  // .. 카드는 다시 패에 돌아올 수 있으므로 Add
            _currentHex = null;
        }

        private void batchBuildingOnTile(IHexGrid currentTile) // .. 건물이 타일위에 배치되었을경우 해주어야할 처리를 하는 메서드
        {
            currentTile.AddProperty("TileOff"); // .. 타일에 건물이 설치되었으므로 다른 건물을 배치할 수 없게 속성 추가
            currentTile.OnMouseDownTile += onMouseDownTile; // .. 건물을 철거하거나 재배치를 시켜주기 위해 건물이 설치된 타일에 이벤트 추가

            _cardModel.SetActiveGameObject(false);
            _cardModel.CardBodyMover.TargetPosition = _cardModel.GetLocalPosition(); // .. 무버 타겟 포지션 설정

            TMCardManager.Instance.RemoveCard(_cardModel);  // .. 카드는 건물을 설치함으로써 패에는 존재하면 안되므로 패에서 카드 제거
            _prevTileData = null;                           // .. 이전에 건물이 설치되었던 타일이 있을경우 해당 타일은 더 이상 사용할 수 없으므로 null 초기화
        }

        private void undoBatchTile()
        {
            if (!_building) return;

            if (_prevTileData is { } prevTileData) // .. 이전에 건물이 설치되었던 타일이 있다면
            {
                _building.IsRender = true;
                _building.SetPosition(prevTileData.PrevPosition); // .. 건물의 포지션 이전 타일의 위치로

                batchBuildingOnTile(prevTileData.Prev);                   // .. 건물이 배치된 타일 설정하기
            }
            else
            {
                _building.SetActiveGameObject(false); // .. 이전에 건물이 설치되었던 타일이 없다면 건물 비활성화
                _cardModel.IsHide.Value = false;
            }
        }

        public void Dispose()
        {
            TMGridManager.Instance.RemoveBuilding(_building);       // .. 해당 카드는 파괴되었으므로 건물 제거
            OnwUnityHelper.DestroyObjectByComponent(ref _building); // .. 해당 카드와 연결된 건물 파괴
        }
    }
}