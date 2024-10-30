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
            float hoverSaturationOffset = -0.2f;  // hover �� ä�� ���� ����
            float clickSaturationOffset = 0.5f;  // Ŭ�� �� ä�� ���� ����

            // ���콺 hover �� ä�� ���
            button.RegisterCallback<MouseEnterEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustSaturation(hoverSaturationOffset));

            // ���콺�� ���� �� ���� �������� ����
            button.RegisterCallback<MouseLeaveEvent>(evt =>
                button.style.backgroundColor = defaultColor);

            // Ŭ�� �� ä�� ��Ӱ�
            button.RegisterCallback<MouseDownEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustSaturation(clickSaturationOffset), 
                TrickleDown.TrickleDown);

            // Ŭ�� ���� �� hover ���¸� �����ϰų� ���� �������� ����
            button.RegisterCallback<MouseUpEvent>(evt =>
                button.style.backgroundColor = defaultColor.AdjustSaturation(hoverSaturationOffset), 
                TrickleDown.TrickleDown);
        }
    }
}
#endif
