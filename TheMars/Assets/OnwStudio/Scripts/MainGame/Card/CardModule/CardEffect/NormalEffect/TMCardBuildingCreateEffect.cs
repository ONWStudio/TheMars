using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Onw.Helper;
using Onw.HexGrid;
using Onw.Attribute;
using TM.Grid;
using TM.Building;
using TM.Card.Runtime;
using TM.Card.Effect.Creator;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
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
            public IHexGrid Prev { get; }
            public Vector3 PrevPosition { get; }

            public PrevTileData(IHexGrid prev, in Vector3 prevPosition)
            {
                Prev = prev;
                PrevPosition = prevPosition;
            }
        }

        private const int TILE_FIND_LAYER_MASK = 1 << 3 | 1 << 0;
        private const int TILE_BATCH_LAYER_MASK = 1 << 3;

        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; } = null;

        public string Description => "";
        public TMBuilding Building => _building;

        [SerializeField, ReadOnly] private TMBuilding _building = null;

        private TMCardModel _cardModel = null;
        private Camera _mainCamera = null;
        private PrevTileData? _prevTileData = null;

        public void Initialize(TMCardBuildingCreateEffectCreator effectCreator)
        {
            BuildingData = effectCreator.BuildingData; // .. 데이터에서 필요한 값 받아오기
        }

        public void ApplyEffect(TMCardModel cardModel, ITMCardEffectTrigger trigger)
        {
            if (!Application.isPlaying || !BuildingData) return;

            _mainCamera = Camera.main; // .. 메인 카메라 캐시

            _cardModel = cardModel;                           // .. 효과를 가지고 있는 카드 캐시
            _cardModel.OnEffectEvent += onEffect;             // .. 효과 발동 이벤트 메서드 추가
            _cardModel.InputHandler.DownAction += onDownCard; // .. 카드 클릭 이벤트에 메서드 추가
            _cardModel.InputHandler.DragAction += onDrag;     // .. 드래그 이벤트에 메서드 추가

            if (BuildingData.BuildingPrefab) // .. 빌딩 프리팹이 존재할때
            {
                _building = Object
                    .Instantiate(BuildingData.BuildingPrefab.gameObject)
                    .GetComponent<TMBuilding>()
                    .Initialize(BuildingData); // .. 프리팹 인스턴스화 후 초기화

                float xWidth = _building.MeshRenderer.bounds.size.x; // .. 빌딩의 너비x
                float zWidth = _building.MeshRenderer.bounds.size.z; // .. 빌딩의 너비z
                float tileSize = TMGridManager.Instance.TileSize;    // .. 현재 타일 사이즈
                float ratio = tileSize / Mathf.Max(xWidth, zWidth);  // .. 타일 사이즈 나누기 너비로 비율 구하기

                _building.transform.localScale *= ratio * 0.55f; // .. 어떤 건물이든 항상 동일한 폭으로 생성
                _building.gameObject.SetActive(false);           // .. 인스턴스화 된 건물 비활성화
            }
        }

        private void onDownCard(PointerEventData eventData)
        {
            if (!_cardModel.CanInteract) return; // .. 카드의 상호작용이 불가능한 상태일때는 이벤트 트리거x

            _building.gameObject.SetActive(true); // .. 드래그 활성화 타이밍이므로 건물 활성화
            _cardModel.IsHide = true;             // .. 카드에 가려보이면 안되므로 카드 렌더링 x

            onDrag(null); // .. 드래그 이벤트
        }

        private void onDrag(PointerEventData _)
        {
            if (!_cardModel.CanInteract) return; // .. 카드가 상호작용 불가능 상태일때는 이벤트 트리거x

            if (!_cardModel.IsOverTombTransform) // .. 카드가 버리기(쓰레기통)칸 위에 있지 않을 경우
            {
                if (!_building.MeshRenderer.enabled) // .. 빌딩이 렌더링 되고 있지 않다면
                {
                    _building.MeshRenderer.enabled = true; // .. 렌더링 활성화
                }

                Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); // .. 레이 생성

                if (TMGridManager.Instance.TryGetTileDataByRay(ray, out (bool, RaycastHit) hitTuple, out IHexGrid hex))
                {
                    _building.transform.position = hex.TilePosition; // .. 건물 포지션 세팅
                }
                else
                {
                    if (hitTuple.Item1)
                    {
                        _building.transform.position = hitTuple.Item2.point;
                    }
                }
            }
            else // .. 카드가 버리기(쓰레기통)칸 위에 위치한 경우
            {
                if (_building.MeshRenderer.enabled) // .. 건물이 렌더링 중인 경우 
                {
                    _building.MeshRenderer.enabled = false; // .. 건물 비활성화
                }
            }
        }

        // .. 효과 발동시
        private void onEffect(TMCardModel card)
        {
            if (!_cardModel.CanInteract) return; // .. 카드가 상호작용 불가능 상태인 경우 트리거x

            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); // .. 레이 생성
            Vector3 keepPosition = _cardModel.transform.localPosition;                  // .. 카드의 현재 포지션을 캐싱

            if (TMGridManager.Instance.TryGetTileDataByRay(ray, out (bool, RaycastHit) hit, out IHexGrid hex) && // .. 레이캐스팅
                _prevTileData?.Prev != hex &&                                                                    // .. 이전에 선택된 타일과 현재 선택된 타일이 같지 않고
                (hex?.Properties.All(property => property is not "DefaultBuilding" and not "TileOff") ?? false)) // .. 현재 선택된 타일이 건물 설치 가능한 타일일때
            {
                if (_prevTileData is not null)
                {
                    _building.MeshRenderer.enabled = true;                      // .. 건물에 배치되었다 알려주기
                    TMGridManager.Instance.AddBuildingWithoutNotify(_building); // .. 타일에 해당 건물이 배치되었다 알려주기
                }
                else
                {
                    _building.BatchOnTile();
                    TMGridManager.Instance.AddBuilding(_building);
                }

                // .. 카드가 코스트를 지불하지 않았고 코스트를 지불 가능한 상태라면?
                if (!_cardModel.WasCostPaid && _cardModel.CanPayCost) // .. 건물의 경우는 첫설치에만 코스트를 지불하기 때문에 코스트 지불 검사
                {
                    _cardModel.WasCostPaid = true; // .. 코스트 지불
                    _cardModel.PayCost();          // .. 실제 코스트 지불 계산
                }

                batchBuildingOnTile(hex); // .. 건물이 배치된 타일 설정하기
            }
            else // .. 만약 타일이 아니라면
            {
                if (_prevTileData is not null) // .. 이전에 건물이 설치되었던 타일이 있다면
                {
                    PrevTileData prevTileData = (PrevTileData)_prevTileData;  // .. Nullable이므로 타입 캐스팅
                    _building.transform.position = prevTileData.PrevPosition; // .. 건물의 포지션 이전 타일의 위치로
                    batchBuildingOnTile(prevTileData.Prev);                   // .. 건물이 배치된 타일 설정하기
                }
                else
                {
                    _building.gameObject.SetActive(false); // .. 이전에 건물이 설치되었던 타일이 없다면 건물 비활성화
                }
            }

            _cardModel.IsHide = false; // .. 카드 렌더링 비활성화

            void onMouseDownTile(IHexGrid tileData) // .. 건물이 배치된 타일에 클릭 이벤트 추가
            {
                if (EventSystem.current.IsPointerOverGameObject()) return; // .. UI가 겹쳐진 상태일때 UI가 먼저 상호작용 되어야 하므로 겹친 상태일때는 트리거x

                _prevTileData = new(tileData, _building.transform.position); // .. 설치되었던 타일의 데이터 미리 캐싱
                tileData.OnMouseDownTile -= onMouseDownTile;                 // .. 해당 타일은 이제 건물이 있지 않은 상태이므로 이벤트에서 딜리게이트 제거
                tileData.RemoveProperty("TileOff");                          // .. 타일은 더 이상 설치 불가능한 상태가 아니므로 TileOff속성 제거
                _cardModel.transform.localPosition = keepPosition;           // .. 카드를 설치 이전 상태의 포지션으로 재위치
                TMCardManager.Instance.AddCard(_cardModel);                  // .. 카드는 다시 패에 돌아올 수 있으므로 Add
                _cardModel.TriggerSelectCard();                              // .. 현재 카드가 선택되었다고 카드에게 알려주기
                onDownCard(null);                                   // .. 이벤트 메서드 직접 호출
            }

            void batchBuildingOnTile(IHexGrid currentTile) // .. 건물이 타일위에 배치되었을경우 해주어야할 처리를 하는 메서드
            {
                currentTile.AddProperty("TileOff"); // .. 타일에 건물이 설치되었으므로 다른 건물을 배치할 수 없게 속성 추가

                _cardModel.transform.localPosition = new(1000, 1000, 100000);                 // .. 카드는 화면에 보이지 않는 포지션으로 위치 변경
                _cardModel.CardBodyMover.TargetPosition = _cardModel.transform.localPosition; // .. 무버 타겟 포지션 설정
                TMCardManager.Instance.RemoveCard(_cardModel);                                // .. 카드는 건물을 설치함으로써 패에는 존재하면 안되므로 패에서 카드 제거

                currentTile.OnMouseDownTile += onMouseDownTile; // .. 건물을 철거하거나 재배치를 시켜주기 위해 건물이 설치된 타일에 이벤트 추가
                _prevTileData = null;                           // .. 이전에 건물이 설치되었던 타일이 있을경우 해당 타일은 더 이상 사용할 수 없으므로 null 초기화
            }
        }


        public void Dispose()
        {
            TMGridManager.Instance.RemoveBuilding(_building);       // .. 해당 카드는 파괴되었으므로 건물 제거
            OnwUnityHelper.DestroyObjectByComponent(ref _building); // .. 해당 카드와 연결된 건물 파괴
        }
    }
}