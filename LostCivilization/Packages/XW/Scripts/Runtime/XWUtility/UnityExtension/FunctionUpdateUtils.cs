using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWUtility
{
    /// <summary>
    /// 每帧执行Function
    /// </summary>
    public class FunctionUpdateUtils
    {
        private class MonoBehaviourHook : MonoBehaviour
        {
            public Action OnUpdate;

            private void Update()
            {
                OnUpdate?.Invoke();
            }

        }


        private static List<FunctionUpdateUtils> updaterList; // Holds a reference to all active updaters
        private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change

        private static void InitIfNeeded()
        {
            if (initGameObject == null)
            {
                initGameObject = new GameObject("FunctionUpdater_Global");
                updaterList = new List<FunctionUpdateUtils>();
            }
        }




        public static FunctionUpdateUtils Create(Action updateFunc)
        {
            return Create(() => { updateFunc(); return false; }, "", true, false);
        }

        public static FunctionUpdateUtils Create(Action updateFunc, string functionName)
        {
            return Create(() => { updateFunc(); return false; }, functionName, true, false);
        }

        public static FunctionUpdateUtils Create(Func<bool> updateFunc)
        {
            return Create(updateFunc, "", true, false);
        }

        public static FunctionUpdateUtils Create(Func<bool> updateFunc, string functionName)
        {
            return Create(updateFunc, functionName, true, false);
        }

        public static FunctionUpdateUtils Create(Func<bool> updateFunc, string functionName, bool active)
        {
            return Create(updateFunc, functionName, active, false);
        }

        public static FunctionUpdateUtils Create(Func<bool> updateFunc, string functionName, bool active, bool stopAllWithSameName)
        {
            InitIfNeeded();

            if (stopAllWithSameName)
            {
                StopAllUpdatersWithName(functionName);
            }

            GameObject gameObject = new GameObject("FunctionUpdater Object " + functionName, typeof(MonoBehaviourHook));
            FunctionUpdateUtils functionUpdater = new FunctionUpdateUtils(gameObject, updateFunc, functionName, active);
            gameObject.GetComponent<MonoBehaviourHook>().OnUpdate = functionUpdater.Update;

            updaterList.Add(functionUpdater);
            return functionUpdater;
        }

        private static void RemoveUpdater(FunctionUpdateUtils funcUpdater)
        {
            InitIfNeeded();
            updaterList.Remove(funcUpdater);
        }

        public static void DestroyUpdater(FunctionUpdateUtils funcUpdater)
        {
            InitIfNeeded();
            if (funcUpdater != null)
            {
                funcUpdater.DestroySelf();
            }
        }

        public static void StopUpdaterWithName(string functionName)
        {
            InitIfNeeded();
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName == functionName)
                {
                    updaterList[i].DestroySelf();
                    return;
                }
            }
        }

        public static void StopAllUpdatersWithName(string functionName)
        {
            InitIfNeeded();
            for (int i = 0; i < updaterList.Count; i++)
            {
                if (updaterList[i].functionName == functionName)
                {
                    updaterList[i].DestroySelf();
                    i--;
                }
            }
        }

        private GameObject gameObject;
        private string functionName;
        private bool active;
        private Func<bool> updateFunc; // Destroy Updater if return true;

        public FunctionUpdateUtils(GameObject gameObject, Func<bool> updateFunc, string functionName, bool active)
        {
            this.gameObject = gameObject;
            this.updateFunc = updateFunc;
            this.functionName = functionName;
            this.active = active;
        }

        public void Pause()
        {
            active = false;
        }

        public void Resume()
        {
            active = true;
        }

        private void Update()
        {
            if (!active) return;
            if (updateFunc())
            {
                DestroySelf();
            }
        }

        public void DestroySelf()
        {
            RemoveUpdater(this);
            if (gameObject != null)
            {
                UnityEngine.Object.Destroy(gameObject);
            }
        }
    }

}
