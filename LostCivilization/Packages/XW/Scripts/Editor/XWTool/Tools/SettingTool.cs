using System.Reflection;
using TMPro;
using XWDataStructure;
using XWUtility;

namespace XWTool
{
    [Tool(ToolConstant.Home + "/设置")]
    [XWFilePath("SettingTool.asset",XWFilePathAttribute.PathType.InXWEditor)]
    public sealed class SettingTool : ScriptableObjectSingleton<SettingTool>
    {
        public TMP_FontAsset defaultFontAsset;

        private void OnValidate()
        {
            Reset();
        }

        public void Reset()
        {
            var tmpSettings = TMP_Settings.instance;
            if(tmpSettings == null) return;
            var field = tmpSettings.GetType().GetField("m_defaultFontAsset",BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null) field.SetValue(tmpSettings, defaultFontAsset);
        }
    }
}