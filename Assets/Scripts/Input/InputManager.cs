using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        private DefaultAction _input;
        private void Awake() { _input = new DefaultAction(); }
        private void OnEnable() { _input.Enable(); }
        private void OnDisable() { _input.Disable(); }
        public Vector2 GetMovementVector() { return _input.Player.MovementVector.ReadValue<Vector2>(); }
        public Vector2 GetCameraLookVector() { return _input.Player.CameraLook.ReadValue<Vector2>(); }
        public Vector2 GetMousePosition() { return _input.UI.MousePosition.ReadValue<Vector2>(); }
        public bool GetJumpButtonState() { return _input.Player.Jump.triggered; }
        public bool GetSprintButtonState() { return _input.Player.Sprint.IsPressed(); }
        public bool GetCrouchButtonState() { return _input.Player.Crouch.IsPressed(); }
        public bool GetMouse1ButtonState() { return _input.Player.Attack1.IsPressed(); }
        public bool GetUseButtonState() { return _input.Player.Use.IsPressed(); }
        public bool GetEscapeButtonState() { return _input.Player.EspaceButton.triggered; }
    }
}