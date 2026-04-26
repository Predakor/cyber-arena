using System.IO;
using System.Linq;
using UnityEditor;

internal static class PathHelpers {
    static readonly char[] pathBreaks = new[] { '/', '\\' };

    public static string GetSelectedPathOrFallback() {
        foreach (var obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets)) {
            string path = AssetDatabase.GetAssetPath(obj);
            if (Directory.Exists(path)) {
                return path;
            }
        }
        return "Assets";
    }

    public static string GetNamespaceFromPath(string path) {
        var parts = path
            .Replace("Assets", "")
            .Split(pathBreaks, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(StringHelpers.Capitalize);

        return string.Join(".", parts);
    }

    public static class Templates {
        private const string TemplatesPath = "Assets/Editor/ScriptTemplates";

        public static string GetPathFor(string templateName) {
            return $"{TemplatesPath}/{templateName}.cs.txt";
        }
    }
}
