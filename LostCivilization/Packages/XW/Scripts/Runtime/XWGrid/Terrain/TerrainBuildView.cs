using UnityEngine;
using XWInput;
using XWUtility;
namespace XWGrid.Terrain
{
    /// <summary>
    /// 地形构建器
    /// </summary>
    public class TerrainBuildView : MonoBehaviour
    {
        private InputReader _inputReader;
        private LayerMask ground = LayerMask.GetMask("TerrainGround");

        private void Awake()
        {
            _inputReader = InputReader.Instance;
            _inputReader.onClick_GAME += Enable;
        }
        private void Enable(Vector2 pos)
        {
            if (UnityUtils.GetMouseWorldPosition(Camera.main, pos, ground, out var clickPosition)) {
                // 点击到了地面
                
            }
        }
    }
}