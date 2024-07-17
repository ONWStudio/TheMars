#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.UI.Heat
{
    public class HeatUIEditorHandler : Editor
    {
        public static GUISkin GetDarkEditor(GUISkin tempSkin)
        {
            tempSkin = (GUISkin)Resources.Load("HeatUIEditor-Dark");
            return tempSkin;
        }

        public static GUISkin GetLightEditor(GUISkin tempSkin)
        {
            tempSkin = (GUISkin)Resources.Load("HeatUIEditor-Light");
            return tempSkin;
        }

        public static void DrawProperty(SerializedProperty property, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static void DrawProperty(SerializedProperty property, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyPlain(SerializedProperty property, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyPlain(SerializedProperty property, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), skin.FindStyle("Text"), GUILayout.Width(120));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyCW(SerializedProperty property, GUISkin skin, string content, float width)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyCW(SerializedProperty property, GUISkin skin, string content, string tooltip, float width)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            EditorGUILayout.LabelField(new GUIContent(content, tooltip), skin.FindStyle("Text"), GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, new GUIContent("", tooltip));

            GUILayout.EndHorizontal();
        }

        public static void DrawPropertyPlainCW(SerializedProperty property, GUISkin skin, string content, float width)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent(content), skin.FindStyle("Text"), GUILayout.Width(width));
            EditorGUILayout.PropertyField(property, new GUIContent(""));

            GUILayout.EndHorizontal();
        }

        public static int DrawTabs(int tabIndex, GUIContent[] tabs, GUISkin skin)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            tabIndex = GUILayout.Toolbar(tabIndex, tabs, skin.FindStyle("Tab Indicator"));

            GUILayout.EndHorizontal();
            GUILayout.Space(-40);
            GUILayout.BeginHorizontal();
            GUILayout.Space(17);

            return tabIndex;
        }

        public static void DrawComponentHeader(GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(new GUIContent(""), skin.FindStyle(content));
            GUILayout.EndHorizontal();
            GUILayout.Space(-42);
        }

        public static void DrawHeader(GUISkin skin, string content, int space)
        {
            GUILayout.Space(space);
            GUILayout.Box(new GUIContent(""), skin.FindStyle(content));
        }

        public static bool DrawToggle(bool value, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            value = GUILayout.Toggle(value, new GUIContent(content, "Current state: " + value.ToString()), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", "Current state: " + value.ToString()), skin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static bool DrawToggle(bool value, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);

            value = GUILayout.Toggle(value, new GUIContent(content, tooltip), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", tooltip), skin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static bool DrawTogglePlain(bool value, GUISkin skin, string content)
        {
            GUILayout.BeginHorizontal();

            value = GUILayout.Toggle(value, new GUIContent(content, "Current state: " + value.ToString()), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", "Current state: " + value.ToString()), skin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static bool DrawTogglePlain(bool value, GUISkin skin, string content, string tooltip)
        {
            GUILayout.BeginHorizontal();

            value = GUILayout.Toggle(value, new GUIContent(content, tooltip), skin.FindStyle("Toggle"));
            value = GUILayout.Toggle(value, new GUIContent("", tooltip), skin.FindStyle("Toggle Helper"));

            GUILayout.EndHorizontal();
            return value;
        }

        public static void DrawUIManagerConnectedHeader()
        {
            EditorGUILayout.HelpBox("This object is connected with the UI Manager. Some parameters (such as colors, " +
                               "fonts or booleans) are managed by the manager.", MessageType.Info);
        }

        public static void DrawUIManagerPresetHeader()
        {
            EditorGUILayout.HelpBox("This object is subject to a preset and cannot be used with the UI Manager. " +
                                         "You can use the standard object for UI Manager connection.", MessageType.Info);
        }

        public static void DrawUIManagerDisconnectedHeader()
        {
            EditorGUILayout.HelpBox("This object does not have any connection with the UI Manager.", MessageType.Info);
        }

        public static Texture2D TextureFromSprite(Sprite sprite)
        {
            if (sprite == null) { return null; }

            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new((int)sprite.rect.width, (int)sprite.rect.height);
                Texture2D spriteTexture = sprite.texture;
                Rect textureRect = sprite.textureRect;

                Color32[] fullTexturePixels = spriteTexture.GetPixels32();
                Color32[] newColors = new Color32[(int)textureRect.width * (int)textureRect.height];

                for (int y = 0; y < textureRect.height; y++)
                {
                    for (int x = 0; x < textureRect.width; x++)
                    {
                        int newIndex = x + y * (int)textureRect.width;
                        int oldIndex = x + (int)textureRect.x + (y + (int)textureRect.y) * spriteTexture.width;
                        newColors[newIndex] = fullTexturePixels[oldIndex];
                    }
                }

                newText.SetPixels32(newColors);
                newText.Apply();
                return newText;
            }

            else { return sprite.texture; }
        }
    }
}
#endif