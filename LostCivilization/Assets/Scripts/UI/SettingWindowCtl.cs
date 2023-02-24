
using XWUI;
namespace GameUI
{
    public class SettingWindowCtl : AWindowCtl
    {
        protected override void OnOpen()
        {
            var view = (SettingWindowView)_view;
            view._backBTN.onClick.AddListener(OnClickBack);
        }
        private void OnClickBack()
        {
            WindowManager.CloseWindow("SettingWindow");
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