#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Michsky.UI.Heat;

namespace Onw.Manager.Editor
{
    [CustomEditor(typeof(LocalizationSingleton<>), true)]
    internal sealed class LocalizationSingletonDrawer : LocalizationTableEditor {}
}
#endif