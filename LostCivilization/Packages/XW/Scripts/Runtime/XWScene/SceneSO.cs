using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using XWUtility;

namespace XWScene{
    public class SceneSO : ScriptableObject
    {
        [SerializeField] public AssetReference sceneReference;
        public bool showLoading;
        private AsyncOperationHandle<SceneInstance> handler;

        public void Enter()
        {
            SceneMessageQueue.SendGameEventImmediateS(SceneMessage.LoadScene, showLoading);
            if (handler.IsValid())
            {
                SceneManager.SetActiveScene(handler.Result.Scene);
                OnEnter(handler);
            }
            else
            {
                CoroutineManager.Instance.AddCoroutine(LoadScene());
            }
        }

        private IEnumerator LoadScene()
        {
            handler = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            handler.Completed += OnEnter;
            if (showLoading)
            {
                CoroutineManager.Instance.AddCoroutineEndFrame(OnLoading, -1);
            }

            yield return handler;
        }

        private void OnLoading()
        {
            SceneMessageQueue.SendGameEventS(SceneMessage.OnSceneLoading, handler.PercentComplete);
        }

        private void OnEnter(AsyncOperationHandle<SceneInstance> obj)
        {
            if (showLoading)
            {
                CoroutineManager.Instance.StopCoroutine(OnLoading);
            }

            SceneMessageQueue.SendGameEventS(SceneMessage.OnSceneLoading, 1f);
            SceneMessageQueue.SendGameEventS(SceneMessage.OnSceneLoaded, this);
        }

        public virtual void Exit()
        {
            if (handler.IsValid())
            {
                sceneReference.UnLoadScene();
            }

            SceneMessageQueue.SendGameEventS(SceneMessage.OnSceneUnLoaded);
        }
    }
}