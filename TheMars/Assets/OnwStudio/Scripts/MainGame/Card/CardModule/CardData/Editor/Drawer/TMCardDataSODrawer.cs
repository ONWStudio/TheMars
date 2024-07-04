#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Onw.Editor;

namespace TMCard.Editor
{
    //using Editor = UnityEditor.Editor;

    //[CustomEditor(typeof(TMCardData), false), CanEditMultipleObjects]
    //public sealed class TMCardDataSODrawer : OnwEditor
    //{
    //    private RenderTexture _renderTexture = null;
    //    private GameObject previewInstance = null;
    //    private Editor previewEditor = null;

    //    protected override void OnEnable()
    //    {
    //        Debug.Log("TMCardDataSODrawer Enable");
    //        base.OnEnable();

    //        if (target is GameObject targetObject)
    //        {
    //            previewInstance = Instantiate(targetObject);
    //            previewEditor = CreateEditor(previewInstance);
    //        }
    //    }

    //    public override void OnInspectorGUI()
    //    {
    //        Debug.Log("TMCardDataSODrawer");
    //        base.OnInspectorGUI();
    //    }
    //}
}
#endif