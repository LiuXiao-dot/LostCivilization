/// <summary>
/// @author TTM
/// @Modified 2022年03月18日 星期五 15:57
/// </summary>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using XWUtility;

namespace XWResource
{
    /// <summary>
    /// AddressablePool:
    /// 描述:从Addressables中加载的GameObject池
    /// </summary>
    public sealed class AddressablePool
    {
        /// <summary>
        /// GameObject池的源资源
        /// </summary>
        private GameObject _resource;

        /// <summary>
        /// GameObject池的Addressables引用
        /// </summary>
        private readonly AsyncOperationHandle<GameObject> _handle;

        /// <summary>
        /// 池中对象存放路径
        /// </summary>
        private readonly Transform _parent;

        public Action OnLoaded;
        
        /// <summary>
        /// 对象栈
        /// </summary>
        private readonly Stack<GameObject> _stack;

        private Transform _poolParent;

        /// <summary>
        /// 池的大小
        /// </summary>
        private int _maxSize;
        
        /// <summary>
        /// 池的大小
        /// </summary>
        internal int MAXSize
        {
            get => _maxSize;
            set
            {
                _maxSize = Math.Max(0, value);
            }
        }
        /// <summary>
        /// 基础大小
        /// </summary>
        private int _baseSize;

        /// <summary>
        /// 正在使用中的实例数量。此处未存储使用中的实例。
        /// </summary>
        private List<GameObject> _usedInstances;

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool isValid = false;

        public string path;

        
        public AddressablePool(string path,Transform parent= null,int maxSize = 5)
        {
            this.path = path;
            _poolParent = (new GameObject(path)).transform;

            MAXSize = maxSize;
            _usedInstances = new List<GameObject>();
            _stack = new Stack<GameObject>();
            _parent = parent;
            
            _handle = Addressables.LoadAssetAsync<GameObject>(path);
            _handle.Completed += OnSyncLoadComplete;
        }

        /// <summary>
        /// 通过Addressables引用创建池
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="parent">池中对象所在路径</param>
        /// <param name="maxSize">池的初始容量(资源加载完成后，会直接实例化maxSize个资源放在parent目录下)</param>
        public AddressablePool(AssetReferenceGameObject reference,string path,Transform parent= null,int maxSize = 5)
        {
            this.path = path;
            _poolParent = (new GameObject($"{reference.SubObjectName}")).transform;

            MAXSize = maxSize;
            _usedInstances = new List<GameObject>();
            _stack = new Stack<GameObject>();
            _parent = parent;
            _handle = reference.LoadAssetAsync<GameObject>();
            _handle.Completed += OnSyncLoadComplete;
        }
        
        /// <summary>
        /// 通过已加载耗的对象创建池
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="parent"></param>
        /// <param name="maxSize"></param>
        public AddressablePool(GameObject resource, string path, Transform parent= null,int maxSize = 5)
        {
            this.path = path;
            _poolParent = (new GameObject($"{resource.name}")).transform;

            MAXSize = maxSize;
            _usedInstances = new List<GameObject>();
            _stack = new Stack<GameObject>();
            _parent = parent;
            _resource = resource;
            InitPool();
        }

        private void OnSyncLoadComplete(AsyncOperationHandle<GameObject> obj)
        {
            _resource = obj.Result;
            
            // 初始化池
            InitPool();

            OnLoaded?.Invoke();
        }

        private void InitPool()
        {
            for (int i = 0; i < _maxSize; i++)
            {
                var obj = GameObject.Instantiate(_resource, _poolParent);
                var ctl = obj.GetOrAddComponent<AddressablePoolObject>();
                ctl.ResetObject();
                _stack.Push(obj);
            }
            isValid = true;
        }

        /// <summary>
        /// 获取当前加载进度
        /// </summary>
        /// <returns></returns>
        public float GetProgress()
        {
            return _resource != null ? 1 : (_handle.IsValid() ? _handle.PercentComplete:0);
        }

        /// <summary>
        /// 获取池的当前大小
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _stack.Count;
        }
        
        /// <summary>
        /// 从池中获取对象
        /// </summary>
        public GameObject Get(Transform parent = null)
        {
            if (!_handle.IsDone)
            {
                Debug.LogError(_handle.DebugName + "池未加载完成");
                return null;
            }

            if (Size() == 0)
            {
                ExpandPool();
            }

            var obj = _stack.Pop();
            var pool = obj.GetOrAddComponent<AddressablePoolObject>();
            pool.GetObject(this);
            if (parent != null)
            {
                obj.transform.SetParent(parent);
            }
            obj.transform.localScale = Vector3.one;
            _usedInstances.Add(obj);
            return obj;
        }

        /// <summary>
        /// 当子对象中有池对象时，子对象也将被Release.
        /// 若释放对象后，池中对象数量过多，会释放部分池对象。
        /// </summary>
        public void Release(GameObject obj)
        {
            var stackSize = Size();
            if (stackSize >= _maxSize - _baseSize && _maxSize > _baseSize) {
                FoldPool();
            }
            var pool = obj.GetComponent<AddressablePoolObject>();
            pool.ResetObject();
            _usedInstances.Remove(obj);
            _stack.Push(obj);
            obj.transform.SetParent(_poolParent);
        }

        /// <summary>
        /// 扩容数量：>1 & MaxSize / 2
        /// </summary>
        private void ExpandPool()
        {
            _maxSize += _baseSize;
            var createCount = Math.Max(_maxSize / 2,1);
            for (int i = 0; i < createCount; i++)
            {
                var obj = GameObject.Instantiate(_resource, _poolParent);
                var ctl = obj.GetOrAddComponent<AddressablePoolObject>();
                ctl.ResetObject();
                _stack.Push(obj);
            }
        }

        /// <summary>
        /// 减小容量
        /// </summary>
        private void FoldPool()
        {
            _maxSize -= _baseSize;
            // 释放_baseSize资源
            for (int i = 0; i < _baseSize; i++)
            {
                GameObject.Destroy(_stack.Pop());
            }
        }

        /// <summary>
        /// 销毁池,该方法会将通过池创建的所有对象也销毁
        /// </summary>
        public void Destroy()
        {
            this.isValid = false;
            var size = Size();
            for (int i = 0; i < size; i++)
            {
                var obj = _stack.Pop();
                GameObject.Destroy(obj);
            }

            var used = _usedInstances.Count;
            for (int i = 0; i < used; i++)
            {
                var obj = _usedInstances[i];
                GameObject.Destroy(obj);
            }
            _usedInstances.Clear();
            
            if (_handle.IsValid())
            {
                Addressables.Release(_handle);
            }

            if (_poolParent != null) {
                GameObject.Destroy(_poolParent.gameObject);
            }
        }
    }
}