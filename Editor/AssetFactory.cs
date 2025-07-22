using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class AssetFactory : MonoBehaviour
{
    public static void CreateAsset<T>(string defaultName = "NewAsset") where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string path = GetActiveProjectWindowPath();
        string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, $"{defaultName}.asset"));

        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

    }

    private static string GetActiveProjectWindowPath()
    {
        // If something is selected, default to selection
        string path;

        // Otherwise, use reflection to get the current ProjectBrowser folder
        var projectBrowserType = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
        var lastProjectBrowser = projectBrowserType?.GetField("s_LastInteractedProjectBrowser",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(null);

        if (lastProjectBrowser != null)
        {
            var method = projectBrowserType.GetMethod("GetActiveFolderPath",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method != null)
            {
                path = (string)method.Invoke(lastProjectBrowser, null);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }
        }

        return "Assets"; // fallback if all else fails
    }


}
