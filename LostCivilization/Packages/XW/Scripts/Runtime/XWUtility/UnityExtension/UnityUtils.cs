// 需要新的Unity InputSystem
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using XWUtility;
using Pointer = UnityEngine.InputSystem.Pointer;

namespace XWUtility
{
    /// <summary>
    /// unity utils
    /// </summary>
    public class UnityUtils
    {
        /// <summary>
        /// 获取当前鼠标/touch点坐标
        /// </summary>
        /// <returns></returns>
        public static Pointer GetPointer()
        {
            if(Mouse.current != null)
            {
                return Mouse.current;
            }

            if(Touchscreen.current != null)
            {
                return Touchscreen.current;
            }

            XWLogger.Error("未支持pointer输入");
            return null;
        }

        public static Vector2 GetPointerPosition()
        {
            return GetPointer().position.ReadValue();
        }

        /// <summary>
        /// 鼠标是否经过某个UI
        /// </summary>
        /// <returns></returns>
        public static bool IsPointerOverUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            else
            {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position = GetPointerPosition();
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pe, hits);
                return hits.Count > 0;
            }
        }


        public static Vector3 GetMouseWorldPosition(Camera camera, Vector2 position, LayerMask layer)
        {
            Ray ray = camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layer.value))
            {
                return raycastHit.point;
            }
            else
            {
                return Vector3.zero;
            }
        }

        public static bool GetMouseWorldPosition(Camera camera, Vector2 position, LayerMask layer, out Vector3 worldPosition)
        {
            Ray ray = camera.ScreenPointToRay(position);
            if (UnityUtils.IsPointerOverUI())
            {
                worldPosition = Vector3.zero;
                return false;
            }

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, layer.value))
            {
                worldPosition = raycastHit.point;
                return true;
            }
            else
            {
                worldPosition = Vector3.zero;
                return false;
            }
        }
    }
}
