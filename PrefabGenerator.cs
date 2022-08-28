using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabGenerator : EditorWindow
{
    private string pathName = "";

    [MenuItem("Tools/Comp-3/Mass Prefab Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(PrefabGenerator));
    }

    private void OnGUI()
    {
        GUILayout.Label("Mass prefab generator", EditorStyles.boldLabel);
        GUILayout.Label("How to use:\nInput the desired output folder name in the Path Name input box, or select your desired folder in the project window and hit Update Location Folder to perform this automatically\nSelect all objects you want to prefab in the scene view and hit Build Prefabs.\nDepending on the amount of objects you're converting this may take a few seconds", EditorStyles.helpBox);
        GUILayout.Space(16);
        pathName = EditorGUILayout.TextField("Save path", pathName);

        GUILayout.Space(16);
        if(GUILayout.Button("Build Prefabs"))
            GeneratePrefabs();

        if (GUILayout.Button("Update location folder"))
            UpdateLocationFolder();
    }

    private void GeneratePrefabs()
    {
        if(pathName == null || pathName.Length < 1)
        {
            Debug.LogWarning("No path name selected");
            return;
        }

        if (!Directory.Exists($"Assets/{pathName}"))
            Directory.CreateDirectory($"Assets/{pathName}");

        foreach(GameObject go in Selection.gameObjects)
        {
            string localPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/{pathName}/{go.name}.prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(go, localPath, InteractionMode.AutomatedAction);
        }
    }

    private void UpdateLocationFolder()
    {
        string selectedPath;
        var obj = Selection.activeObject;
        if (obj == null)
        {
            Debug.LogWarning("No folder selected");
            return;
        }
        else
        {
            selectedPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
        }

        if (selectedPath.Length > 0)
        {
            if (Directory.Exists(selectedPath))
            {
                pathName = selectedPath.Replace("Assets/","");
            }
            else
            {
                Debug.LogWarning("Not a valid folder");
                return;
            }
        }
    }
}
