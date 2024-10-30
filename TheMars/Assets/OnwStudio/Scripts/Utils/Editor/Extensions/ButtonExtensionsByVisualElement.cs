#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using Onw.Extensions;
using System.Linq;

namespace Onw.Editor.Extensions
{
    public static class ButtonExtensionsByVisualElement
    {
        public static void SetChangedColorButtonEvent(this Button button)
        {
            Color defaultColor = button.style.backgroundColor.value;
            float hoverSaturationOffset = -0.2f;  // hover 시 채도 감소 정도
            float clickSaturationOffset = 0.5f;  // 클릭 시 채도 증가 정도

            // 마우스 hover 시 채도 밝게
            button.RegisterCallback<MouseEnterEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustSaturation(hoverSaturationOffset));

            // 마우스가 떠날 때 원래 색상으로 복원
            button.RegisterCallback<MouseLeaveEvent>(evt =>
                button.style.backgroundColor = defaultColor);

            // 클릭 시 채도 어둡게
            button.RegisterCallback<MouseDownEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustSaturation(clickSaturationOffset), 
                TrickleDown.TrickleDown);

            // 클릭 해제 시 hover 상태를 유지하거나 원래 색상으로 복원
            button.RegisterCallback<MouseUpEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustSaturation(hoverSaturationOffset), 
                TrickleDown.TrickleDown);
        }
    }
}
#endif
