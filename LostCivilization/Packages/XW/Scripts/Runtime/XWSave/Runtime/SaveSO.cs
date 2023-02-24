using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace XWSave
{
    /// <summary>
    /// 存档SO
    /// </summary>
    public class SaveSO : ScriptableObject
    {
        private static string baseAddress;

        /// <summary>
        /// 除GetFolder方法外，不可调用
        /// </summary>
        public string folder;

        public string baseFileName;

        private bool isInit = false;
        [SerializeField]
        private string extension = "xw";


        /// <summary>
        /// 检测目标文件夹是否存在
        /// </summary>
        private void Init()
        {
            if (!isInit)
            {
                isInit = true;
                if (!Directory.Exists(GetFolder()))
                {
                    Directory.CreateDirectory(GetFolder());
                }
            }
        }

        private void OnEnable()
        {
            SaveSO.baseAddress = Application.persistentDataPath.Replace('/','\\');
            Init();
        }

        #region 保存
        /// <summary>
        /// 保存基础方法 保存字符串到指定文件
        /// </summary>
        /// <param name="saveString"></param>
        /// <param name="overwrite">是否覆盖</param>
        private string BaseSave(string unSavedfileName, string saveString, bool overwrite)
        {
            Init();
            string saveFileName = unSavedfileName;
            if (!overwrite)
            {
                int saveNumber = 1;
                while (File.Exists(GetTotalAddress(saveFileName)))
                {
                    saveFileName = baseFileName + "_" + saveNumber;
                    saveNumber++;
                }
            }

            var writer = new StreamWriter(GetTotalAddress(saveFileName));
            writer.Write(saveString);
            writer.Close();

            //var stream = File.Create(GetTotalAddress(saveFileName));
            //File.WriteAllText(saveFileName, saveString);
            //stream.Close();
            return GetTotalAddress(saveFileName);
        }

        /// <summary>
        /// 保存基础方法 保存bytep[]到指定文件
        /// </summary>
        /// <param name="unSavedfileName"></param>
        /// <param name="datas"></param>
        /// <param name="overwrite"></param>
        private void BaseSave(string unSavedfileName, byte[] datas, bool overwrite)
        {
            Init();
            string saveFileName = unSavedfileName;
            if (!overwrite)
            {
                int saveNumber = 1;
                while (File.Exists(GetTotalAddress(saveFileName)))
                {
                    saveFileName = baseFileName + "_" + saveNumber;
                    saveNumber++;
                }
            }
            File.WriteAllBytes(GetTotalAddress(saveFileName), datas);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="savedData">待保存数据(可序列化)</param>
        public string Save<T>(T savedData, bool overwrite = true)
        {
            return Save(baseFileName, savedData, overwrite);
        }

        public string Save<T>(string fileName,T savedData, bool overwrite = true)
        {
            string json = JsonUtility.ToJson(savedData);
            return BaseSave(fileName, json, overwrite);
        }

        /// <summary>
        /// 加密保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="savedData"></param>
        /// <param name="overwrite"></param>
        public void SaveAES<T>(T savedData, bool overwrite)
        {
            string json = JsonUtility.ToJson(savedData);
            var aesMaker = new AESMaker();
            aesMaker.c = AESSystem.Encrypt(json, out aesMaker.a, out aesMaker.b);
            BaseSave(baseFileName, JsonUtility.ToJson(aesMaker), overwrite);
        }
        #endregion

        #region 读取
        /// <summary>
        /// 从给定地址加载文件并转换为string
        /// </summary>
        /// <param name="totalPath">路径</param>
        /// <returns></returns>
        private string LoadWithTotalPath(string totalPath)
        {
            if (File.Exists(totalPath))
            {

                var reader = new StreamReader(totalPath);
                var saveString = reader.ReadToEnd();
                reader.Close();
                return saveString;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据路径加载文件并转换为T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public T LoadWithPath<T>(string path)
        {
            string saveString = LoadWithTotalPath(path);
            if (saveString != null)
            {
                T saveObject = JsonUtility.FromJson<T>(saveString);
                return saveObject;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 根据给定文件名加载文件
        /// </summary>
        /// <param name="savedFileName">文件名</param>
        /// <returns></returns>
        private string Load(string savedFileName)
        {
            Init();
            var totalPath = GetTotalAddress(savedFileName);
            return LoadWithTotalPath(totalPath);
        }

        /// <summary>
        /// 加载并创建一个新的T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Load<T>(string savedFileName)
        {
            string saveString = Load(savedFileName);
            if (saveString != null)
            {
                T saveObject = JsonUtility.FromJson<T>(saveString);
                return saveObject;
            }
            else
            {
                return default(T);
            }
        }

        /// <summary>
        /// 以解密方式加载文件并返回T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="savedFileName"></param>
        /// <returns></returns>
        public T LoadAES<T>(string savedFileName)
        {
            var marker = Load<AESMaker>(savedFileName);
            if(marker != null)
            {
                var data = AESSystem.Decrypt(marker.c, marker.a, marker.b);
                return JsonUtility.FromJson<T>(data);
            }
            return default(T);
        }

        /// <summary>
        /// 在给定文件夹加载全部文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> LoadAllDirectly<T>()
        {
            if (!Directory.Exists(GetFolder()))
            {
                return default(List<T>);
            }

            var savedFiles = Directory.GetFiles(GetFolder(), $"*.{extension}");
            if (savedFiles != null && savedFiles.Length > 0)
            {
                List<T> values = null;
                foreach (var file in savedFiles)
                {
                    var value = LoadWithPath<T>(file);
                    if (value == null)
                    {
                        Debug.Log(file + "unexit");
                        continue;
                    }
                    if (values == null)
                    {
                        values = new List<T>();
                    }
                    Debug.Log(file + "exit");
                    values.Add(value);
                }
                return values;
            }
            return default(List<T>);
        }

        /// <summary>
        /// 以解密方式加载指定文件夹下全部文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> LoadAllAES<T>()
        {
            List<T> values = null;
            var aesMakers = LoadAllDirectly<AESMaker>();
            if (aesMakers != null)
            {
                foreach (var marker in aesMakers)
                {
                    var data = AESSystem.Decrypt(marker.c, marker.a, marker.b);
                    if (values == null)
                    {
                        values = new List<T>();
                    }
                    values.Add(JsonUtility.FromJson<T>(data));
                }
            }
            return values;
        }
        #endregion


        public string GetTotalAddress(string fileName)
        {
            Debug.Log(Path.Combine(GetFolder(), fileName + "." + extension));
            return Path.Combine(GetFolder(), fileName + "." + extension);
        }

        private string GetFolder()
        {
            return Path.Combine(baseAddress,folder);
        }
    }

    /// <summary>
    /// 加密数据
    /// </summary>
    [Serializable]
    public class AESMaker
    {
        // key
        public byte[] a;
        // IV
        public byte[] b;
        // data
        public byte[] c;
    }
}