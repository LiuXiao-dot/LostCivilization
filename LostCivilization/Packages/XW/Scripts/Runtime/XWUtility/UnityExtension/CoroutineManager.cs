using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWUtility
{
    /// <summary>
    /// TODO:协程管理
    /// </summary>
    public class CoroutineManager : MonoBehaviour
    {
        public static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var newObj = new GameObject("CoroutineManager");
                    _instance = newObj.AddComponent<CoroutineManager>();
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
            set
            {
                if(_instance == value)
                    return;
                if(_instance != null)
                {
                    Destroy(value);
                    return;
                }
                _instance = value;
                DontDestroyOnLoad(_instance);
            }
        }

        private static CoroutineManager _instance;

        private Dictionary<int, IEnumerator> coroutines = new Dictionary<int, IEnumerator>();

        private void Awake()
        {
            Instance = this;
        }

        public void AddCoroutine(IEnumerator enumrator)
        {
            StartCoroutine(enumrator);
        }
        
        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        public void AddCoroutineWaitTime(Action action,int loopTime,float interval,float delayTime = 0)
        {
            var routine = ActionCoroutineWaitTime(action, loopTime,interval,delayTime);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            StartCoroutine(routine);
        }

        private IEnumerator ActionCoroutineWaitTime(Action action,int loopTime,float interval,float delatTime)
        {
            yield return new WaitForSeconds(delatTime);
            var inter = new WaitForSeconds(interval);
            while (loopTime != 0)
            {
                action();
                yield return inter;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }


        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        public void AddCoroutineTime(Action action,int loopTime,float delayTime = 0)
        {
            var routine = ActionCoroutineTime(action, loopTime,delayTime);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            StartCoroutine(routine);
        }

        private IEnumerator ActionCoroutineTime(Action action,int loopTime,float delatTime)
        {
            yield return new WaitForSeconds(delatTime);
            while (loopTime != 0)
            {
                action();
                yield return null;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }

        
        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        public void AddCoroutineFrame(Action action,int loopTime,int delayFrame = 0)
        {
            var routine = ActinoCoroutineFrame(action, loopTime,delayFrame);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            StartCoroutine(routine);
        }

        private IEnumerator ActinoCoroutineFrame(Action action,int loopTime,int delayFrame)
        {
            for (int i = 0; i < delayFrame; i++)
            {
                yield return null;
            }
            while (loopTime != 0)
            {
                action();
                yield return null;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }
        
                
        /// <summary>
        /// 每帧执行一次，执行loopTime次，loopTime小于0时为循环
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loopTime"></param>
        public void AddCoroutineEndFrame(Action action,int loopTime,int delayFrame = 0)
        {
            var routine = ActinoCoroutineEndFrame(action, loopTime,delayFrame);
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                Debug.LogWarning("暂不支持同一个action同时在多个协程中执行");
                return;
            }
            coroutines.Add(action.GetHashCode(),routine);
            StartCoroutine(routine);
        }

        private IEnumerator ActinoCoroutineEndFrame(Action action,int loopTime,int delayFrame)
        {
            var wait = new WaitForEndOfFrame();
            for (int i = 0; i < delayFrame; i++)
            {
                yield return null;
            }
            while (loopTime != 0)
            {
                action();
                yield return wait;
                loopTime--;
            }

            coroutines.Remove(action.GetHashCode());
        }
        
        public void StopCoroutine(Action action)
        {
            var hashCode = action.GetHashCode();
            if (coroutines.ContainsKey(hashCode))
            {
                StopCoroutine(coroutines[hashCode]);
                coroutines.Remove(hashCode);
            }
        }
    }
}
