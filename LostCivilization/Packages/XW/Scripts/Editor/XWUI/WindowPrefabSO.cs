using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XWConverter;
using XWUI;
using XWUtility;

namespace XWUIEditor
{
    /// <summary>
    /// 窗口Prefab
    /// </summary>
    public class WindowPrefabSO : ScriptableObject
    {
        protected const string LEFT_HOR_GROUP = "窗口编辑/窗口配置";
        protected const string RIGHT_HOR_GROUP = "窗口编辑/预览";

        protected const string LEFT_STAT_GROUP = "窗口编辑/窗口配置/配置数据";
        [ReadOnly] [FolderPath] public string PrefabDirectory = "Assets/Game/Arts/Windows";
        [ReadOnly] [FolderPath] public string CodeDirectory = "Assets/Game/Scripts/Windows";


        [HorizontalGroup("窗口编辑")]
        [HorizontalGroup(LEFT_HOR_GROUP)]
        [BoxGroup(LEFT_STAT_GROUP)]
        [ReadOnly]
        public string Name;

        [BoxGroup(LEFT_STAT_GROUP)]
        [LabelText("窗口背景")]
        public WindowBg windowBg;

        [BoxGroup(LEFT_STAT_GROUP)]
        [LabelText("窗口层级")]
        public WindowLayer windowLayer;

        [BoxGroup(LEFT_STAT_GROUP)]
        [LabelText("窗口顶部栏")]
        public WindowHeader windowHeader;

        [BoxGroup(LEFT_STAT_GROUP)]
        [ReadOnly]
        [Tooltip("该部分只能在源Prefab中编辑")]
        public GameObject customWindow;

        [ReadOnly] [BoxGroup(LEFT_STAT_GROUP)] public MonoScript viewCode;
        [ReadOnly] [BoxGroup(LEFT_STAT_GROUP)] public MonoScript ctlCode;

        [ReadOnly]
        [HorizontalGroup(RIGHT_HOR_GROUP)]
        [TextArea(10, 200)]
        public string viewCodePreview;

        private void OnEnable()
        {
            bool needRefresh = ctlCode == null || viewCode == null;
            viewCode = AssetDatabase.LoadAssetAtPath<MonoScript>($"{CodeDirectory}/{Name}View.cs");
            if (viewCode != null)
            {
                viewCodePreview = File.ReadAllText($"{CodeDirectory}/{Name}View.cs");
            }

            ctlCode = AssetDatabase.LoadAssetAtPath<MonoScript>($"{CodeDirectory}/{Name}Ctl.cs");
            try
            {
                if (!customWindow.TryGetComponent(viewCode.GetClass(), out Component a))
                {
                    customWindow.AddComponent(viewCode.GetClass());
                }
                if (!customWindow.TryGetComponent(typeof(GraphicRaycaster), out a))
                {
                    customWindow.AddComponent<GraphicRaycaster>();
                }
            }
            catch
            {

            }
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// 有改动后刷新Prefab
        /// </summary>
        private void OnValidate()
        {
            //PrefabUtility.InstantiatePrefab();
            if (customWindow == null) return;
        }

        /// <summary>
        /// 更新或生成代码
        /// </summary>
        [Button("更新代码")]
        public void UpdateCode()
        {
            var dirPath = CodeDirectory;
            var path = $"{dirPath}/{Name}View.cs";
            var ctlPath = $"{dirPath}/{Name}Ctl.cs";
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            //using (var s = File.Create(path))
            // 写入代码
            var reader = new WindowPrefabReader();
            var writer = new CSharpFileWriter();
            AILData from = reader.Read(this);
            var convert = new UIPrefab2CSharpViewConvertor("GameUI");
            writer.Write(convert.Convert(from as DictionaryILData), path);
            if (!File.Exists(ctlPath))
                File.WriteAllText(ctlPath, GetCtlCode(this));
            EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// 初始化所有资源：prefab+code
        /// </summary>
        public void InitAllAssets(string name)
        {
            this.Name = name;
            this.ctlCode = null;
            this.viewCode = null;
            customWindow = CreateWindowPrefab();
            UpdateCode();
        }

        internal GameObject CreateWindowPrefab()
        {
            var windowName = this.Name;
            var totalPath = $"{PrefabDirectory}/{windowName}.prefab";
            if (AssetDatabase.LoadAssetAtPath<GameObject>(totalPath) != null)
            {
                return AssetDatabase.LoadAssetAtPath<GameObject>(totalPath);
            }

            GameObject returnGo = null;
            var rootCanvas =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    $"{XWFilePathAttribute.XWPATH}/Prefabs/Scene/UIScenePrefab.prefab");
            var rootCanvasGo = (GameObject)PrefabUtility.InstantiatePrefab(rootCanvas);
            var canvasGo = new GameObject(windowName);
            try
            {
                canvasGo.transform.SetParent(rootCanvasGo.GetComponentInChildren<Canvas>().transform);
                var rectTrans = canvasGo.AddComponent<RectTransform>();
                rectTrans.localScale = Vector3.one;
                rectTrans.anchorMin = Vector2.zero;
                rectTrans.anchorMax = Vector2.one;
                rectTrans.offsetMin = Vector2.zero;
                rectTrans.offsetMax = Vector2.zero;
                var canvas = canvasGo.AddComponent<Canvas>();
                canvas.overrideSorting = true;
                canvas.overridePixelPerfect = true;
                canvas.sortingLayerName = "UI";
                canvas.sortingOrder = 0;
                canvas.additionalShaderChannels = AdditionalCanvasShaderChannels.None;
                canvasGo.layer = 5;

                PrefabUtility.SaveAsPrefabAsset(canvasGo, totalPath, out bool savedSuccessfully);
                if (savedSuccessfully)
                {
                    returnGo = AssetDatabase.LoadAssetAtPath<GameObject>(totalPath);
                    //EditorUtility.SetDirty(returnGo);
                }
                else
                    XWLogger.Error($"保存失败{totalPath}");
            }
            catch (Exception e)
            {
                XWLogger.Error(e.ToString());
                throw;
            }
            finally
            {
                GameObject.DestroyImmediate(canvasGo);
                GameObject.DestroyImmediate(rootCanvasGo);
            }

            return returnGo;
        }

        public static string GetCtlCode(WindowPrefabSO so)
        {
            var ctlCode = $@"
using XWUI;
namespace GameUI
{{
    public class {so.Name}Ctl : AWindowCtl
    {{
        protected override void OnOpen()
        {{
        }}
        
        protected override void OnPause()
        {{
        }}
        
        protected override void OnResume()
        {{
        }}
        
        protected override void OnClose()
        {{
        }}
    }}
}}";
            return ctlCode;
        }

        /// <summary>
        /// 销毁窗口
        /// </summary>
        internal void DestroyWindow()
        {
            if (customWindow)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(customWindow));
            if (ctlCode)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(ctlCode));
            if (viewCode)
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(viewCode));

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
        }
    }
}