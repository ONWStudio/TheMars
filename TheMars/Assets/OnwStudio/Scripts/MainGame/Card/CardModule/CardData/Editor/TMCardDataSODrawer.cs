#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace TMCard.Editor
{
    using Editor = UnityEditor.Editor;
    [CustomEditor(typeof(TMCardData), false, isFallback = false), CanEditMultipleObjects]
    public sealed class TMCardDataSODrawer : Editor
    {
        private RenderTexture _renderTexture = null;
        private GameObject previewInstance = null;

        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawDefaultInspector();
            Debug.Log("TMCardDataSODrawer");
        }
    }
}
#endif