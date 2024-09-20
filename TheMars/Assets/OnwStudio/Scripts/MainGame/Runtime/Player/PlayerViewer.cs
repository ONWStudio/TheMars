using System.Collections;
using Onw.Attribute;
using TM.Manager;
using UnityEngine;
using TMPro;
using VContainer;

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

        [SerializeField, ReadOnly, Inject] private PlayerManager _playerManager;
        [SerializeField, ReadOnly, Inject] private TMSimulator _simulator;
        
        private void Start()
        {
            _playerManager.OnChangedLevel += level => _levelText.text = $"Lv. {level}";
            _playerManager.OnChangedMarsLithium += marsLithium => _marsLithiumText.text = marsLithium.ToString();
            _playerManager.OnChangedCredit += credit => _creditText.text = credit.ToString();
            _playerManager.OnChangedPopulation += population => _populationText.text = $"{population} / {_playerManager.TotalPopulation}";
            _playerManager.OnChangedTotalPopulation += totalPopulation => _populationText.text = $"{_playerManager.Population} / {totalPopulation}";
            _playerManager.OnChangedSteel += steel => _steelText.text = steel.ToString();
            _playerManager.OnChangedPlants += plants => _plantsText.text = plants.ToString();
            _playerManager.OnChangedClay += clay => _clayText.text = clay.ToString();
            _playerManager.OnChangedElectricity += electricity => _electricityText.text = electricity.ToString();
            _simulator.OnChangedSeconds += seconds => _timeText.text = buildTimeText(_simulator.NowDay, _simulator.NowMinutes, seconds);
            _simulator.OnChangedMinutes += minutes => _timeText.text = buildTimeText(_simulator.NowDay, minutes, _simulator.NowSeconds);
            _simulator.OnChangedDay += day => _timeText.text = buildTimeText(day, _simulator.NowMinutes, _simulator.NowSeconds);
            _timeText.text = buildTimeText(_simulator.NowDay, _simulator.NowMinutes, _simulator.NowSeconds);

            static string buildTimeText(int day, int minutes, int seconds) => $"{day} day {minutes}m {seconds}s";
        }
        
        
    }
}