#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XWDataStructure;
using XWTool;
using XWUtility;

namespace XWScene
{
    /// <summary>
    /// 场景设置工具
    /// </summary>
    [Tool("场景管理")]
    [XWFilePath("SceneSettingTool.asset", XWFilePathAttribute.PathType.InXWEditor)]
    public class SceneSettingTool : ScriptableObjectSingleton<SceneSettingTool>
    {
        /// <summary>
        /// 生成初始场景的文件夹
        /// </summary>
        [FolderPath] [LabelText("生成初始场景的文件夹")] public string folder;

        [LabelText("新建场景的名字")] public string newScene;

        [LabelText("launch场景的初始化预设")] public GameObject launchScenePrefab;

        [LabelText("UI场景的初始化预设")] public GameObject uiScenePrefab;

        [LabelText("Game场景的初始化预设")] public GameObject gameScenePrefab;

        /// <summary>
        /// 生成Launch,UI,Game三个场景与它们相关的SO文件.
        /// 同时生成SceneManager的Prefab直接就可以使用
        /// Launcher:第一个场景，负责加载UI场景
        /// UI:所有2DUI元素存在的场景
        /// Game:3D/2D游戏场景
        /// </summary>
        [Button("生成初始化场景")]
        public void Init()
        {
            if (!EditorUtility.DisplayDialog("场景初始化确认", "是否初始化场景？若已有场景，会被覆盖掉", "创建", "取消")) return;

            // ui
            var uiScene = CreateScene("UIScene");
            if (uiScenePrefab != null)
            {
                var uiGo = (GameObject)PrefabUtility.InstantiatePrefab(uiScenePrefab);
                EditorSceneManager.MoveGameObjectToScene(uiGo, uiScene);
                EditorSceneManager.SaveScene(uiScene);
            }

            // launch
            var launchScene = CreateScene("LaunchScene");
            var sceneManagerGo = new GameObject("SceneManager", typeof(XWSceneManager));
            EditorSceneManager.MoveGameObjectToScene(sceneManagerGo, launchScene);
            if (launchScenePrefab != null)
            {
                var launchGo = (GameObject)PrefabUtility.InstantiatePrefab(launchScenePrefab);
                EditorSceneManager.MoveGameObjectToScene(launchGo, launchScene);
            }

            EditorSceneManager.SaveScene(launchScene);

            // game
            var gameScene = CreateScene("GameScene");
            if (gameScenePrefab != null)
            {
                var gameGo = (GameObject)PrefabUtility.InstantiatePrefab(gameScenePrefab);
                EditorSceneManager.MoveGameObjectToScene(gameGo, gameScene);
                EditorSceneManager.SaveScene(gameScene);
            }

            //EditorUtility.RevealInFinder(folder); 
        }

        [Button("新建场景")]
        public void Generate()
        {
            if (string.IsNullOrEmpty(newScene))
                return;
            CreateScene(newScene);
        }

        private Scene CreateScene(string sceneName)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            scene.name = sceneName;

            var scenePath = $"{folder}/{sceneName}.unity";
            var sceneSOPath = $"{folder}/SceneConfig/{sceneName}.asset";

            if (!EditorSceneManager.SaveScene(scene, scenePath))
            {
                XWLogger.Error($"场景创建失败{scenePath}");
            }

            return scene;
        }
    }
}
#endif