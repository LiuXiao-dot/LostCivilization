using Sirenix.OdinInspector;
using UnityEngine;
namespace LostCivilization.Test
{
    public class QuaterionCalculator : MonoBehaviour
    {
        public float degree;
        public Quaternion quaternion;
        public float rotateDegree;

        public Vector3 vector;

        [Button]
        private void CalculateQuaternion()
        {
            quaternion = Quaternion.AngleAxis(degree,Vector3.up);
        }
        
        [Button]
        private void Rotate()
        {
            quaternion *= Quaternion.AngleAxis(rotateDegree, Vector3.up);
        }

        [Button]
        private void CalculateQuaByVector()
        {
            quaternion = Quaternion.LookRotation(vector);
        }
    }
}