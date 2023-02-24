#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

namespace XWEditorResource
{
    public class AddressablesAutoSync
    {
        static AddressableAssetSettings setting { get { return AddressableAssetSettingsDefaultObject.Settings; } }

        private static bool simpleName;
        public static void Excute(bool simpleName)
        {
            AddressablesAutoSync.simpleName = simpleName;
            var so = AddressableAutoSyncSO.Instance;
            var dirs = so.dirs;
            if (dirs == null) return;

            foreach (var dir in dirs)
            {
                CreateGroupWithAllChilds(dir);
            }
            Debug.Log("Addressables同步完成");
        }

        /// <summary>
        /// 用包含所有子对象的方式创建Group
        /// </summary>
        private static void CreateGroupWithAllChilds(string dir)
        {
            
            if (!FindGroup(GetDirName(dir)))
            {
                CreateGroup(dir,GetDirName(dir));
            }
            else
            {
                SyncGroup(dir,GetDirName(dir));
            }
        }

        private static string GetDirName(string dir)
        {
            var splits = dir.Split('/');
            return splits[splits.Length - 1];
        }

        /// <summary>
        /// 获取所有一级目录下的目录与文件
        /// </summary>
        // private static List<string> GetAllFirstDirAndFild(string dir)
        // {
        //     var files = 
        //     var dirs = Directory.GetDirectories(dir);
        //
        //     var address = new List<string>();
        //     address.AddRange(files);
        //     address.RemoveAll(temp => !IsFileMatch(temp));
        //     address.AddRange(dirs);
        //     
        //     return address;
        // }

        private static List<string> GetInvalidFiles(string dir)
        {
            var files = Directory.GetFiles(dir);
            var fileList = files.ToList();
            fileList.RemoveAll(temp => !IsFileMatch(temp));
            return fileList;
        }

        /// <summary>
        /// 支持的文件类型
        /// </summary>
        private static List<string> extensions = new List<string>()
        {
            ".prefab",
            ".mat",
            ".png",
            ".unity",
            ".asset",
            ".physicsMaterial",
            ".ttf",
            ".fbx",
            ".shader",
            ".wav"
        };
        private static bool IsFileMatch(string fileName)
        {
            var end = Path.GetExtension(fileName);
            return extensions.Contains(end);
        }

        /// <summary>
        /// 查找已有目录
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static bool FindGroup(string groupName)
        {
            for (int i = 0; i < setting.groups.Count; ++i)
            {
                if (groupName == setting.groups[i].Name) { return true; }
            }
            return false;
        }
        private static void CreateGroup(string path,string groupName)
        {
            AddressableAssetGroup group = setting.CreateGroup(groupName, false, false, true, new List<AddressableAssetGroupSchema>());//创建Group
            group.AddSchema<ContentUpdateGroupSchema>();
            group.AddSchema<BundledAssetGroupSchema>();
            AddFiles(group, "", path);
        }

        private static void AddFiles(AddressableAssetGroup group,string groupName,string path)
        {            
            var childDirctories = Directory.GetDirectories(path);
            foreach (var dir in childDirctories)
            {
                if (groupName == "")
                    AddFiles(group, GetDirName(dir.Replace("\\","/")), dir);
                else
                    AddFiles(group, $"{groupName}/{GetDirName(dir.Replace("\\","/"))}", dir);
            }

            var files = GetInvalidFiles(path);
            foreach (var file in files)
            {
                string guid = AssetDatabase.AssetPathToGUID(file);//要打包的资产条目   将路径转成guid

                var temp = file.Replace("\\","/");
                var address = "";
                if (simpleName) {
                    address = Path.GetFileName(temp);
                } else {
                    address = groupName == "" ? GetDirName(temp) : $"{groupName}/{GetDirName(temp)}";
                }
                if (setting.FindAssetEntry(guid) == null)
                {
                    Debug.Log(address);
                    AddressableAssetEntry entry = setting.CreateOrMoveEntry(guid, group,false,true);//要打包的资产条目   会将要打包的路径移动到group节点下
                    if (entry == null) continue;
                    Debug.Log("创建成功");
                    entry.SetAddress(address, true);
                }
                else
                {
                    setting.FindAssetEntry(guid).SetAddress(address,true);
                }
            }
        }

        private static void SyncGroup(string path,string groupName)
        {
            var group = setting.FindGroup(groupName);
            AddFiles(group, "", path);
        }
    }
}
#endif