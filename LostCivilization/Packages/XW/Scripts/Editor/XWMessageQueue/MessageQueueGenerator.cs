#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEditor.VersionControl;
using XWConverter;
using XWMessageQueue;
using XWTool;
using XWUtility;

namespace XWEditorMessageQueue
{
    /// <summary>
    /// 消息队列代码生成
    /// </summary>
    [XWFilePath("MessageQueueGenerator.asset", XWFilePathAttribute.PathType.InXWEditor)]
    [Tool("消息队列")]
    public class MessageQueueGenerator : SerializedScriptableObject
    {
        [ValueDropdown("CheckType")] public Type[] messages;

        [Button("生成消息队列代码")]
        public void GenerateAll()
        {
            if (messages == null) return;
            foreach (var message in messages)
            {
                if (message == null) continue;
                CreateScript(message);
            }
        }

        private void CreateScript(Type enumType)
        {
            var typeText = enumType.Name;
            var className = $"{typeText}Queue";
            var messageQueueAttribute = CustomAttributeExtensions.GetCustomAttribute<MessageQueueAttribute>(enumType) as MessageQueueAttribute;
            var isMain = messageQueueAttribute.isMainThread;
            var namespaceText = enumType.Namespace;
            var initText = $"var newObject = new GameObject(\"{className}\");";
            var parent = isMain
                ? $"AMainThreadMessageQueue<{typeText}>"
                : $"AChildThreadMessageQueue<{typeText}>";
            var path = (AssetDatabase.FindAssets($"t:Script {typeText}").Select(AssetDatabase.GUIDToAssetPath).First(temp => Path.GetFileName(temp) == $"{typeText}.cs"));
            var dir = Path.GetDirectoryName(path);
            
            var messageQueueText = @$"
// 自动生成的代码，请勿修改
using UnityEngine;
using XWMessageQueue;
namespace {namespaceText}{{
    public class {className} : {parent}
    {{
        public static {className} Instance
        {{
            get{{
                if(_instance == null)
                {{
                    {initText}
                    GameObject.DontDestroyOnLoad(newObject);
                    _instance = newObject.AddComponent<{className}>();
                }}
                return _instance;
            }}
        }}
        private static {className} _instance;

        public static bool SubscribeS(IGameEventListener<{typeText}> newListener, params {typeText}[] operates)
        {{
            return Instance.Subscribe(newListener, operates);
        }}

        public static bool UnSubscribeS(IGameEventListener<{typeText}> newListener, params {typeText}[] operates)
        {{
            return Instance.UnSubscribe(newListener, operates);
        }}

        public static void SendGameEventS({typeText} operate, params object[] args)
        {{
            Instance.SendGameEvent(operate, args);
        }}

        public static void SendGameEventImmediateS({typeText} operate, params object[] args)
        {{
            Instance.SendGameEventImmediate(operate, args);
        }}
    }}
}}";
            File.WriteAllText(Path.Combine(dir,$"{className}.cs"), messageQueueText);
            XWLogger.Log(Path.Combine(dir,$"{className}.cs"));
        }

        public IEnumerable<Type> CheckType()
        {
            var toolConfig = ToolMenuConfig.Instance;
            var temp = new List<Type>();
            toolConfig.GetAllChildType<Enum>(temp, false);
            temp.RemoveAllUnMarkedType<MessageQueueAttribute>();
            if (messages != null)
            {
                var old = messages.ToList();
                temp.RemoveAll(temp => old.Exists(temp2 => temp.FullName == temp2.FullName));
            }

            return temp;
        }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void Init()
        {
            EditorUtils.CheckAsset<MessageQueueGenerator>();
        }
#endif
    }
}
#endif