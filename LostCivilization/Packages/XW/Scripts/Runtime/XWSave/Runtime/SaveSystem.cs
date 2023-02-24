using System.IO;
using UnityEngine;
using XWUtility;

namespace XWSave
{
    /// <summary>
    /// 存档系统
    /// </summary>
    public static class SaveSystem
    {

        private const string SAVE_EXTENSION = "txt";

        private static string SAVE_FOLDER = Application.dataPath + "/Saves/";
        private static bool isInit = false;

        public static void SetFolder(string path)
        {
            SAVE_FOLDER = path;
        }

        public static void Init()
        {
            if (!isInit)
            {
                isInit = true;
                // Test if Save Folder exists
                if (!Directory.Exists(SAVE_FOLDER))
                {
                    // Create Save Folder
                    Directory.CreateDirectory(SAVE_FOLDER);
                }
            }
        }

        public static void Save(string fileName, string saveString, bool overwrite)
        {
            Init();
            string saveFileName = fileName;
            if (!overwrite)
            {
                // Make sure the Save Number is unique so it doesnt overwrite a previous save file
                int saveNumber = 1;
                while (File.Exists(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION))
                {
                    saveNumber++;
                    saveFileName = fileName + "_" + saveNumber;
                }
                // saveFileName is unique
            }
            File.WriteAllText(SAVE_FOLDER + saveFileName + "." + SAVE_EXTENSION, saveString);
        }

        public static string Load(string fileName)
        {
            Init();
            var path = $"{SAVE_FOLDER}{fileName}.{SAVE_EXTENSION}";
            if (File.Exists(path))
            {
                #if UNITY_EDITOR
                XWLogger.Log($"存档:{path}");
                #endif
                string saveString = File.ReadAllText(path);
                return saveString;
            }
            else
            {
                return null;
            }
        }

        public static string LoadMostRecentFile()
        {
            Init();
            DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
            // Get all save files
            FileInfo[] saveFiles = directoryInfo.GetFiles("*." + SAVE_EXTENSION);
            // Cycle through all save files and identify the most recent one
            FileInfo mostRecentFile = null;
            foreach (FileInfo fileInfo in saveFiles)
            {
                if (mostRecentFile == null)
                {
                    mostRecentFile = fileInfo;
                }
                else
                {
                    if (fileInfo.LastWriteTime > mostRecentFile.LastWriteTime)
                    {
                        mostRecentFile = fileInfo;
                    }
                }
            }

            // If theres a save file, load it, if not return null
            if (mostRecentFile != null)
            {
                string saveString = File.ReadAllText(mostRecentFile.FullName);
                return saveString;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="saveObject"></param>
        public static void SaveObject(object saveObject)
        {
            SaveObject("save", saveObject, false);
        }

        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="saveObject"></param>
        /// <param name="overwrite"></param>
        public static void SaveObject(string fileName, object saveObject, bool overwrite)
        {
            Init();
            string json = JsonUtility.ToJson(saveObject);
            Save(fileName, json, overwrite);
        }

        /// <summary>
        /// 加载最近的对象存档
        /// </summary>
        /// <typeparam name="TSaveObject"></typeparam>
        /// <returns></returns>
        public static TSaveObject LoadMostRecentObject<TSaveObject>()
        {
            Init();
            string saveString = LoadMostRecentFile();
            if (saveString != null)
            {
                TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
                return saveObject;
            }
            else
            {
                return default(TSaveObject);
            }
        }

        /// <summary>
        /// 指定文件名加载对象存档
        /// </summary>
        /// <typeparam name="TSaveObject"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static TSaveObject LoadObject<TSaveObject>(string fileName)
        {
            Init();
            string saveString = Load(fileName);
            if (saveString != null)
            {
                TSaveObject saveObject = JsonUtility.FromJson<TSaveObject>(saveString);
                return saveObject;
            }
            else
            {
                return default(TSaveObject);
            }
        }

    }
} 