using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace SerializeReferenceDropdown.Editor
{
    public class AdvancedDropdown : UnityEditor.IMGUI.Controls.AdvancedDropdown
    {
        private readonly IEnumerable<string> typeNames;

        private readonly Dictionary<AdvancedDropdownItem, int> itemAndIndexes =
            new Dictionary<AdvancedDropdownItem, int>();

        private readonly Action<int> onSelectedTypeIndex;

        public AdvancedDropdown(AdvancedDropdownState state, IEnumerable<string> typeNames,
            Action<int> onSelectedNewType) :
            base(state)
        {
            this.typeNames = typeNames;
            onSelectedTypeIndex = onSelectedNewType;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem("Types");
            itemAndIndexes.Clear();

            int index = 0;
            foreach (string typeName in typeNames)
            {
                AdvancedDropdownItem item = new AdvancedDropdownItem(typeName);
                itemAndIndexes.Add(item, index);
                root.AddChild(item);
                index++;
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            if (itemAndIndexes.TryGetValue(item, out int index))
            {
                onSelectedTypeIndex.Invoke(index);
            }
        }
    }
}