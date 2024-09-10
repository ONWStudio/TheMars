using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Onw.Coroutine;
using Onw.ServiceLocator;
using Onw.Helper;
using Onw.UI;
using TM.Manager;
using TMCard.Runtime;
using UnityEngine;

namespace TM
{
    public sealed class TMCardNotifyIconSpawner : MonoBehaviour
    {
        private const float REPEAT_TIME_MIN = 5f;
        private const float REPEAT_TIME_MAX = 10f;

        [SerializeField] private TMCardCollectNotifyIcon _iconPrefab = null;
        [SerializeField, Range(REPEAT_TIME_MIN, REPEAT_TIME_MAX)] private float _repeatTime = 5f;

        private void Start()
        {
            TMSimulator simulator = null;

            this.WaitCompletedConditions(
                () => ServiceLocator<TMSimulator>.TryGetService(out simulator),
                () => simulator.OnChangedDay += onChangedDay);

            onChangedDay(1);

            void onChangedDay(int day)
            {
                TMCardManager cardManager = null;
                this.WaitCompletedConditions(
                    () => ServiceLocator<TMCardManager>.TryGetService(out cardManager),
                    () =>
                    {
                        TMCardCollectNotifyIcon iconInstance = Instantiate(_iconPrefab.gameObject)
                            .GetComponent<TMCardCollectNotifyIcon>();
                        
                        iconInstance.transform.SetParent(cardManager.UIComponents.CardCollectIconScrollView.content, false);
                        RectTransform content = cardManager
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
                    });
            }
        }
    }
}