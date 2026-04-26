using System.IO;
using UnityEditor;
using UnityEngine;

public static class CustomScriptCreator {
    private const string MenuPath = "Assets/Create/Scripts";

    [MenuItem(MenuPath + "/C# Monobehaviour ", false, 70)]
    public static void CreateScriptWithNamespace() {
        StartNameEditing("DefaultScriptTemplate");
    }

    [MenuItem(MenuPath + "/C# Plain ", false, 71)]
    public static void CreatePlainCSharpScript() {
        StartNameEditing("PlainScriptTemplate");
    }

    [MenuItem(MenuPath + "/C# Interface ", false, 72)]
    public static void CreateInterface() {
        StartNameEditing("DefaultInterfaceTemplate");
    }

    private static void StartNameEditing(string templateName) {
        string folder = PathHelpers.GetSelectedPathOrFallback();
        string templatePath = PathHelpers.Templates.GetPathFor(templateName);

        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0, // instance ID (0 for new)
            ScriptableObject.CreateInstance<DoCreateScript>(),
            Path.Combine(folder, "NewScript.cs"),
            null,
            templatePath
        );
    }

    private class DoCreateScript : UnityEditor.ProjectWindowCallback.EndNameEditAction {
        public override void Action(int instanceId, string pathName, string resourceFile) {
            CreateFileFromTemplate(pathName, resourceFile);
            AssetDatabase.Refresh();
        }
    }

    private static void CreateFileFromTemplate(string filePath, string templatePath) {
        string folder = Path.GetDirectoryName(filePath);
        string scriptName = Path.GetFileNameWithoutExtension(filePath);
        string namespaceName = PathHelpers.GetNamespaceFromPath(folder);

        var updatedTemplate = File.ReadAllText(templatePath)
            .Replace("#SCRIPTNAME#", scriptName.ToCapitalized())
            .Replace("#NAMESPACE#", namespaceName);

        File.WriteAllText(filePath, updatedTemplate);
    }
}

public static class StringHelpers {
    public static string ToCapitalized(this string input) => Capitalize(input);

    public static string Capitalize(string input) {
        return !string.IsNullOrEmpty(input) ? $"{char.ToUpper(input[0])}{input[1..]}" : input;
    }
}
