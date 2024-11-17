using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using TMPro;
using Onw.Attribute;
using TM.Manager;

namespace TM.UI
{
    public sealed class TMPlayerViewer : MonoBehaviour
    {
        [Header("Player Info")]
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _levelText;
        [SerializeField, SelectableSerializeField] private Image _expProgressImage;
        
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
        [SerializeField, SelectableSerializeField] private Button _playButton;
        [SerializeField, SelectableSerializeField] private Button _pauseButton;
        [SerializeField, SelectableSerializeField] private Button _fastButton;
        
        [Header("Terraform Info")]
        [SerializeField, SelectableSerializeField] private Image _terraformHexagonImage;
        [SerializeField, SelectableSerializeField] private TextMeshProUGUI _terraformValueText;

        private void Start()
        {
            TMPlayerManager.Instance.Level.AddListener(OnChangedLevel);
            TMPlayerManager.Instance.MarsLithium.AddListener(OnChangedMarsLithium);
            TMPlayerManager.Instance.Credit.AddListener(OnChangedCredit);
            TMPlayerManager.Instance.Population.AddListener(OnChangedPopulation);
            TMPlayerManager.Instance.TotalPopulation.AddListener(OnChangedTotalPopulation);
            TMPlayerManager.Instance.Steel.AddListener(OnChangedSteel);
            TMPlayerManager.Instance.Plants.AddListener(OnChangedPlants);
            TMPlayerManager.Instance.Clay.AddListener(OnChangedClay);
            TMPlayerManager.Instance.Electricity.AddListener(OnChangedElectricity);
            TMPlayerManager.Instance.Satisfaction.AddListener(OnChangedSatisfaction);
            TMPlayerManager.Instance.TerraformValue.AddListener(OnChangedTerraformValue);
            TMPlayerManager.Instance.Exp.AddListener(OnChangedExp);
            TMPlayerManager.Instance.MaxExp.AddListener(OnChangedMaxExp);
            TMSimulator.Instance.NowSeconds.AddListener(OnChangedSeconds);
            TMSimulator.Instance.NowDay.AddListener(OnChangedDay);
            _playButton.onClick.AddListener(OnClickAgainButton);
            _pauseButton.onClick.AddListener(OnClickStopButton);
            _fastButton.onClick.AddListener(OnClickFastButton);
        }

        public void OnChangedMaxExp(int maxExp)
        {
            _expProgressImage.fillAmount = TMPlayerManager.Instance.Exp.Value / (float)maxExp;
        }

        public void OnChangedExp(int exp)
        {
            _expProgressImage.fillAmount = exp / (float)TMPlayerManager.Instance.MaxExp.Value;
        }

        public void OnClickAgainButton()
        {
            TimeManager.IsPause = false;
        }

        public void OnClickStopButton()
        {
            TimeManager.IsPause = true;
        }

        public void OnClickFastButton()
        {
            TimeManager.GameSpeed = TimeManager.GameSpeed switch
            {
                1 => 2,
                2 => 3,
                _ => 1
            };
        }
        
        public void OnChangedLevel(int level)
        {
            _levelText.text = $"Lv. {level}";
        }

        public void OnChangedMarsLithium(int marsLithium)
        {
            _marsLithiumText.text = marsLithium.ToString();
        }

        public void OnChangedCredit(int credit)
        {
            _creditText.text = credit.ToString();
        }

        public void OnChangedPopulation(int population)
        {
            _populationText.text = $"{population} / {TMPlayerManager.Instance.TotalPopulation.Value}";
        }

        public void OnChangedTotalPopulation(int totalPopulation)
        {
            _populationText.text = $"{TMPlayerManager.Instance.Population.Value} / {totalPopulation}";
        }

        public void OnChangedSteel(int steel)
        {
            _steelText.text = steel.ToString();
        }

        public void OnChangedPlants(int plants)
        {
            _plantsText.text = plants.ToString();
        }

        public void OnChangedClay(int clay)
        {
            _clayText.text = clay.ToString();
        }

        public void OnChangedElectricity(int electricity)
        {
            _electricityText.text = electricity.ToString();
        }

        public void OnChangedSatisfaction(int satisfaction)
        {
            _satisfactionProgressImage.fillAmount = satisfaction / (float)TMPlayerManager.MAX_SATISFACTION;
        }

        public void OnChangedSeconds(int seconds)
        {
            _timeProgressImage.fillAmount = seconds % TMSimulator.Instance.IntervalInSeconds / (float)TMSimulator.Instance.IntervalInSeconds;
        }

        public void OnChangedDay(int day)
        {
            _dayText.text = $"Day\n{day}";
        }

        public void OnChangedTerraformValue(int terraformValue)
        {
            float normalizedTerraformValue = terraformValue / (float)TMPlayerManager.MAX_TERRAFORM_VALUE;
            
            _terraformHexagonImage.fillAmount = normalizedTerraformValue;
            _terraformValueText.text = $"{((int)(normalizedTerraformValue * 100f))}%";
        }
    }
}