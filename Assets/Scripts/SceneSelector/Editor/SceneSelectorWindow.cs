#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace MacUtils.SceneSelector
{
    /// <summary>
    /// Creates a window that allows the user to select a scene to open.
    /// A label is created for each folder within the SceneManagement folder, and within each folder, a button is created for each scene.
    /// </summary>
    public class SceneSelectorWindow : EditorWindow
    {
        private VisualElement rootElement;
        private ScrollView scrollView;

        [MenuItem("Tools/Scene Selector")]
        private static void OpenWindow()
        {
            GetWindow<SceneSelectorWindow>();
        }

        private void OnEnable()
        {
            rootElement = rootVisualElement;
            scrollView = new(ScrollViewMode.Vertical);
            rootElement.Add(scrollView);
        }

        private void CreateGUI()
        {
            CreateHeader();

            // Find the SceneManagement folder
            string sceneManagementFolder = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("SceneManagement")[0]);

            // Get all subfolders within SceneManagement folder
            string[] subfolders = AssetDatabase.GetSubFolders(sceneManagementFolder);

            foreach (string folderPath in subfolders)
            {
                // Create label for each folder
                string folderName = System.IO.Path.GetFileName(folderPath);
                CreateFolderLabel(folderName);

                // Find all scenes within the folder
                string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { folderPath });

                foreach (string sceneGuid in sceneGuids)
                    CreateSceneButton(sceneGuid);

                // Add splitter line between each folder
                AddSplitterLine();
            }
        }

        private void CreateHeader()
        {
            VisualElement header = new();
            Label headerLabel = new("Scene Selector")
            {
                style =
                {
                    borderBottomColor = new Color(0.5f, 0.5f, 0.5f, 1f),
                    fontSize = 20,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    marginTop = 30,
                    marginBottom = 30,

                }
            };
            header.Add(headerLabel);

            scrollView.Add(header);
        }

        private void CreateFolderLabel(string folderName)
        {
            VisualElement folderLabelContainer = new();

            // Create label for folder
            Label label = new(folderName)
            {
                style =
                {
                    fontSize = 16,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    marginTop = 10,
                    marginBottom = 5
                }
            };
            folderLabelContainer.Add(label);

            scrollView.Add(folderLabelContainer);
        }

        private void CreateSceneButton(string sceneGuid)
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
            VisualElement buttonGroup = new();
            buttonGroup.style.unityTextAlign = TextAnchor.MiddleCenter;

            Object sceneAsset = AssetDatabase.LoadAssetAtPath(scenePath, typeof(SceneAsset));

            Button openButton = new(() =>
            {
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            })
            {
                text = sceneAsset.name,
                style =
                {
                    height = 30,
                    marginTop = 5,
                    marginBottom = 5,
                    marginRight = 50,
                    marginLeft = 50
                }
            };
            buttonGroup.Add(openButton);

            scrollView.Add(openButton);
        }

        private void AddSplitterLine()
        {
            // Create line below label
            VisualElement line = new()
            {
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    backgroundColor = Color.gray,
                    height = 1,
                    marginTop = 15,
                    marginBottom = 30,
                    marginLeft = 80,
                    marginRight = 80
                }
            };
            scrollView.Add(line);
        }
    }
}
#endif
