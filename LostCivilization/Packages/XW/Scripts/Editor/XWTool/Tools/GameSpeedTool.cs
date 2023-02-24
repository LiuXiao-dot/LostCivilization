using Sirenix.OdinInspector;
using UnityEngine;
namespace XWTool
{
    [Tool( "调试/游戏速度")]
    public class GameSpeedTool
    {
        [Range(0,99)]
        public float speed;

        [Button]
        public void Set()
        {
            Time.timeScale = speed;
        }
    }
}