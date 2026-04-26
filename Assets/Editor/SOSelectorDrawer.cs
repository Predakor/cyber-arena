using System;
using System.Collections.Generic;
using System.Linq;
using Systems.Guns;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DerivedSoSelectorAttribute))]
public sealed class SOSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SOSelectorGUI.DrawPicker(position, property, label, ((DerivedSoSelectorAttribute)attribute).BaseType);
    }
}

[CustomPropertyDrawer(typeof(TypedDerivedSOSelectorAttribute))]
public sealed class TypedSOSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SOSelectorGUI.DrawPicker(position, property, label, fieldInfo.FieldType);
    }
}

internal static class SOSelectorGUI
{
    internal static void DrawPicker(Rect position, SerializedProperty property, GUIContent label, Type baseType)
    {
        EditorGUI.BeginProperty(position, label, property);

        var fieldRect = EditorGUI.PrefixLabel(position, label);
        var currentName = property.objectReferenceValue != null
            ? property.objectReferenceValue.name
            : "None";

        if (GUI.Button(fieldRect, currentName, EditorStyles.objectField))
        {
            var assets = GetDerivedAssets(baseType);
            SOPickerWindow.Show(fieldRect, assets, selected =>
            {
                property.objectReferenceValue = selected;
                property.serializedObject.ApplyModifiedProperties();
            });
        }

        EditorGUI.EndProperty();
    }

    private static ScriptableObject[] GetDerivedAssets(Type baseType)
    {
        var types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract)
            .ToList();

        if (!baseType.IsAbstract)
        {
            types.Insert(0, baseType);
        }

        var result = new List<ScriptableObject>();
        foreach (var type in types)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:" + type.Name))
            {
                var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(
                    AssetDatabase.GUIDToAssetPath(guid));
                if (asset != null)
                {
                    result.Add(asset);
                }
            }
        }
        return result.ToArray();
    }
}

public sealed class SOPickerWindow : PopupWindowContent
{
    private ScriptableObject[] _assets;
    private System.Action<ScriptableObject> _onSelected;
    private string _search = "";
    private Vector2 _scroll;

    public static void Show(Rect rect, ScriptableObject[] assets, System.Action<ScriptableObject> onSelected)
    {
        PopupWindow.Show(rect, new SOPickerWindow { _assets = assets, _onSelected = onSelected });
    }

    public override Vector2 GetWindowSize() => new Vector2(250, 300);

    public override void OnGUI(Rect rect)
    {
        _search = EditorGUILayout.TextField("Search", _search);
        EditorGUILayout.Space(2);
        _scroll = EditorGUILayout.BeginScrollView(_scroll);

        if (GUILayout.Button("None", EditorStyles.label))
        {
            _onSelected(null);
            editorWindow.Close();
        }

        foreach (var asset in _assets.Where(a =>
            string.IsNullOrEmpty(_search) || a.name.ToLower().Contains(_search.ToLower())))
        {
            if (GUILayout.Button(asset.name, EditorStyles.label))
            {
                _onSelected(asset);
                editorWindow.Close();
            }
        }

        EditorGUILayout.EndScrollView();
    }
}