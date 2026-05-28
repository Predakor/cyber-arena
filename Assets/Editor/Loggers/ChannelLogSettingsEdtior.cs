using Systems.Shared.Channels;
using UnityEditor;
using UnityEngine;

namespace Editors.Loggers
{
    [CustomEditor(typeof(EventChannelBase<>), true)]
    public sealed class ChannelLogSettingsEditor : Editor
    {
        private SerializedProperty _rules;
        private string _searchText;

        private void OnEnable()
        {
            _rules = serializedObject.FindProperty("_rules");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawControlButtons();

            EditorGUILayout.Space();

            bool hasSearch = DrawSearchBar();

            EditorGUILayout.Space();

            DrawRules(hasSearch);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawRules(bool hasSearch)
        {
            if (_rules == null || _rules.arraySize == 0)
            {
                EditorGUILayout.HelpBox("No log rules found. Click 'Refresh Rules' to populate.", MessageType.Info);
                return;
            }

            for (int i = 0; i < _rules.arraySize; i++)
            {
                var rule = _rules.GetArrayElementAtIndex(i);
                if (!hasSearch)
                {
                    DrawRuleRow(rule);
                    continue;
                }

                var className = rule.FindPropertyRelative("EventName");
                if (className.stringValue?.Contains(_searchText, System.StringComparison.OrdinalIgnoreCase) == true)
                {
                    DrawRuleRow(rule);
                }

            }
        }

        private bool DrawSearchBar()
        {
            _searchText = EditorGUILayout.TextField("Search", _searchText);
            var hasSearch = !string.IsNullOrWhiteSpace(_searchText);
            return hasSearch;
        }

        private void DrawControlButtons()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Refresh Rules"))
                {
                    foreach (var t in targets)
                    {
                        var settings = (IEventChannelLogRules)t;
                        Undo.RecordObject((UnityEngine.Object)settings, "Refresh Rules");
                        settings.RefreshLogRules();
                        EditorUtility.SetDirty((UnityEngine.Object)settings);
                    }

                    serializedObject.Update();
                }

                if (GUILayout.Button("Enable All"))
                {
                    foreach (var t in targets)
                    {
                        var settings = (IEventChannelLogRules)t;
                        Undo.RecordObject((UnityEngine.Object)settings, "Enable All Rules");
                        settings.SetAllRules(true);
                        EditorUtility.SetDirty((UnityEngine.Object)settings);
                    }

                    serializedObject.Update();
                }

                if (GUILayout.Button("Disable All"))
                {
                    foreach (var t in targets)
                    {
                        var settings = (IEventChannelLogRules)t;
                        Undo.RecordObject((UnityEngine.Object)settings, "Disable All Rules");
                        settings.SetAllRules(false);
                        EditorUtility.SetDirty((UnityEngine.Object)settings);
                    }

                    serializedObject.Update();
                }
            }
        }

        private void DrawRuleRow(SerializedProperty rule)
        {
            var name = rule.FindPropertyRelative("EventName");
            var enabled = rule.FindPropertyRelative("Enabled");

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(enabled, GUIContent.none, GUILayout.Width(20));
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.TextField(name.stringValue);
                }
            }
        }
    }

}