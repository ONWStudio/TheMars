using System.Collections;
using Onw.Attribute;
using Onw.Extensions;
using Onw.Helper;
using Onw.ServiceLocator;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
using UniRx;

namespace TM.UI
{
    public sealed class PlayerViewer : MonoBehaviour
    {
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _levelText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _marsLithiumText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _creditText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _populationText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _steelText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _plantsText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _clayText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _electricityText;

        private IEnumerator Start()
        {
            PlayerManager player = null;
            
            yield return new WaitUntil(() => ServiceLocator<PlayerManager>.TryGetService(out player));

            player.OnChangedLevel += level => _levelText.text = $"Lv. {level}";
            player.OnChangedMarsLithium += marsLithium => _marsLithiumText.text = marsLithium.ToString();
            player.OnChangedCredit += credit => _creditText.text = credit.ToString();
            player.OnChangedPopulation += population => _populationText.text = $"{population} / {player.TotalPopulation}";
            player.OnChangedTotalPopulation += totalPopulation => _populationText.text = $"{player.Population} / {totalPopulation}";
            player.OnChangedSteel += steel => _steelText.text = steel.ToString();
            player.OnChangedPlants += plants => _plantsText.text = plants.ToString();
            player.OnChangedClay += clay => _clayText.text = clay.ToString();
            player.OnChangedElectricity += electricity => _electricityText.text = electricity.ToString();
        }
    }
}