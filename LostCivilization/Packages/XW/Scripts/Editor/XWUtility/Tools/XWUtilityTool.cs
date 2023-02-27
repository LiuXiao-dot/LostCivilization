using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XWDataStructure;
using XWTool;
using XWUtility;

namespace XWUtilityEditor
{
    [Tool("调试/工具配置")]
    [XWFilePath("XWUtilityTool.asset", XWFilePathAttribute.PathType.InXWEditor)]
    public class XWUtilityTool : ScriptableObjectSingleton<XWUtilityTool>, IXWLoggerSetter
    {
        [LabelText("调试等级")] [Tooltip("OUTPUT:打包之后的调试方式")]
        public XWLoggerLevel logLevel;

        [SerializeField][ReadOnly]private string _dotsDefine = "XW_ENABLE_DOTS";
        
        [LabelText("启用DOTS")]
        [ShowInInspector]
        public bool enableDots
        {
            get => PlayerSettings
                .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup)
                .Contains(_dotsDefine);
            set
            {
                var defines = PlayerSettings
                    .GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                var hasPartialDefine = defines
                    .Contains(_dotsDefine);
                if (value && !hasPartialDefine)
                {
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        $"{defines};{_dotsDefine}");
                }
                else if (!value && hasPartialDefine)
                {
                    defines = defines.Remove(defines.IndexOf(_dotsDefine, StringComparison.Ordinal),
                        _dotsDefine.Length);
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                        defines);
                }

            }   
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            XWLogger.ChangeLoggerLevel(Instance);
        }

        public XWLoggerLevel GetLoggerLevel()
        {
            return logLevel;
        }

        private void OnValidate()
        {
            if(Instance)
                XWLogger.ChangeLoggerLevel(Instance);
        }
    }
}