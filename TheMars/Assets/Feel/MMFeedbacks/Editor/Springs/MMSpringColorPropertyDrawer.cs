using MoreMountains.Tools;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace MoreMountains.Feedbacks
{
	[CustomPropertyDrawer(typeof(MMSpringColor))]
	internal class MMSpringColorPropertyDrawer : PropertyDrawer
	{
		protected float _lastTarget;
		protected float _max;
		
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			VisualElement root = new VisualElement();
			
			SerializedProperty _colorSpring = property.FindPropertyRelative("ColorSpring");
			root.Add(new PropertyField(_colorSpring));
			
			return root;
		}
		
	}
}


