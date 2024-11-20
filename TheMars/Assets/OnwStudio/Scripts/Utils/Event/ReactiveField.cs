using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Onw.Extensions;
// ReSharper disable InconsistentNaming

namespace Onw.Event
{
    public interface IReadOnlyReactiveField<T>
    {
        T Value { get; }
        
        void AddListener(UnityAction<T> onChangedValue);
        void RemoveListener(UnityAction<T> onChangedValue);
    }

    public interface IReactiveField<T> : IReadOnlyReactiveField<T>
    {
        new T Value { get; set; }
        bool InvokeImmediately { get; }
        bool InvokeIfValueChanged { get; }
    }

    [System.Serializable]
    public class ReactiveField<T> : ReactiveFieldBase<T>
    {
        private static readonly EqualityComparer<T> _defaultEqualityComparer = EqualityComparer<T>.Default;
        protected virtual EqualityComparer<T> EqualityComparer => _defaultEqualityComparer;
        [field: SerializeReference, SerializeReferenceDropdown] public List<IValueProcessor> ValueProcessors { get; set; } = new();

        public override T Value
        {
            get => _value;
            set
            {
                T processedValue = value;
                ValueProcessors?.ForEach(processor => processedValue = processor.Process(processedValue));

                if (!InvokeIfValueChanged || !EqualityComparer.Equals(_value, processedValue))
                {
                    _value = processedValue;
                    _onChangedValue.Invoke(_value);
                }
            }
        }
        
        [field: SerializeField] protected virtual T _value { get; set; }
    }


    [System.Serializable]
    public abstract class ReactiveFieldBase<T> : IReactiveField<T>
    {
        [field: SerializeField] public virtual bool InvokeImmediately { get; set; } = true;
        [field: SerializeField] public virtual bool InvokeIfValueChanged { get; set; } = true;

        public abstract T Value { get; set; }

        [SerializeField] protected UnityEvent<T> _onChangedValue = new();

        public virtual void AddListener(UnityAction<T> onChangedValue)
        {
            if (onChangedValue is null) return;

            _onChangedValue.AddListener(onChangedValue);

            if (InvokeImmediately)
            {
                onChangedValue.Invoke(Value);
            }
        }

        public virtual void RemoveListener(UnityAction<T> onChangedValue)
        {
            if (onChangedValue is null) return;

            _onChangedValue.RemoveListener(onChangedValue);
        }

        public int GetPersistentEventCount() => _onChangedValue.GetPersistentEventCount();
        public Object GetPersistentTarget(int index) => _onChangedValue.GetPersistentTarget(index);
        public void SetPersistentListenerState(UnityEventCallState callState) => _onChangedValue.SetPersistentListenerState(callState);
        public void SetPersistentListenerState(int index, UnityEventCallState callState) => _onChangedValue.SetPersistentListenerState(index, callState);
        public void RemoveAllListener() => _onChangedValue.RemoveAllListeners();
        public string GetPersistentMethodName(int index) => _onChangedValue.GetPersistentMethodName(index);
        public override string ToString() => Value.ToString();
    }
}