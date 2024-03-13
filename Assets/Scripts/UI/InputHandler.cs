using UnityEngine;
using UnityEngine.InputSystem;

namespace ContextIII
{
    public class InputHandler : Singleton<InputHandler>
    {
        //private void OnLPrimary2DAxis(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LPrimary2DAxis: " + context.ReadValue<Vector2>());
        //}

        //private void OnRPrimary2DAxis(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RPrimary2DAxis: " + context.ReadValue<Vector2>());
        //}

        //private void OnLTrigger(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LTrigger: " + context.ReadValue<float>());
        //}

        //private void OnRTrigger(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RTrigger: " + context.ReadValue<float>());
        //}

        //private void OnLGrip(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LGrip: " + context.ReadValue<float>());
        //}

        //private void OnRGrip(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RGrip: " + context.ReadValue<float>());
        //}

        //private void OnLPrimaryButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LPrimaryButton: " + context.ReadValueAsButton());
        //}

        //private void OnRPrimaryButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RPrimaryButton: " + context.ReadValueAsButton());
        //}

        //private void OnLPrimaryTouch(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LPrimaryTouch: " + context.ReadValueAsButton());
        //}

        //private void OnRPrimaryTouch(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RPrimaryTouch: " + context.ReadValueAsButton());
        //}

        //private void OnLSecondaryButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LSecondaryButton: " + context.ReadValueAsButton());
        //}

        //private void OnRSecondaryButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RSecondaryButton: " + context.ReadValueAsButton());
        //}

        //private void OnLSecondaryTouch(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LSecondaryTouch: " + context.ReadValueAsButton());
        //}

        //private void OnRSecondaryTouch(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RSecondaryTouch: " + context.ReadValueAsButton());
        //}

        //private void OnLGripButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LGripButton: " + context.ReadValueAsButton());
        //}

        //private void OnRGripButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RGripButton: " + context.ReadValueAsButton());
        //}

        //private void OnLTriggerButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LTriggerButton: " + context.ReadValueAsButton());
        //}

        //private void OnRTriggerButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RTriggerButton: " + context.ReadValueAsButton());
        //}

        //private void OnLPrimary2DAxisClick(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LPrimary2DAxisClick: " + context.ReadValueAsButton());
        //}

        //private void OnRPrimary2DAxisClick(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RPrimary2DAxisClick: " + context.ReadValueAsButton());
        //}

        //private void OnLPrimary2DAxisTouch(InputAction.CallbackContext context)
        //{
        //    Debug.Log("LPrimary2DAxisTouch: " + context.ReadValueAsButton());
        //}

        //private void OnRPrimary2DAxisTouch(InputAction.CallbackContext context)
        //{
        //    Debug.Log("RPrimary2DAxisTouch: " + context.ReadValueAsButton());
        //}

        //private void OnMenuButton(InputAction.CallbackContext context)
        //{
        //    Debug.Log("MenuButton: " + context.ReadValueAsButton());
        //}
        private void OnLPrimary2DAxis(InputValue value)
        {
            Debug.Log("LPrimary2DAxis: " + value.Get<Vector2>());
        }

        private void OnRPrimary2DAxis(InputValue value)
        {
            Debug.Log("RPrimary2DAxis: " + value.Get<Vector2>());
        }

        private void OnLTrigger(InputValue value)
        {
            Debug.Log("LTrigger: " + value.Get<float>());
        }

        private void OnRTrigger(InputValue value)
        {
            Debug.Log("RTrigger: " + value.Get<float>());
        }

        private void OnLGrip(InputValue value)
        {
            Debug.Log("LGrip: " + value.Get<float>());
        }

        private void OnRGrip(InputValue value)
        {
            Debug.Log("RGrip: " + value.Get<float>());
        }

        private void OnLPrimaryButton(InputValue value)
        {
            Debug.Log("LPrimaryButton: " + value.isPressed);
        }

        private void OnRPrimaryButton(InputValue value)
        {
            Debug.Log("RPrimaryButton: " + value.isPressed);
        }

        private void OnLPrimaryTouch(InputValue value)
        {
            Debug.Log("LPrimaryTouch: " + value.isPressed);
        }

        private void OnRPrimaryTouch(InputValue value)
        {
            Debug.Log("RPrimaryTouch: " + value.isPressed);
        }

        private void OnLSecondaryButton(InputValue value)
        {
            Debug.Log("LSecondaryButton: " + value.isPressed);
        }

        private void OnRSecondaryButton(InputValue value)
        {
            Debug.Log("RSecondaryButton: " + value.isPressed);
        }

        private void OnLSecondaryTouch(InputValue value)
        {
            Debug.Log("LSecondaryTouch: " + value.isPressed);
        }

        private void OnRSecondaryTouch(InputValue value)
        {
            Debug.Log("RSecondaryTouch: " + value.isPressed);
        }

        private void OnLGripButton(InputValue value)
        {
            Debug.Log("LGripButton: " + value.isPressed);
        }

        private void OnRGripButton(InputValue value)
        {
            Debug.Log("RGripButton: " + value.isPressed);
        }

        private void OnLTriggerButton(InputValue value)
        {
            Debug.Log("LTriggerButton: " + value.isPressed);
        }

        private void OnRTriggerButton(InputValue value)
        {
            Debug.Log("RTriggerButton: " + value.isPressed);
        }

        private void OnLPrimary2DAxisClick(InputValue value)
        {
            Debug.Log("LPrimary2DAxisClick: " + value.isPressed);
        }

        private void OnRPrimary2DAxisClick(InputValue value)
        {
            Debug.Log("RPrimary2DAxisClick: " + value.isPressed);
        }

        private void OnLPrimary2DAxisTouch(InputValue value)
        {
            Debug.Log("LPrimary2DAxisTouch: " + value.isPressed);
        }

        private void OnRPrimary2DAxisTouch(InputValue value)
        {
            Debug.Log("RPrimary2DAxisTouch: " + value.isPressed);
        }

        private void OnMenuButton(InputValue value)
        {
            Debug.Log("MenuButton: " + value.isPressed);
        }


    }
}
