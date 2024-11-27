using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Onw.Attribute;
using Onw.Extensions;
using Onw.ObjectPool;
using Onw.Prototype;
using TM.Buff;

namespace TM.UI
{
    public class TMBuffUIController : MonoBehaviour
    {
        public event UnityAction<TMBuffIcon> OnAddedBuffIcon
        {
            add => _onAddedBuffIcon.AddListener(value);
            remove => _onAddedBuffIcon.RemoveListener(value);
        }
        
        public event UnityAction<TMBuffIcon> OnRemovedBuffIcon
        {
            add => _onRemovedBuffIcon.AddListener(value);
            remove => _onRemovedBuffIcon.RemoveListener(value);
        }
        
        public IReadOnlyDictionary<string, TMBuffIcon> BuffIcons => _buffDictionary;
        
        [SerializeField, SelectableSerializeField] private ScrollRect _buffScrollView;
        [SerializeField, SelectableSerializeField] private TMBuffDescriptor _buffDescriptor;

        [SerializeField] private UnityEvent<TMBuffIcon> _onAddedBuffIcon = new();
        [SerializeField] private UnityEvent<TMBuffIcon> _onRemovedBuffIcon = new();

        private readonly Dictionary<string, TMBuffIcon> _buffDictionary = new();

        private string _currentBuffId = string.Empty;

        public void OnAddedBuff(TMBuffBase buff)
        {
            if (!GenericObjectPool<TMBuffIcon>.TryPop(out TMBuffIcon icon))
            {
                icon = PrototypeManager.Instance.ClonePrototypeSync<TMBuffIcon>("Buff_Icon");
            }

            icon.SetUI(buff);
            icon.SetParent(_buffScrollView.content.transform, false);
            _buffDictionary.Add(buff.ID, icon);

            icon.OnPointerEnterEvent += onSelectIcon;
            icon.OnPointerExitEvent += onDeselectIcon;
            
            _onAddedBuffIcon.Invoke(icon);
            
            void onSelectIcon()
            {
                _currentBuffId = buff.ID;
                buff.Description.StringChanged += _buffDescriptor.SetDescriptionText;
                _buffDescriptor.SetActiveDescriptor(true);

                switch (buff)
                {
                    case TMRepeatBuff repeatBuff:
                        repeatBuff.RepeatTimeDescription.StringChanged += _buffDescriptor.SetTimeText;
                        break;
                    case TMDelayBuff delayBuff:
                        delayBuff.DelayTimeDescription.StringChanged += _buffDescriptor.SetTimeText;
                        break;
                }
            }

            void onDeselectIcon()
            {
                _currentBuffId = buff.ID;
                buff.Description.StringChanged -= _buffDescriptor.SetDescriptionText;
                _buffDescriptor.SetActiveDescriptor(false);
                _buffDescriptor.SetDescriptionText(string.Empty);
                _buffDescriptor.SetTimeText(string.Empty);
                
                switch (buff)
                {
                    case TMRepeatBuff repeatBuff:
                        repeatBuff.RepeatTimeDescription.StringChanged -= _buffDescriptor.SetTimeText;
                        break;
                    case TMDelayBuff delayBuff:
                        delayBuff.DelayTimeDescription.StringChanged -= _buffDescriptor.SetTimeText;
                        break;
                }
            }
        }

        public void OnRemovedBuff(TMBuffBase buff)
        {
            if (!_buffDictionary.Remove(buff.ID, out TMBuffIcon icon)) return;

            buff.Description.StringChanged -= _buffDescriptor.SetDescriptionText;

            if (_currentBuffId == buff.ID)
            {
                _buffDescriptor.SetActiveDescriptor(false);
                _buffDescriptor.SetDescriptionText(string.Empty);
                _buffDescriptor.SetTimeText(string.Empty);
                _currentBuffId = string.Empty;
            }
            
            switch (buff)
            {
                case TMRepeatBuff repeatBuff:
                    repeatBuff.RepeatTimeDescription.StringChanged -= _buffDescriptor.SetTimeText;
                    break;
                case TMDelayBuff delayBuff:
                    delayBuff.DelayTimeDescription.StringChanged -= _buffDescriptor.SetTimeText;
                    break;
            }

            _onRemovedBuffIcon.Invoke(icon);
            GenericObjectPool<TMBuffIcon>.Return(icon);
        }
    }
}