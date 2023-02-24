using TMPro;
using UnityEngine;
using XWResource;
using XWUI;
using XWUtility;
namespace GameUI
{
    public class PopupMessageWindowCtl : AWindowCtl
    {
        private int count;
        protected override void OnOpen()
        {
            var view = (PopupMessageWindowView)_view;
            AddressablesPoolManager.InstantiateGameObject("TXT_message.prefab",view.transform, result =>
            {
                var messageTXT = result.GetComponent<TextMeshProUGUI>();
                messageTXT.text = _values[0].ToString();
                var trans = result.transform;
                count++;
                var i = 0;
                CoroutineManager.Instance.AddCoroutineFrame(() =>
                {
                    i++;
                    trans.position += new Vector3(0, 20, 0) * Time.deltaTime;
                    if (i == 60) {
                        count--;
                        AddressablesPoolManager.DestroyGameObject(result);
                        if(count == 0)
                            WindowManager.CloseWindow("PopupMessageWindow");
                    }
                }, 60, count * 10);
            });
        }

        protected override void OnPause()
        {
        }

        protected override void OnResume()
        {
        }

        public override void ReOpen()
        {
            OnOpen();
        }

        protected override void OnClose()
        {
        }

        public static void ShowMessage(string message)
        {
            WindowManager.OpenWindow("PopupMessageWindow", message);
        }
    }
}