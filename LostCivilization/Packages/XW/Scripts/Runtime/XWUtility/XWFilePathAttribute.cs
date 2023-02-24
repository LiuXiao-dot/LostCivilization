using System;
using XWUtility;

namespace XWUtility
{
    /// <summary>
    /// 文件路径
    /// </summary>
    public class XWFilePathAttribute : Attribute
    {
        public static string XWPATH = "Assets/XW/XWResources";
        public static string XWEDITORPATH = "Assets/XW/EditorResources";
        /// <summary>
        /// 路径类型
        /// </summary>
        public enum PathType
        {
            /// <summary>
            /// XW的EditorResources目录
            /// </summary>
            XWEditor,
            /// <summary>
            /// XW的Resources目录
            /// </summary>
            XW,
            /// <summary>
            /// 相对与项目的绝对路径
            /// </summary>
            Absolute,
            /// <summary>
            /// 包内
            /// </summary>
            InXWEditor,
            /// <summary>
            /// 包内
            /// </summary>
            InXW,
        }
        public string filePath;
        
        public XWFilePathAttribute(string filePath, PathType pathType = PathType.Absolute)
        {
            switch (pathType)
            {
                case PathType.InXWEditor:
                case PathType.XWEditor:
                    FileExtension.CheckDirectory("Assets/XW/EditorResources/Generates");
                    this.filePath = $"Assets/XW/EditorResources/Generates/{filePath}";
                    break;
                case PathType.XW:
                case PathType.InXW:
                    FileExtension.CheckDirectory("Assets/XW/XWResources/Generates");
                    this.filePath = $"Assets/XW/XWResources/Generates/{filePath}";
                    break;
                case PathType.Absolute:
                    this.filePath = filePath;
                    break;
                    /*FileExtension.CheckDirectory("Packages/XW/EditorResources/Generates");
                    this.filePath =  $"Packages/XW/EditorResources/Generates/{filePath}";*/
                    /*FileExtension.CheckDirectory("Packages/XW/XWResources/Generates");
                    this.filePath =  $"Packages/XW/XWResources/Generates/{filePath}";*/
                default:
                    this.filePath = filePath;
                    break;
            }
        }

        public string GetPath()
        {
            return this.filePath;
        }
    }
}