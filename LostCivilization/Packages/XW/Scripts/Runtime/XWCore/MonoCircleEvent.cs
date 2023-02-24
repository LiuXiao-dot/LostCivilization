using System;
using UnityEngine;
using UnityEngine.Events;

namespace XWCore
{
    /// <summary>
    /// monoBehaviour的生命周期事件
    /// </summary>
    public class MonoCircleEvent : MonoBehaviour
    {
        public UnityEvent onAwake;
        public UnityEvent<Transform> onAwakeTrans;
        public UnityEvent onStart;
        public UnityEvent onEnable;
        public UnityEvent onDisable;
        
        private void Awake()
        {
            onAwake?.Invoke();
            onAwakeTrans?.Invoke(transform);
        }

        private void Start()
        {
            onStart?.Invoke();
        }

        private void OnEnable()
        {
            onEnable?.Invoke();
        }

        private void OnDisable()
        {
            onDisable?.Invoke();
        }
    }
}