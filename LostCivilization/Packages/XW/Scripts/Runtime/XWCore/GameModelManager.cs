using System;
using XWDataStructure;
using XWSave;

namespace XWCore
{
    public class GameModelManager : IManager, IPreloader
    {
        public static GameModelManager Instance { get; } = new GameModelManager();
        private const string saveFileName = "save";

        public void Init()
        {
            SaveSystem.Init();
        }

        public void Load(Action<float> onProgress)
        {
            AppModel.InitGameModel(SaveSystem.LoadObject<AppModel>(saveFileName));
            onProgress.Invoke(1);
        }

        public void Save()
        {
            SaveSystem.SaveObject(saveFileName,AppModel.GetCurModel(),true);
        }
    }
}