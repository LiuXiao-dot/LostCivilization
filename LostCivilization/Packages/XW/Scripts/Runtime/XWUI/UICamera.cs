using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace XWUI
{
    /// <summary>
    /// UI相机
    /// </summary>
    public class UICamera : MonoBehaviour
    {
        public static UICamera Instance;

        private void Awake()
        {
            if(Instance != null) return;

            Instance = this;
            InitCamera();
        }

        private void InitCamera()
        {
            var camera = GetComponent<Camera>();
            var addData = camera.GetUniversalAdditionalCameraData();
            addData.renderType = CameraRenderType.Base;
            camera.orthographic = true;
            camera.orthographicSize = 54;
            addData.SetRenderer(90);
            camera.cullingMask = LayerMask.NameToLayer("UI");
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Color.gray;
        }
    }
}