/* "SGTeam", 2023
by Kazantsev Arseniy*/

//using Unity.Cinemachine;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine.UI;

namespace Base.FirstPersonController
{
    
    [RequireComponent(typeof(Rigidbody))]
    public class MultiplayerFirstPersonController : NetworkBehaviour
    {
        //Get need components
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private UI.PlayerGUI playerGUI;
        [SerializeField] private Image staminaLine;
        [SerializeField] private Input.InputManager input;
        //[SerializeField] private Animator footstepsAnimator;

        //Settings
        [SerializeField] private bool enableMovement = true;
        [SerializeField] private bool enableCameraLook = true;
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 7f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float mouseSensitivity = 0.5f;
        [SerializeField] private bool enableXClamp = true;
        [SerializeField, Range(-360, 360)] private float maxCameraX = 60f;
        [SerializeField, Range(-360, 360)] private float minCameraX = -60f;

        //Mutable Variables
        private Vector3 _originalScale;
        private float _currentSpeed = 5f;
        private Vector2 _playerMovementVector;
        private Vector2 _cameraLookVector;
        private float _cameraLookX;
        private float _cameraLookY;
        private bool _isSprinted = false;
        private bool _sprintAllowed = true;
        private float _stamina = 6f;
        private bool _isCrouched = false;

        private bool _isGrounded()
        {
            //Create a raycast
            Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
            Vector3 direction = transform.TransformDirection(Vector3.down);
            float distance = .75f;
            //Check a raycast
            if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
            {
                Debug.DrawRay(origin, direction * distance, Color.red);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!base.IsOwner) return;
            playerCamera = Camera.main;
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0, 0.5f, 0);
            input = FindAnyObjectByType<Input.InputManager>();
            playerGUI = FindAnyObjectByType<UI.PlayerGUI>();
            staminaLine = playerGUI.StaminaLine;
            Cursor.lockState = CursorLockMode.Locked;
            _currentSpeed = walkSpeed;
            _originalScale = transform.localScale;
        }

        private void Update()
        {
            if (!base.IsOwner) return;
            // Check: Movement is enable?
            if (enableMovement && _isGrounded())
            {
                _playerMovementVector = input.GetMovementVector();
                PlayerMove(_playerMovementVector);
            }

            //Check: Movement is enable?
            if (enableCameraLook)
            {
                _cameraLookVector = input.GetCameraLookVector();
                CameraLook(_cameraLookVector);
            }

            if (input.GetJumpButtonState())
            {
                Jump(jumpForce);
            }
            Crouch(input.GetCrouchButtonState(),_isCrouched);
            Sprint(input.GetSprintButtonState(), sprintSpeed);
        }

        private void PlayerMove(Vector2 _playerMovementVector)
        {
            //Getting and adding movement values
            float moveX = _playerMovementVector.x;
            float moveZ = _playerMovementVector.y;
            Vector3 moveVector = (moveX * transform.right + moveZ * transform.forward) * _currentSpeed;
            rb.linearVelocity = moveVector + rb.linearVelocity.y * transform.up;
            //if (rb.linearVelocity != new Vector3(0,0,0) && footstepsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) footstepsAnimator.Play("Footsteps");
            
        }

        private void CameraLook(Vector2 _cameraLookVector)
        {
            //Getting and adding camera look values
            _cameraLookX -= _cameraLookVector.y * mouseSensitivity;
            _cameraLookY += _cameraLookVector.x * mouseSensitivity;
            if (enableXClamp)
            {
                _cameraLookX = Mathf.Clamp(_cameraLookX, minCameraX, maxCameraX);
            }
            playerCamera.transform.localEulerAngles = new Vector3(_cameraLookX, 0, 0);
            transform.eulerAngles = new Vector3(0, _cameraLookY, 0);
        }

        private void Crouch(bool enable, bool crouch)
        {
            if (!crouch && enable)
            {
                transform.localScale = new Vector3(_originalScale.x, .75f, _originalScale.z);
                crouch = false;
            }
            else
            {
                transform.localScale = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
                crouch = true;
            }
        }

        private void Jump(float force)
        {
            if (_isGrounded())
            {
                rb.linearVelocity += transform.up * force;
            }
        }

        private void Sprint(bool enable, float speed)
        {
            if (enable && _isGrounded() && _sprintAllowed && input.GetMovementVector() != Vector2.zero)
            {
                _isSprinted = true;
                _currentSpeed = speed;
                _stamina -= 1 * Time.deltaTime;
                float normalizedValue = Mathf.InverseLerp(0, 6, _stamina);
                float result = Mathf.Lerp(0, 1, normalizedValue);
                staminaLine.fillAmount = result;
            }
            else
            {
                _isSprinted = false;
                _currentSpeed = walkSpeed;
                if (_stamina < 6f)
                {
                    _stamina += 1 * Time.deltaTime;
                    float normalizedValue = Mathf.InverseLerp(0, 6, _stamina);
                    float result = Mathf.Lerp(0, 1, normalizedValue);
                    staminaLine.fillAmount = result;
                }
            }

            if (_stamina <= 0)
            {
                _sprintAllowed = false;
            }

            if (_stamina >= 2)
            {
                _sprintAllowed = true;
            }
        }

        public void SetEnableMovement(bool state)
        {
            enableMovement = state;
        }

        public void SetCameraLook(bool state)
        {
            enableCameraLook = state;
        }

        public void SetMouseSensitivity(float sensitivity)
        {
            mouseSensitivity = sensitivity;
        }

        public void SetCameraFOV(float fov)
        {
            playerCamera.fieldOfView = fov;
        }

        public void TakeScreenshot()
        {
            ScreenCapture.CaptureScreenshot("Screenshots/Screenshot" +
                                            System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
            Debug.Log("Screenshot saved at " + "Screenshots/Screenshot" +
                      System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
        }
    }
}