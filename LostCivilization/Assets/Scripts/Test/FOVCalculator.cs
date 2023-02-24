#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using XWUtility;

public class FOVCalculator : MonoBehaviour
{
    public float fieldOfView;
    public float frustumHeight;
    public float distance;
    
    [Button]
    public void GetFrustumHeight()
    {
        frustumHeight = CameraUtils.GetFrustumHeight(distance,fieldOfView);
    }

    [Button]
    public void GetDistance()
    {
        distance = CameraUtils.GetDistance(frustumHeight,fieldOfView);
    }
    
    [Button]
    public void GetFOV()
    {
        fieldOfView = CameraUtils.GetFov(frustumHeight,distance);
    }

    public void GetMax()
    {
        
    }
}
#endif
