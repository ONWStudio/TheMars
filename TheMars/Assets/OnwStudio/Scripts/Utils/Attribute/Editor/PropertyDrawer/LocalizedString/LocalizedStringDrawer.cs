#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEditor.Localization;
using Onw.Attribute;
using Onw.Editor;
using Onw.Editor.Extensions;

namespace Onw.Attribute.Editor
{
    [CustomPropertyDrawer(typeof(LocalizedStringAttribute))]
    internal sealed class LocalizedStringDrawer : PropertyDrawer
    {
        private static readonly Dictionary<string, StringTableCollection> _tableCache = new();
        private static readonly Dictionary<string, SharedTableData.SharedTableEntry> _entryCache = new();
        private static EntryAdvancedDropdown _entryDropdown = null;
        private static List<Locale> _localesCache;

        static LocalizedStringDrawer()
        {
            // 로케일 변경 이벤트 구독
            LocalizationEditorSettings.EditorEvents.LocaleAdded += onLocaleChanged;
            LocalizationEditorSettings.EditorEvents.LocaleRemoved += onLocaleChanged;
        }

        private static void onLocaleChanged(Locale locale)
        {
            // 로케일 캐시 무효화
            _localesCache = null;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 어트리뷰트 가져오기
            LocalizedStringAttribute attr = (LocalizedStringAttribute)attribute;

            // 필드 타입이 LocalizedString인지 확인
            if (property.propertyType != SerializedPropertyType.Generic || property.type != nameof(LocalizedString))
            {
                EditorGUI.LabelField(position, label.text, "LocalizedStringAttribute는 LocalizedString 타입에만 사용 가능합니다.");
                return;
            }

            // 프로퍼티 시작
            EditorGUI.BeginProperty(position, label, property);

            // 인덴트 설정
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            Debug.Log(property.propertyPath);
            
            LocalizedString localizedString = (LocalizedString)property
                .serializedObject
                .targetObject
                .GetType()
                .GetField(property.propertyPath, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!
                .GetValue(property.serializedObject.targetObject);
            Debug.Log(localizedString);
            // SerializedProperty 가져오기
            SerializedProperty tableReferenceProp = property.FindPropertyRelative("m_TableReference");
            SerializedProperty tableNameProp = tableReferenceProp.FindPropertyRelative("m_TableCollectionName");
            SerializedProperty tableEntryReferenceProp = property.FindPropertyRelative("m_TableEntryReference");
            SerializedProperty tableEntryKeyProp = tableEntryReferenceProp.FindPropertyRelative("m_Key");
            
            // 테이블 이름 및 엔트리 키 설정
            string tableName = attr.TableName;
            if (string.IsNullOrEmpty(tableName))
            {
                tableName = tableNameProp.stringValue;
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = tableNameProp.stringValue = property.serializedObject.targetObject.GetType().Name;
                }
            }

            string entryKey = attr.EntryKey;
            if (string.IsNullOrEmpty(entryKey))
            {
                tableEntryReferenceProp.FindPropertyRelative("m_KeyId").longValue = 0;
                entryKey = tableEntryKeyProp.stringValue;
            }

            // 테이블 컬렉션 가져오기
            StringTableCollection tableCollection = getTableCollection(tableName);
            // 엔트리 가져오기
            SharedTableData.SharedTableEntry sharedTableEntry = null;
            if (tableCollection)
            {
                sharedTableEntry = getSharedTableEntry(tableCollection, entryKey);
            }

            // 현재 위치 저장
            Rect currentPosition = position;

            // 레이블 그리기
            currentPosition.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(currentPosition, label);

            // 다음 줄로 이동
            currentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (!tableCollection)
            {
                EditorGUI.HelpBox(currentPosition, $"테이블 '{tableName}'이(가) 존재하지 않습니다.", MessageType.Error);
                currentPosition.y += EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing;

                // 테이블 생성 버튼 제공
                if (GUI.Button(new(currentPosition.x, currentPosition.y, currentPosition.width, EditorGUIUtility.singleLineHeight), "테이블 생성"))
                {
                    // 테이블 생성 로직
                    string folderPath = EditorUtility.OpenFolderPanel("테이블을 생성할 폴더를 선택하세요.", "Assets", "") + $"/{tableName}/";
                    if (!string.IsNullOrEmpty(folderPath))
                    {
                        folderPath = FileUtil.GetProjectRelativePath(folderPath);
                        if (string.IsNullOrEmpty(folderPath))
                        {
                            Debug.LogError("선택한 폴더가 프로젝트 내에 없습니다.");
                        }
                        else
                        {
                            createStringTable(tableName, folderPath);
                        }
                    }
                }
                currentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else if (sharedTableEntry == null)
            {
                EditorGUI.HelpBox(currentPosition, $"엔트리 '{entryKey}'이(가) 테이블 '{tableName}'에 존재하지 않습니다.", MessageType.Error);
                currentPosition.y += EditorGUIUtility.singleLineHeight * 2f + EditorGUIUtility.standardVerticalSpacing;

            }
            else
            {
                // 로케일 목록 가져오기
                List<Locale> locales = getLocales();

                foreach (Locale locale in locales)
                {
                    // 해당 로케일의 테이블 가져오기
                    StringTable stringTable = tableCollection.GetTable(locale.Identifier) as StringTable;

                    if (stringTable)
                    {
                        // 엔트리 가져오기
                        // 엔트리가 없을 경우 생성
                        StringTableEntry entry = stringTable.GetEntry(sharedTableEntry.Id) ?? stringTable.AddEntry(sharedTableEntry.Id, "");

                        // 번역 값 표시 및 편집
                        currentPosition.height = EditorGUIUtility.singleLineHeight;
                        string newValue = EditorGUI.TextField(currentPosition, locale.LocaleName, entry.Value);

                        // 값이 변경되었을 경우 업데이트
                        if (newValue != entry.Value)
                        {
                            entry.Value = newValue;
                            EditorUtility.SetDirty(stringTable);
                        }

                        // 다음 줄로 이동
                        currentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                    else
                    {
                        // 해당 로케일의 테이블이 없을 경우
                        currentPosition.height = EditorGUIUtility.singleLineHeight;
                        EditorGUI.LabelField(currentPosition, locale.LocaleName, "해당 로케일의 테이블이 존재하지 않습니다.");
                        currentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                }

                // 엔트리 생성 버튼 제공
                if (GUI.Button(new(currentPosition.x, currentPosition.y, currentPosition.width, EditorGUIUtility.singleLineHeight), "엔트리 생성"))
                {
                    if (_entryDropdown is null)
                    {
                        _entryDropdown = new(new(), getTableCollection(tableName), tableCollection.SharedData);
                        _entryDropdown.OnEntrySelected += entry => tableEntryKeyProp.stringValue = entry.Key;
                    }
                    
                    _entryDropdown.Show(new(currentPosition.x, currentPosition.y, 300f, 200f));   
                }
                currentPosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                
                // 변경사항 저장
                if (GUI.changed)
                {
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            // 인덴트 복원
            EditorGUI.indentLevel = indent;

            // 프로퍼티 종료
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0f;

            // 기본 레이블 높이
            totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            // 어트리뷰트 가져오기
            LocalizedStringAttribute attr = (LocalizedStringAttribute)attribute;

            // 테이블 이름 및 엔트리 키 설정
            string tableName = attr.TableName ?? property.FindPropertyRelative("m_TableReference").FindPropertyRelative("m_TableCollectionName").stringValue;
            string entryKey = attr.EntryKey ?? property.FindPropertyRelative("m_TableEntryReference").FindPropertyRelative("m_Key").stringValue;

            // 테이블 컬렉션 가져오기
            StringTableCollection tableCollection = getTableCollection(tableName);

            // 엔트리 가져오기
            SharedTableData.SharedTableEntry sharedTableEntry = null;
            if (tableCollection)
            {
                sharedTableEntry = getSharedTableEntry(tableCollection, entryKey);
            }

            if (!tableCollection || sharedTableEntry == null)
            {
                // 경고 메시지 높이
                totalHeight += (EditorGUIUtility.singleLineHeight * 2f) + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                // 로케일 목록 가져오기
                List<Locale> locales = getLocales();
                totalHeight += locales.Sum(_ => EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
            }
            
            totalHeight += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            
            return totalHeight;
        }

        private static StringTableCollection getTableCollection(string tableName)
        {
            if (string.IsNullOrEmpty(tableName)) return null;

            if (_tableCache.TryGetValue(tableName, out StringTableCollection tableCollection))
            {
                return tableCollection;
            }

            tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
            if (tableCollection)
            {
                _tableCache[tableName] = tableCollection;
            }

            return tableCollection;
        }

        private static SharedTableData.SharedTableEntry getSharedTableEntry(StringTableCollection tableCollection, string entryKey)
        {
            if (!tableCollection || string.IsNullOrEmpty(entryKey)) return null;

            string cacheKey = $"{tableCollection.TableCollectionName}_{entryKey}";
            if (_entryCache.TryGetValue(cacheKey, out SharedTableData.SharedTableEntry sharedTableEntry))
            {
                return sharedTableEntry;
            }

            SharedTableData sharedTableData = tableCollection.SharedData;
            sharedTableEntry = sharedTableData.GetEntry(entryKey);
            if (sharedTableEntry != null)
            {
                _entryCache[cacheKey] = sharedTableEntry;
            }

            return sharedTableEntry;
        }

        private static List<Locale> getLocales()
        {
            _localesCache ??= new(LocalizationEditorSettings.GetLocales());
            return _localesCache;
        }

        private static void createStringTable(string tableName, string folderPath)
        {
            // EditorApplication.delayCall을 사용하여 다음 에디터 사이클로 작업을 미룸
            EditorApplication.delayCall += () =>
            {
                // 테이블 컬렉션 생성
                StringTableCollection collection = LocalizationEditorSettings.CreateStringTableCollection(tableName, folderPath);

                // 테이블 에셋 이동
                // 에셋 데이터베이스 저장
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // 캐시 업데이트
                _tableCache[tableName] = collection;

                Debug.Log($"테이블 '{tableName}'이(가) '{folderPath}'에 생성되었습니다.");
            };
        }
    }
}
#endif