using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TheraBytes.BetterUi.Editor
{
    [CustomPropertyDrawer(typeof(Vector2SizeModifier))]
    public class Vector2SizeModifierDrawer : ScreenDependentSizeDrawer<Vector2>
    {
        protected override void DrawModifiers(SerializedProperty property)
        {
            SerializedProperty modx = property.FindPropertyRelative("ModX");
            DrawModifierList(modx, "X Modification");

            SerializedProperty mody = property.FindPropertyRelative("ModY");
            DrawModifierList(mody, "Y Modification");
        }

        protected override string GetValueString(Vector2 obj)
        {
            return string.Format("({0}, {1})", obj.x, obj.y);
        }
    }
}
