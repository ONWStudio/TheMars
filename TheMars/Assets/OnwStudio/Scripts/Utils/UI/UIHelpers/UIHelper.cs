using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Onw.UI
{
    public static class UIHelper
    {
        public static RectTransform GetNewUIObject(Transform parent, string name = "")
        {
            GameObject newUIObject = new(name);
            RectTransform rectTransform = newUIObject.AddComponent<RectTransform>();
            newUIObject.transform.SetParent(parent, false);

            return rectTransform;
        }

    }
}
