// straight from samples of package: https://github.com/marijnz/unity-toolbar-extender

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

namespace Editor
{
    internal static class ToolbarStyles
    {
        public static readonly GUIStyle CommandButtonStyle;

        static ToolbarStyles()
        {
            CommandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }
    
    [InitializeOnLoad]
    public sealed class BaseSceneSwitcher
    {
        static BaseSceneSwitcher()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if(GUILayout.Button(new GUIContent("S", "Start Base Scene"), ToolbarStyles.CommandButtonStyle))
            {
                SceneHelper.StartScene("Startup Scene");
            }
        }
    }

    internal static class SceneHelper
    {
        private static string _sceneToOpen;

        public static void StartScene(string sceneName)
        {
            if(EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }

            _sceneToOpen = sceneName;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate()
        {
            if (_sceneToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // need to get scene via search because the path to the scene
                // file contains the package version so it'll change over time
                var guids = AssetDatabase.FindAssets("t:scene " + _sceneToOpen, null);
                if (guids.Length == 0)
                {
                    Debug.LogWarning("Couldn't find scene file");
                }
                else
                {
                    var scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
            }
            _sceneToOpen = null;
        }
    }
}