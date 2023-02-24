using XWUI;
namespace GameUI
{
    public class MainWindowCtl : AWindowCtl
    {
        protected override void OnOpen()
        {
            var view = (this._view as MainWindowView);
            view._startBTN.onClick.AddListener(OnClickStart);
            view._settingBTN.onClick.AddListener(OnClickSetting);
            PopupMessageWindowCtl.ShowMessage("进入游戏成功");
            PopupMessageWindowCtl.ShowMessage("进入游戏成功");
            PopupMessageWindowCtl.ShowMessage("进入游戏成功");
        }

        private void OnClickSetting()
        {
            WindowManager.OpenWindow("SettingWindow");
        }

        private void OnClickStart()
        {
            // 调用世界管理器，进入标准模式世界
            //WorldManager.Instance.EnterWorld("Standard");
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