using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using XWDataStructure;
using XWUtility;

namespace XWInput
{
    /// <summary>
    /// 要自定义按键，参考InputActionAsset.ToJson
    /// </summary>
    [XWFilePath("InputReader.asset", XWFilePathAttribute.PathType.InXW)]
    public class InputReader : ScriptableObjectSingleton<InputReader>, GameInput.IMenusActions,
        GameInput.IGameplayActions
    {
        private GameInput gameInput;

        public UnityAction<Vector2> onClick_GAME = delegate {  };

        protected override void OnEnable()
        {
            base.OnEnable();
            if (gameInput == null)
            {
                gameInput = new GameInput();
                gameInput.Menus.SetCallbacks(this);
                gameInput.Gameplay.SetCallbacks(this);
            }

            //EnableGameplayInput();
            EnableMenuInput();
        }

        #region 切换Input

        public void EnableGameplayInput()
        {
            DisableAllInput();

            gameInput.Gameplay.Enable();
        }
        
        public void EnableMenuInput()
        {
            DisableAllInput();

            gameInput.Menus.Enable();
        }

        public void DisableGameplayInput()
        {
            gameInput.Gameplay.Disable();
        }
        public void DisableAllInput()
        {
            gameInput.Gameplay.Disable();
            gameInput.Menus.Disable();
        }

        #endregion

        #region Menu
        
        public void OnNavigate(InputAction.CallbackContext context)
        {
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
        }

        public void OnClick(InputAction.CallbackContext context)
        {
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
        }

        #endregion

        #region Game
        public void OnOnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) {
                onClick_GAME.Invoke(UnityUtils.GetPointerPosition());
            }
        }
        public void OnAnyKey(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}