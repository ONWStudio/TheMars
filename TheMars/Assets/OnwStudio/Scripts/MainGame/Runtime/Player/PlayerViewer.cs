using System.Collections;
using Onw.Attribute;
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

        [SerializeField, ReadOnly, Inject] private PlayerManager _playerManager;
        
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
        }
    }
}