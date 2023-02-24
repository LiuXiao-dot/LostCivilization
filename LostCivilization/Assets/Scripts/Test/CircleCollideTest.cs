#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;
using XWDataStructure;

public class CircleCollideTest : MonoBehaviour
{
    public Circle2D shape1;
    public Circle2D shape2;
    [Tooltip("shape1的受力方向")]
    public Vector2 normal;
    
    [Button]
    public void CheckCollider()
    {
        normal = shape1.GetNormalize(shape2);
    }

    private void OnDrawGizmosSelected()
    {
        if(shape1 == null) return;
        if (shape2 == null) return;
        Gizmos.DrawWireSphere(shape1.center,shape1.radius);
        Gizmos.DrawWireSphere(shape2.center,shape2.radius);
    }
}
#endif