using System;
using XWDataStructure;
using XWResource;

namespace XWInput
{
    public class InputManager : IManager, IPreloader
    {
        public static InputManager Instance = new InputManager();
        private InputReader inputReader;

        public static InputReader GetInputReader()
        {
            return Instance.inputReader;
        }

        public void Init()
        {
        }

        public void Load(Action<float> onProgress)
        {
            AddressablesPoolManager.LoadScriptableObject<InputReader>("InputReader.asset", so =>
            {
                inputReader = so;
                onProgress.Invoke(1);
            });
        }
    }
}