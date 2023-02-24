using XWCore;
using XWSave;

namespace LostCivilization
{
    public sealed class LCGameManager : GameManager
    {
        protected override void InitModels()
        {
            base.InitModels();
            PlayerPrefsUtils.Get(GameConfig.Instance);
            //WorldManager.Instance.Init(); // 需要等GameScene加载完
        }

        protected override void OnDestroy()
        {
            PlayerPrefsUtils.Set(GameConfig.Instance);
        }
    }
}