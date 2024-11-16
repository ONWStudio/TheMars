using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Onw.Event;
using Onw.Attribute;
using TM.Building.Effect;
using UnityEngine.Events;

namespace TM.Building
{
    public sealed class TMBuilding : MonoBehaviour
    {
        public event UnityAction<TMBuilding> OnBatchTile
        {
            add => _onBatchTile.AddListener(value);
            remove => _onBatchTile.RemoveListener(value);
        }

        [SerializeReference, SerializeReferenceDropdown, ReadOnly] private List<ITMBuildingEffect> _effects = new();
        
        [SerializeField] private ReactiveField<bool> _isActive = new() { Value = true };
        [SerializeField] private UnityEvent<TMBuilding> _onBatchTile = new();

        [Header("Components")]
        [SerializeField] private List<Renderer> _meshRenderers = new();
        [SerializeField] private bool _isRender = true;

        [FormerlySerializedAs("_buildingLevel")]
        [Header("Building State")]
        [SerializeField] private ReactiveField<int> _gradePlus = new() { Value = 0 };
        [SerializeField] private ReactiveField<int> _grade  = new() { Value = 1, ValueProcessors = new() { new ClampIntProcessor(1, 3) }};
        [SerializeField] private ReactiveField<bool> _onTile = new();
        
        [field: Header("Building Data")]
        [field: SerializeField, ReadOnly] public TMBuildingData BuildingData { get; private set; }

        public IReadOnlyList<ITMBuildingEffect> Effects => _effects;

        public string LocalizedEffectDescription => string.Join(
            "\n", 
            _effects
                .Where(effect => !effect.LocalizedDescription.IsEmpty)
                .Select(effect => effect.LocalizedDescription.GetLocalizedString()));

        public bool IsRender
        {
            get => _isRender;
            set
            {
                _isRender = value;
                _meshRenderers.ForEach(renderer => renderer.enabled = _isRender);
            }
        }

        public int LastGrade => Mathf.Clamp(_grade.Value + _gradePlus.Value, 1, 3);

        public IReactiveField<int> GradePlus => _gradePlus;
        public IReactiveField<int> Grade => _grade;
        public IReactiveField<bool> OnTile => _onTile;
        public IReactiveField<bool> IsActive => _isActive;

        public TMBuilding Initialize(TMBuildingData buildingData)
        {
            if (BuildingData) return this;

            BuildingData = buildingData;
            _effects = BuildingData
                .CreateBuildingEffects()
                .ToList();

            _meshRenderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());

            return this;
        }

        public void BatchOnTile()
        {
            ApplyBuildingEffect();
            _onBatchTile.Invoke(this);
            IsRender = true;
        }

        public void ApplyBuildingEffect()
        {
            _effects
                .ForEach(buildingEffect => buildingEffect.ApplyEffect(this));
        }
        
        public void DisableBuilding()
        {
            _effects
                .ForEach(effect => effect.DisableEffect(this));
        }
    }
}