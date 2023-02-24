using Sirenix.OdinInspector;
using UnityEngine;
namespace LostCivilization.Test
{
    [ExecuteInEditMode]
    public class DegreeCalculate : MonoBehaviour
    {
        public int wave;
        public int value;
        public bool showGraph;

        [LabelText("增大趋势")]
        public float a = 1;
        [LabelText("波的频率")]
        public int b = 9;
        [LabelText("波的大小")]
        public float c = 1;
        /// <summary>
        /// 计算
        /// </summary>
        [Button]
        public void Calculate()
        {
            value = Calculate(wave);
        }

        /*private void OnGUI()
        {
            if(!showGraph) return;
            Handles.color = Color.red;
            var oldV = 0;
            var newV = 0;
            for (int i = 0; i < 50; i++) {
                newV = Calculate(i);
                //Handles.DrawLine(new Vector3(200 + i-1,960 - oldV),new Vector3(200 + i,960 - newV));
                Handles.DrawLine(new Vector3(200 + i-1,960 - oldV),new Vector3(200 + i,960 - newV));
                oldV = newV;
            }
        }*/

        private void OnDrawGizmosSelected()
        {
            if(!showGraph) return;
            Gizmos.color = Color.red;
            var oldV = 0;
            var newV = 0;
            for (int i = 1; i < 200; i++) {
                newV = Calculate(i);
                //Handles.DrawLine(new Vector3(200 + i-1,960 - oldV),new Vector3(200 + i,960 - newV));
                //Gizmos.DrawLine(new Vector3(i-1, oldV),new Vector3(i, newV));
                Gizmos.DrawCube(new Vector3(i-1, oldV),new Vector3(1f, 1f));
                oldV = newV;
            }
        }

        private int Calculate(int wave)
        {
            var tempa = a;
            var tempb = b == 0 ? 1: b;
            var tempc = c;
            var value = wave; // 递增
            value = Mathf.FloorToInt(Mathf.Sqrt(wave * tempa)); // 减缓wave较大时的增长趋势
            // 前期峰谷少,后期多,前期峰谷小,后期高
            //tempb = (1 / tempb) + Mathf.Sqrt(tempb);
            tempb = tempb / (int)Mathf.Sqrt(value) + 1;
            tempc = (1 + tempc) * tempc;
            var rectWave = (wave % tempb) == 0 ? ((wave / tempb)%2 == 0 ? 1 : -1) * tempc + value: value; // 方波,添加波峰和波谷
            
            return (int)rectWave + value;
        }
    }
}