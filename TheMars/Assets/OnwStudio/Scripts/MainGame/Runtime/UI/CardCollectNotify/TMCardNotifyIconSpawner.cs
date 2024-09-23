using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Onw.UI;
using Onw.Manager.ObjectPool;
using TM.Manager;
using TM.Runtime.UI;
using TM.Card.Runtime;

namespace TM.Runtime
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

        [FormerlySerializedAs("_onCreatePreIcon")]
        [SerializeField] private UnityEvent<TMCardCollectNotifyIcon> _onCreateIcon = new();
        
        private void Start()
        {
            TMSimulator.Instance.OnChangedDay += onChangedDay;
            onChangedDay(1);

            void onChangedDay(int day)
            {
                if (!GenericObjectPool<TMCardCollectNotifyIcon>.TryPop(out TMCardCollectNotifyIcon iconInstance))
                {
                    iconInstance = Instantiate(_iconPrefab.gameObject)
                        .GetComponent<TMCardCollectNotifyIcon>();
                    
                    _onCreateIcon.Invoke(iconInstance);
                }
                        
                iconInstance.transform.SetParent(TMCardManager.Instance.UIComponents.CardCollectIconScrollView.content, false);
                RectTransform content = TMCardManager
                    .Instance
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