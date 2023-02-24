using XWUI;
namespace GameUI
{
    public class GameWindowCtl : AWindowCtl
    {
        protected override void OnOpen()
        {
            var view = (this._view as GameWindowView);
            view._exitBTN.onClick.AddListener(OnClickExit);
        }

        private void OnClickExit()
        {
            //WorldManager.Instance.ExitWorld();
            WindowManager.CloseWindow("GameWindow");
        }

        protected override void OnPause()
        {
        }
        
        protected override void OnResume()
        {
        }
        
        protected override void OnClose()
        {
        }
    }
}