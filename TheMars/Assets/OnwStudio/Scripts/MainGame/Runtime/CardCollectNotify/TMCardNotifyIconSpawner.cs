using Onw.UI;
using Onw.Attribute;
using Onw.Coroutine;
using Onw.Manager.ObjectPool;
using TM.Manager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VContainer;

namespace TM.Card.Runtime
{
    public sealed class TMCardNotifyIconSpawner : MonoBehaviour
    {
        private const float REPEAT_TIME_MIN = 5f;
        private const float REPEAT_TIME_MAX = 10f;

        public event UnityAction<TMCardCollectNotifyIcon> OnCreateIcon
        {
            add => _onCreateIcon.AddListener(value);
            remove => _onCreateIcon.RemoveListener(value);
        }
        
        [SerializeField] private TMCardCollectNotifyIcon _iconPrefab = null;
        [SerializeField, Range(REPEAT_TIME_MIN, REPEAT_TIME_MAX)] private float _repeatTime = 5f;
        [SerializeField, ReadOnly, Inject] private TMSimulator _simulator;
        [SerializeField, ReadOnly, Inject] private TMCardManager _cardManager;

        [FormerlySerializedAs("_onCreatePreIcon")]
        [SerializeField] private UnityEvent<TMCardCollectNotifyIcon> _onCreateIcon = new();
        
        private void Start()
        {
            _simulator.OnChangedDay += onChangedDay;
            onChangedDay(1);

            void onChangedDay(int day)
            {
                if (!GenericObjectPool<TMCardCollectNotifyIcon>.TryPop(out TMCardCollectNotifyIcon iconInstance))
                {
                    iconInstance = Instantiate(_iconPrefab.gameObject)
                        .GetComponent<TMCardCollectNotifyIcon>();
                    
                    _onCreateIcon.Invoke(iconInstance);
                }
                        
                iconInstance.transform.SetParent(_cardManager.UIComponents.CardCollectIconScrollView.content, false);
                RectTransform content = _cardManager
                    .UIComponents
                    .CardCollectIconScrollView
                    .content;

                content.sizeDelta = new(content.GetChildWidthSum(), content.sizeDelta.y);
                        
                Vector3[] positionArray = content
                    .GetHorizontalSortedPosition();

                for (int i = 0; i < positionArray.Length; i++)
                {
                    TMCardCollectNotifyIcon icon = content
                        .GetChild(i)
                        .GetComponent<TMCardCollectNotifyIcon>();
                            
                    icon.SetTargetLocalPosition(positionArray[i]);
                }
            }
        }
    }
}