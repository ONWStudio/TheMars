using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using Onw.Attribute;

namespace TM.Card.Runtime
{
    public class TMCardCostIcon : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private Image _icon;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _costText;

        private AsyncOperationHandle<Sprite> _costIconHandle;

        public void SetIcon(TMResourceKind kind)
        {
            string spriteKey = kind switch
            {
                TMResourceKind.CREDIT => "Credit_Icon",
                TMResourceKind.ELECTRICITY => "Electricity_Icon",
                TMResourceKind.MARS_LITHIUM => "MarsLithium_Icon[MarsLithium_Icon]",
                TMResourceKind.POPULATION => "Population_Icon",
                TMResourceKind.STEEL => "Steel_Icon",
                TMResourceKind.PLANT => "Plant_Icon",
                TMResourceKind.CLAY => "Clay_Icon",
                TMResourceKind.SATISFACTION => "Satisfaction_Icon",
                _ => null
            };

            if (!string.IsNullOrEmpty(spriteKey))
            {
                _costIconHandle = Addressables.LoadAssetAsync<Sprite>(spriteKey);
                _costIconHandle.Completed += onCostIconLoaded;
            }
        }

        public void SetCost(int cost)
        {
            _costText.text = cost.ToString();
        }

        private void onCostIconLoaded(AsyncOperationHandle<Sprite> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _icon.sprite = handle.Result;
            }
            else
            {
                Debug.LogError("Failed to load sprite: " + handle.Status);
            }
        }

        private void OnDestroy()
        {
            if (!_costIconHandle.IsValid()) return;

            Addressables.Release(_costIconHandle);
        }
    }
}
