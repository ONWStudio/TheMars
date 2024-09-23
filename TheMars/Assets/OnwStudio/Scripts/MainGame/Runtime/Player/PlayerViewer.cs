using System.Collections;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

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
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _timeText;

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
            TMSimulator.Instance.OnChangedSeconds += seconds => _timeText.text = buildTimeText(TMSimulator.Instance.NowDay, TMSimulator.Instance.NowMinutes, seconds);
            TMSimulator.Instance.OnChangedMinutes += minutes => _timeText.text = buildTimeText(TMSimulator.Instance.NowDay, minutes, TMSimulator.Instance.NowSeconds);
            TMSimulator.Instance.OnChangedDay += day => _timeText.text = buildTimeText(day, TMSimulator.Instance.NowMinutes, TMSimulator.Instance.NowSeconds);
            _timeText.text = buildTimeText(TMSimulator.Instance.NowDay, TMSimulator.Instance.NowMinutes, TMSimulator.Instance.NowSeconds);

            static string buildTimeText(int day, int minutes, int seconds) => $"{day} day {minutes}m {seconds}s";
        }


    }
}