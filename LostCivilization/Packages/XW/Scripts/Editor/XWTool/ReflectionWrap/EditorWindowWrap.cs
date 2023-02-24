namespace XWTool
{
    using System;
    using UnityEditor;
    using System.Reflection;

    /// <summary>
    /// 编辑器窗口
    /// </summary>
    public static class EditorWindowWrap
    {
        private static Type _editorWindowType;

        public static Type EdtiorWindowType
        {
            get
            {
                if (_editorWindowType == null) _editorWindowType = typeof(EditorWindow);
                return _editorWindowType;
            }
        }

        /// <summary>
        /// 匹配父物体尺寸
        /// </summary>
        /// <param name="instance"></param>
        public static void MakeParentsSettingsMatchMe(object instance)
        {
            MethodInfo mInfo = EdtiorWindowType.GetMethod("MakeParentsSettingsMatchMe", BindingFlags.Instance | BindingFlags.NonPublic);
            if (mInfo==null) return;
            mInfo.Invoke(instance, null);
        }

        public static object GetDockArea(this EditorWindow editorWindow)
        {
            var dockArea = typeof(EditorWindow).GetField("m_Parent", BindingFlags.Instance | BindingFlags.NonPublic);
            return dockArea.GetValue(editorWindow);
        }
    }

}