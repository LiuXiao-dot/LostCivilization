using System;
using UnityEditor;
using UnityEngine;

namespace XWUtility
{
    /// <summary>
    /// 调试
    /// </summary>
    public static class XWLogger
    {
        /// <summary>
        /// 调试等级
        /// </summary>
        public static XWLoggerLevel loggerLevel =
#if UNITY_EDITOR
            XWLoggerLevel.DEBUG | XWLoggerLevel.WARNING | XWLoggerLevel.ERROR;
#else
            XWLoggerLevel.OUTPUT;
#endif

        public static void ChangeLoggerLevel(IXWLoggerSetter loggerSetter)
        {
            loggerLevel = loggerSetter.GetLoggerLevel();
        }

        public static void Log(string message = null)
        {
            if (loggerLevel.HasFlag(XWLoggerLevel.DEBUG))
            {
                Debug.Log(message);
            }
        }

        public static void Warning(string message = null)
        {
            if (loggerLevel.HasFlag(XWLoggerLevel.WARNING))
            {
                Debug.LogWarning(message);
            }
        }

        public static void Error(string message = null)
        {
            if (loggerLevel.HasFlag(XWLoggerLevel.ERROR) || loggerLevel.HasFlag(XWLoggerLevel.OUTPUT))
            {
                Debug.LogError(message);
            }
        }
    }
}