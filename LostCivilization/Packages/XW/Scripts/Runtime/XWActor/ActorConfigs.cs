using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XWDataStructure;
using XWUtility;
namespace XWActor
{
    /// <summary>
    /// Actor全部配置
    /// </summary>
    [XWFilePath("ActorConfigs.asset",XWFilePathAttribute.PathType.InXW)]
    public class ActorConfigs : ScriptableObjectSingleton<ActorConfigs>
    {
        public SerializableDictionary<string, ActorConfig> configs;
        
        #if UNITY_EDITOR
        public void OnValidate()
        {
            // 检查配置是否存在，不存在则生成
            var folderPath = XWFilePathAttribute.GetPath("ActorConfigs", XWFilePathAttribute.PathType.InXW);
            FileExtension.CheckDirectory(folderPath);
            var newConfigs = new List<ActorConfig>();
            foreach (var config in configs) {
                var fullPath = $"{folderPath}/{config.Key}.asset";
                if (config.Value == null) {
                    // 创建一个
                    ActorConfig newSO = ScriptableObject.CreateInstance<ActorConfig>();
                    newSO.actorType = config.Key;
                    AssetDatabase.CreateAsset(newSO,fullPath);
                    newConfigs.Add(newSO);
                }
            }
            var length = newConfigs.Count;
            for (int i = 0; i < length; i++) {
                var newConfig = newConfigs[i];
                configs[newConfig.actorType] = newConfig;
            }
        }
        #endif
    }
}