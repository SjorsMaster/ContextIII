using Oculus.Interaction;
using PortalsVR;
using SharedSpaces.Managers;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SharedSpaces.Menus
{
    public class CustomEditAnchorsMenu : Menu
    {
        [SerializeField] private GameObject anchorPrefab;

        [SerializeField] private GameObject LeftHandCanvas;
        [SerializeField] private GameObject RightHandCanvas;

        [SerializeField] private UnityEvent OnRPrimaryPress;
        [SerializeField] private UnityEvent OnRSecondaryPress;
        [SerializeField] private UnityEvent OnLPrimaryPress;
        [SerializeField] private UnityEvent OnLSecondaryPress;

        [SerializeField] private RayInteractable tableInteractable;

        private int worldIndex = 0;
        private int maxWorlds = 0;

        #region Event Handlers
        private void SpatialAnchorManager_OnDebugMessage(string message)
        {
            VRDebugPanel.Instance.SendDebugMessage(message);
        }

        private void InputManager_StartRelease()
        {
            MenuManager.ShowMenu<MainMenu>();
        }

        private void InputManager_RPrimaryPress()
        {
            OnRPrimaryPress.Invoke();
        }

        private void InputManager_RSecondaryPress()
        {
            OnRSecondaryPress.Invoke();
        }

        private void InputManager_LPrimaryPress()
        {
            OnLPrimaryPress.Invoke();
        }

        private void InputManager_LSecondaryPress()
        {
            OnLSecondaryPress.Invoke();
        }
        #endregion

        private void Awake()
        {
            MenuManager.RegisterMenu(this);
        }

        private void OnDestroy()
        {
            MenuManager.UnregisterMenu(this);
        }

        public override void OnShow()
        {
            base.OnShow();

            ToggleEventHandlers(true);
            ToggleHandCanvases(true);

            tableInteractable.enabled = true;
        }

        public override void OnHide()
        {
            base.OnHide();

            ToggleEventHandlers(false);
            ToggleHandCanvases(false);

            tableInteractable.enabled = false;
        }

        private void ToggleEventHandlers(bool enable)
        {
            if (enable)
            {
                InputManager.StartRelease += InputManager_StartRelease;
                InputManager.RPrimaryPress += InputManager_RPrimaryPress;
                InputManager.RSecondaryPress += InputManager_RSecondaryPress;
                InputManager.LPrimaryPress += InputManager_LPrimaryPress;
                InputManager.LSecondaryPress += InputManager_LSecondaryPress;
            }
            else
            {
                InputManager.StartRelease -= InputManager_StartRelease;
                InputManager.RPrimaryPress -= InputManager_RPrimaryPress;
                InputManager.RSecondaryPress -= InputManager_RSecondaryPress;
                InputManager.LPrimaryPress -= InputManager_LPrimaryPress;
                InputManager.LSecondaryPress -= InputManager_LSecondaryPress;
            }
        }

        private void ToggleHandCanvases(bool value)
        {
            LeftHandCanvas.SetActive(value);
            RightHandCanvas.SetActive(value);
        }

        public async void PlaceWorldAnchor()
        {
            WorldsAnchorManager.Instance.PlaceAnchorInWorld("Global");
            foreach (var world in World.worlds)
            {
                while (WorldsAnchorManager.Instance.IsBusy)
                {
                    await Task.Yield();
                }
                WorldsAnchorManager.Instance.PlaceAnchorInWorld(world.Key);
            }
        }

        public void OpenThisMenu()
        {
            MenuManager.ShowMenu<CustomEditAnchorsMenu>();
        }
    }
}
