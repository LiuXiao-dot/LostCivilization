/// <summary>
/// @author TTM
/// @Modified 2022年03月18日 星期五 15:39
/// </summary>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using XWDataStructure;
using XWUtility;

namespace XWResource
{
    /// <summary>
    /// AddressablesPoolManager:
    /// 描述:Addressables加载的资源池管理类
    /// </summary>
    public sealed class AddressablesPoolManager : IManager, IPreloader
    {
        private static AddressablesPoolManager instance = new AddressablesPoolManager();
        public static AddressablesPoolManager Instance => instance;
        private Dictionary<string, AddressablePool> _pools;

        private Transform _parent;

        private Dictionary<string, AsyncOperationHandle<ScriptableObject>> _scriptableObjects;

        /// <summary>
        /// gameObject的缓存
        /// </summary>
        private Dictionary<AssetReferenceGameObject, AsyncOperationHandle<GameObject>> _gameObjects;

        /// <summary>
        /// 开启加载检测后，<全部加载任务数量，已加载完成数量>
        /// </summary>
        public event UnityAction<int, int> onLoading = delegate { };

        /// <summary>
        /// 是否开启加载检测
        /// </summary>
        private bool _enableLoading;

        // 开启检测后加载的才会计数
        public int total;
        private int finished;

        /// <summary>
        /// 管理器初始化
        /// </summary>
        public void Init()
        {
            var parentObj = new GameObject("Pool");
            GameObject.DontDestroyOnLoad(parentObj);
            _parent = parentObj.transform;
            _pools = new Dictionary<string, AddressablePool>();
            _scriptableObjects = new Dictionary<string, AsyncOperationHandle<ScriptableObject>>();
            _gameObjects = new Dictionary<AssetReferenceGameObject, AsyncOperationHandle<GameObject>>();
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="onProgress"></param>
        public void Load(Action<float> onProgress)
        {
            var handle = Addressables.InitializeAsync(); // 提前初始化Addressables，防止未初始化导致的错误
            handle.Completed += operationHandle => { onProgress.Invoke(1); };
        }

        /// <summary>
        /// 同步加载实例化GameObject
        /// </summary>
        /// <returns></returns>
        public static GameObject InstantiateGameObjectSync(AssetReferenceGameObject reference, Transform parent)
        {
            if (reference.IsValid() && reference.IsDone) {
                return GameObject.Instantiate((GameObject)reference.OperationHandle.Result, parent);
            }

            var handle = reference.LoadAssetAsync<GameObject>();
            handle.WaitForCompletion();
            return GameObject.Instantiate(handle.Result, parent);
        }

        /// <summary>
        /// 仅加载并缓存下来，不实例化,通过reference释放,不提供单独回调
        /// </summary>
        public static void LoadGameObject(AssetReferenceGameObject reference, Action callback = null)
        {
            instance.CheckTotal();
            var handle = reference.LoadAssetAsync<GameObject>();
            handle.Completed += temp =>
            {
                callback?.Invoke();
                instance._gameObjects.Add(reference, temp);
                instance.CheckFinished();
            };
        }

        /// <summary>
        /// 同步加载缓存下来，不实例化，通过reference释放，不提供单独回调
        /// </summary>
        /// <returns>返回加载的prefab</returns>
        public static GameObject LoadGameObjectSync(string path)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            handle.WaitForCompletion();
            return handle.Result;
        }

        /// <summary>
        /// 实例化cache的GameObject
        /// </summary>
        public static GameObject InstantiateGameObject(AssetReferenceGameObject reference, Transform parent)
        {
            if (!instance._gameObjects.ContainsKey(reference)) {
                Debug.LogError($"需要先加载缓存才能实例化{reference.ToString()}");
                return null;
            }

            return GameObject.Instantiate(instance._gameObjects[reference].Result, parent);
        }

        /// <summary>
        /// 正常加载Prefab，若Prefab上有挂AddressablePoolObject，会放入池中
        /// </summary>
        /// <returns></returns>
        public static void InstantiateGameObject(string path, Transform parent, Action<GameObject> callback = null)
        {
            if (instance._pools.ContainsKey(path)) {
                var newObj = instance._pools[path].Get(parent);
                callback?.Invoke(newObj);
                return;
            }

            var handle = Addressables.LoadAssetAsync<GameObject>(path);
            instance.CheckTotal();
            handle.Completed += temp =>
            {
                var obj = temp.Result;
                var poolComponent = obj.GetComponent<AddressablePoolObject>();
                if (poolComponent == null) {
                    var unPoolComponent = obj.GetOrAddComponent<AddressablesUnPoolObject>();
                    unPoolComponent.handle = handle;
                    var newObj = GameObject.Instantiate(obj, parent);
                    callback?.Invoke(newObj);
                    instance.CheckFinished();
                    return;
                } else {
                    var pool = new AddressablePool(obj, path, instance._parent);
                    instance._pools.Add(path, pool);
                }

                var newObj2 = instance._pools[path].Get(parent);
                callback?.Invoke(newObj2);
                instance.CheckFinished();
            };
        }

        /// <summary>
        /// 若所加载Prefab无AddressablePoolObject组件，会自动添加一个，在池中取出时会显示出来，放回池中时会隐藏
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parent"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static void InstantiatePoolGameObject(string path, Transform parent, Action<GameObject> callback = null)
        {
            if (instance._pools.ContainsKey(path)) {
                var tempPool = instance._pools[path];
                if (tempPool.isValid) {
                    var temp = instance._pools[path].Get(parent);
                    callback?.Invoke(temp);
                } else {
                    tempPool.OnLoaded += () =>
                    {
                        var temp = instance._pools[path].Get(parent);
                        callback?.Invoke(temp);
                    };
                }
                return;
            }

            var pool = new AddressablePool(path, instance._parent);
            instance.CheckTotal();
            pool.OnLoaded = () =>
            {
                var temp = instance._pools[path].Get(parent);
                callback?.Invoke(temp);
                instance.CheckFinished();
            };
            instance._pools.Add(path, pool);
        }


        /// <summary>
        /// 正常加载Prefab，若Prefab上有挂AddressablePoolObject，会放入池中
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void InstantiateGameObject(AssetReferenceGameObject reference, Transform parent,
            Action<GameObject> callback)
        {
            var guid = reference.AssetGUID;
            if (instance._pools.ContainsKey(guid)) {
                callback(instance._pools[guid].Get(parent));
                return;
            }

            instance.CheckTotal();
            var handle = reference.LoadAssetAsync<GameObject>();
            handle.Completed += temp =>
            {
                var obj = temp.Result;
                var poolComponent = obj.GetComponent<AddressablePoolObject>();
                if (poolComponent == null) {
                    var unPoolComponent = obj.GetOrAddComponent<AddressablesUnPoolObject>();
                    unPoolComponent.handle = handle;
                    var instance = GameObject.Instantiate(obj, parent);
                    callback?.Invoke(instance);
                    AddressablesPoolManager.instance.CheckFinished();
                    return;
                } else {
                    var pool = new AddressablePool(obj, guid, instance._parent);
                    instance._pools.Add(guid, pool);
                }

                callback(instance._pools[guid].Get(parent));
                instance.CheckFinished();
            };
        }

        /// <summary>
        /// 若所加载Prefab无AddressablePoolObject组件，会自动添加一个，在池中取出时会显示出来，放回池中时会隐藏
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void InstantiatePoolGameObject(AssetReferenceGameObject reference, Transform parent,
            Action<GameObject> callback)
        {
            var guid = reference.AssetGUID;
            if (instance._pools.ContainsKey(guid)) {
                var tempPool = instance._pools[guid];
                if (!reference.IsDone) {
                    tempPool.OnLoaded += () => { callback(tempPool.Get(parent)); };
                } else {
                    var instance = tempPool.Get(parent);
                    callback?.Invoke(instance);
                }

                return;
            }

            instance.CheckTotal();
            var pool = new AddressablePool(reference, guid, instance._parent);
            pool.OnLoaded = () =>
            {
                callback(instance._pools[guid].Get(parent));
                instance.CheckFinished();
            };
            instance._pools.Add(guid, pool);
        }

        /// <summary>
        /// 销毁GameObject,如果是池对象，会回池
        /// </summary>
        public static void DestroyGameObject(GameObject obj)
        {
            if (obj.TryGetComponent<AddressablePoolObject>(out var poolObj)) {
                poolObj.Destroy();
            } else {
                GameObject.Destroy(obj);
            }
        }

        /// <summary>
        /// 慎用,相关obj都会销毁
        /// </summary>
        /// <param name="obj"></param>
        public static void DestroyPool(GameObject obj)
        {
            if (obj.TryGetComponent<AddressablePoolObject>(out var poolObj)) {
                var pool = poolObj.GetPool();
                if (pool.isValid) {
                    pool.Destroy();
                    instance._pools.Remove(pool.path);
                }
            } else {
                GameObject.Destroy(obj);
            }
        }

        public static void LoadScriptableObject<T>(string path, Action<T> callback = null) where T : ScriptableObject
        {
            var _scriptableObjects = instance._scriptableObjects;
            if (_scriptableObjects.ContainsKey(path)) {
                if (_scriptableObjects[path].IsDone) {
                    if (_scriptableObjects[path].IsValid()) {
                        callback?.Invoke(_scriptableObjects[path].Result as T);
                    } else {
                        _scriptableObjects.Remove(path);
                    }
                } else {
                    _scriptableObjects[path].Completed += temp => { callback?.Invoke(temp.Result as T); };
                }

                return;
            }

            var handle = Addressables.LoadAssetAsync<ScriptableObject>(path);
            instance.CheckTotal();
            handle.Completed += temp =>
            {
                callback?.Invoke(temp.Result as T);
                _scriptableObjects.Add(path, temp);
                instance.CheckFinished();
            };
        }

        /// <summary>
        /// SO在内存中只有一份，删除采用直接销毁handle的方式,被复制的ScriptableObject需要自行删除
        /// </summary>
        /// <param name="path"></param>
        public static void DestroyScriptableObject(string path)
        {
            var _scriptableObjects = instance._scriptableObjects;
            if (_scriptableObjects.ContainsKey(path)) {
                if (_scriptableObjects[path].IsDone) {
                    Addressables.Release(_scriptableObjects[path]);
                    _scriptableObjects.Remove(path);
                } else {
                    _scriptableObjects[path].Completed += temp =>
                    {
                        Addressables.Release(temp);
                        _scriptableObjects.Remove(path);
                    };
                }
            }
        }

        /// <summary>
        /// 打开场景
        /// </summary>
        /// <param name="sceneName"></param>
        public static void OpenScene(string sceneName, Action callback = null)
        {
            var handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            instance.CheckTotal();
            handle.Completed += operationHandle =>
            {
                var scene = operationHandle.Result.Scene;
                if (!scene.isLoaded) {
                    //SceneManager.SetActiveScene(scene);
                }

                callback?.Invoke();
                instance.CheckFinished();
            };
        }

        /// <summary>
        /// 移除场景
        /// </summary>
        public static void RemoveScene()
        {

        }

        private static void SetLoadingEnabled(bool enableLoading)
        {
            if (instance._enableLoading == enableLoading) return;
            instance._enableLoading = enableLoading;
            instance.total = 0;
            instance.finished = 0;
        }

        private void CheckTotal()
        {
            if (_enableLoading) {
                total++;
            }
        }

        private void CheckFinished()
        {
            if (_enableLoading) {
                finished++;
                onLoading.Invoke(total, finished);
                if (total == finished)
                    SetLoadingEnabled(false);
            }
        }

        /// <summary>
        /// 开启加载记录
        /// </summary>
        public static void StartLoadingRecord()
        {
            SetLoadingEnabled(true);
            instance.CheckTotal();
        }

        /// <summary>
        /// 停止加载记录(并不会停止加载，只是把欠缺的一次加载补上)
        /// </summary>
        public static void StopLoadingRecord()
        {
            instance.CheckFinished();
        }
    }
}