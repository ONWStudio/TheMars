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
        
        /// <summary>
        /// .. backgroundColor변경시 마우스 이벤트로 인한 버튼 View 상호작용이 일어나지 않는 현상을 해결합니다
        /// </summary>
        /// <param name="button"></param>
        public static void SetChangedColorButtonEvent(this Button button)
        {
            Color defaultColor = button.style.backgroundColor.value;
            const float HOVER_OFFSET = 0.1f;  
            const float CLICK_OFFSET = -0.05f; 

            button.RegisterCallback<MouseEnterEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustValue(HOVER_OFFSET));

            button.RegisterCallback<MouseLeaveEvent>(evt =>
                button.style.backgroundColor = defaultColor);

            button.RegisterCallback<MouseDownEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustValue(CLICK_OFFSET), 
                TrickleDown.TrickleDown);

            button.RegisterCallback<MouseUpEvent>(evt =>
                button.style.backgroundColor = defaultColor, 
                TrickleDown.TrickleDown);
        }
    }
}
#endif
