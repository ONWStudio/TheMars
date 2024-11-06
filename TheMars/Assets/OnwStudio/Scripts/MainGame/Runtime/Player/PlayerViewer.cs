using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Onw.Attribute;
using TM.Manager;
using UnityEngine.Serialization;

namespace TM.UI
{
    public sealed class PlayerViewer : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _levelText;
        
        [Header("Resource Info")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _marsLithiumText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _creditText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _populationText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _steelText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _plantsText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _clayText;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _electricityText;
        [SerializeField, SelectableSerializeField] private Image _satisfactionProgressImage;
        
        [Header("Time Info")]
        [SerializeField, SelectableSerializeField, FormerlySerializedAs("_timeBarImage")] private Image _timeProgressImage;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _dayText;
        
        [Header("Terraform Info")]
        [SerializeField, SelectableSerializeField] private Image _terraformHexagonImage;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _terraformValueText;

        private void Start()
        {
            TMPlayerManager.Instance.OnChangedLevel += level => _levelText.text = $"Lv. {level}";
            TMPlayerManager.Instance.OnChangedMarsLithium += marsLithium => _marsLithiumText.text = marsLithium.ToString();
            TMPlayerManager.Instance.OnChangedCredit += credit => _creditText.text = credit.ToString();
            TMPlayerManager.Instance.OnChangedPopulation += population => _populationText.text = $"{population} / {TMPlayerManager.Instance.TotalPopulation}";
            TMPlayerManager.Instance.OnChangedTotalPopulation += totalPopulation => _populationText.text = $"{TMPlayerManager.Instance.Population} / {totalPopulation}";
            TMPlayerManager.Instance.OnChangedSteel += steel => _steelText.text = steel.ToString();
            TMPlayerManager.Instance.OnChangedPlants += plants => _plantsText.text = plants.ToString();
            TMPlayerManager.Instance.OnChangedClay += clay => _clayText.text = clay.ToString();
            TMPlayerManager.Instance.OnChangedElectricity += electricity => _electricityText.text = electricity.ToString();
            TMPlayerManager.Instance.OnChangedSatisfaction += satisfaction => _satisfactionProgressImage.fillAmount = satisfaction / (float)TMPlayerManager.MAX_SATISFACTION;
            TMSimulator.Instance.OnChangedSeconds += seconds => _timeProgressImage.fillAmount = TMSimulator.Instance.IntervalInSeconds / (seconds % TMSimulator.Instance.IntervalInSeconds);
            TMSimulator.Instance.OnChangedDay += day => _dayText.text = $"Day {day}";
        }
    }
}