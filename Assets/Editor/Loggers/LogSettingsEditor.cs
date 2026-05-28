using System;
using System.Collections.Generic;
using Systems.Shared.Loggers;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(LogSettings))]
    public sealed class LogSettingsEditor : Editor
    {
        private SerializedProperty _defaultEnabledLevels;
        private SerializedProperty _rules;
        private string _searchText;
        private Dictionary<LogGroup, bool> _groupFoldouts;

        private void OnEnable()
        {
            _defaultEnabledLevels = serializedObject.FindProperty("_defaultEnabledLevels");
            _rules = serializedObject.FindProperty("_rules");
            _groupFoldouts = new Dictionary<LogGroup, bool>();

            foreach (LogGroup group in Enum.GetValues(typeof(LogGroup)))
            {
                _groupFoldouts[group] = true;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_defaultEnabledLevels);

            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Refresh Rules"))
                {
                    foreach (var t in targets)
                    {
                        var settings = (LogSettings)t;
                        Undo.RecordObject(settings, "Refresh Rules");
                        settings.RefreshRules();
                        EditorUtility.SetDirty(settings);
                    }

                    serializedObject.Update();
                }

                if (GUILayout.Button("Enable All"))
                {
                    foreach (var t in targets)
                    {
                        var settings = (LogSettings)t;
                        Undo.RecordObject(settings, "Enable All Rules");
                        settings.EnableAll();
                        EditorUtility.SetDirty(settings);
                    }

                    serializedObject.Update();
                }

                if (GUILayout.Button("Disable All"))
                {
                    foreach (var t in targets)
                    {
                        var settings = (LogSettings)t;
                        Undo.RecordObject(settings, "Disable All Rules");
                        settings.DisableAll();
                        EditorUtility.SetDirty(settings);
                    }

                    serializedObject.Update();
                }
            }

            EditorGUILayout.Space();

            _searchText = EditorGUILayout.TextField("Search", _searchText);
            var hasSearch = !string.IsNullOrWhiteSpace(_searchText);

            EditorGUILayout.Space();

            if (_rules != null)
            {
                foreach (LogGroup group in Enum.GetValues(typeof(LogGroup)))
                {
                    var groupIndices = new List<int>();

                    for (int i = 0; i < _rules.arraySize; i++)
                    {
                        var rule = _rules.GetArrayElementAtIndex(i);
                        var className = rule.FindPropertyRelative("ClassName");
                        var ruleGroup = rule.FindPropertyRelative("Group");

                        if ((LogGroup)ruleGroup.intValue != group)
                        {
                            continue;
                        }

                        if (hasSearch && className.stringValue?.IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) < 0)
                        {
                            continue;
                        }

                        groupIndices.Add(i);
                    }

                    if (groupIndices.Count == 0)
                    {
                        continue;
                    }

                    _groupFoldouts[group] = EditorGUILayout.Foldout(
                        _groupFoldouts[group],
                        $"{group} ({groupIndices.Count})",
                        true
                    );

                    if (!_groupFoldouts[group])
                    {
                        continue;
                    }

                    foreach (var index in groupIndices)
                    {
                        var rule = _rules.GetArrayElementAtIndex(index);
                        var className = rule.FindPropertyRelative("ClassName");
                        var enabled = rule.FindPropertyRelative("Enabled");
                        var levels = rule.FindPropertyRelative("EnabledLevels");
                        var ruleGroup = rule.FindPropertyRelative("Group");

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            enabled.boolValue = EditorGUILayout.Toggle(enabled.boolValue, GUILayout.Width(18));

                            using (new EditorGUI.DisabledScope(true))
                            {
                                EditorGUILayout.TextField(className.stringValue);
                            }

                            EditorGUILayout.PropertyField(ruleGroup, GUIContent.none, GUILayout.Width(90));

                            levels.intValue = (int)(LogLevelFlags)EditorGUILayout.EnumFlagsField(
                                (LogLevelFlags)levels.intValue,
                                GUILayout.Width(140)
                            );
                        }
                    }

                    EditorGUILayout.Space();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
