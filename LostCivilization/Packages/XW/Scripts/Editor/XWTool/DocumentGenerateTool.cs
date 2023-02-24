using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Device;
using XWDataStructure;
using XWTool;
using XWUtility;
using Debug = UnityEngine.Debug;

namespace DocumentGenerator
{
    /// <summary>
    /// 代码文档生成工具
    /// </summary>
    [Tool("调试/文档生成")]
    [XWFilePath("DocumentGenerateTool.asset", XWFilePathAttribute.PathType.InXWEditor)]
    public class DocumentGenerateTool : ScriptableObjectSingleton<DocumentGenerateTool>
    {

        public AssemblyDefinitionAsset[] assmblys;
        
        private string defaultPath;

        /// <summary>
        /// 调用外部bat生成API
        /// todo:自动生成文章
        /// </summary>
        [Button(ButtonSizes.Large, Name = "生成文档")]
        public void Generate()
        {
            if(assmblys == null) return;
            defaultPath = $"{EditorUtils.BasePath}/Packages/XW/.Tools/DocGenerate/XWDocumentGeneratorExp.exe";
            var initPah = $"{EditorUtils.BasePath}/Assets/XW";
            var slnPath = $"{EditorUtils.BasePath}/{Application.productName}.sln";
            var targetPath = "";
            foreach (var assembly in assmblys)
            {
                targetPath = $"{targetPath}${assembly.name}";
            }

            var process = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            if (File.Exists(defaultPath))
            {
                info.FileName = defaultPath;
            }
            else
            {
                info.FileName = Environment.GetEnvironmentVariable("DOCGENERATE",EnvironmentVariableTarget.User);
            }
            
            if (string.IsNullOrEmpty(info.FileName))
            {
                Debug.LogError("缺少生成工具或需要配置环境变量");
                return;
            }

            info.Arguments = $"{initPah} {slnPath} {targetPath}";
            /*info.ArgumentList.Add(initPah);
            info.ArgumentList.Add(targetPath);*/
            info.CreateNoWindow = false;
            info.WindowStyle = ProcessWindowStyle.Normal;
            process.StartInfo = info;
            EditorUtility.DisplayCancelableProgressBar("生成文档",$"文档生成中{initPah}",0);
            try
            {
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
      
        }
    }
}