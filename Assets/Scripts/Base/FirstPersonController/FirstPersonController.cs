/* "SGTeam", 2023
by Kazantsev Arseniy*/

//using Unity.Cinemachine;
using UnityEngine;

namespace Base.FirstPersonController
{
    
    [RequireComponent(typeof(Rigidbody))]
    public class FirstPersonController : MonoBehaviour
    {
        //Get need components
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private RectTransform staminaLine;
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
        private float currentSpeed = 5f;
        private Vector2 playerMovementVector;
        private Vector2 cameraLookVector;
        private float cameraLookX;
        private float cameraLookY;
        private bool isSprinted = false;
        private bool sprintAllowed = true;
        private float stamina = 6f;
        private DefaultAction input;

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

        private void Awake()
        {
            input = new DefaultAction();
            Cursor.lockState = CursorLockMode.Locked;
            currentSpeed = walkSpeed;
        }
        private void OnEnable() { input.Enable(); }
        private void OnDisable() { input.Disable(); }

        private void Update()
        {
            // Check: Movement is enable?
            if (enableMovement && _isGrounded())
            {
                playerMovementVector = input.Player.MovementVector.ReadValue<Vector2>();;
                PlayerMove(playerMovementVector);
            }

            //Check: Movement is enable?
            if (enableCameraLook)
            {
                cameraLookVector = input.Player.CameraLook.ReadValue<Vector2>();
                CameraLook(cameraLookVector);
            }

            if (input.Player.Jump.triggered)
            {
                Jump(jumpForce);
            }
 
            Sprint(input.Player.Sprint.IsPressed(), sprintSpeed);
        }

        private void PlayerMove(Vector2 playerMovementVector)
        {
            //Getting and adding movement values
            float moveX = playerMovementVector.x;
            float moveZ = playerMovementVector.y;
            Vector3 moveVector = (moveX * transform.right + moveZ * transform.forward) * currentSpeed;
            rb.linearVelocity = moveVector + rb.linearVelocity.y * transform.up;
            //if (rb.linearVelocity != new Vector3(0,0,0) && footstepsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) footstepsAnimator.Play("Footsteps");
        }

        private void CameraLook(Vector2 cameraLookVector)
        {
            //Getting and adding camera look values
            cameraLookX -= cameraLookVector.y * mouseSensitivity;
            cameraLookY += cameraLookVector.x * mouseSensitivity;
            if (enableXClamp)
            {
                cameraLookX = Mathf.Clamp(cameraLookX, minCameraX, maxCameraX);
            }
            playerCamera.transform.localEulerAngles = new Vector3(cameraLookX, 0, 0);
            transform.eulerAngles = new Vector3(0, cameraLookY, 0);
        }

        private void Jump(float force)
        {
            if (_isGrounded())
            {
                rb.linearVelocity += transform.up *  force;
            }
            Debug.Log("Player jumped");
        }

        private void Sprint(bool enable, float speed)
        {
            if (enable && _isGrounded() && sprintAllowed)
            {
                isSprinted = true;
                currentSpeed = speed;
                stamina -= 1 * Time.deltaTime;
                float normalizedValue = Mathf.InverseLerp(0, 6, stamina);
                float result = Mathf.Lerp(0, 1, normalizedValue);
                staminaLine.localScale = new Vector2(result, 1);
            }
            else
            {
                isSprinted = false;
                currentSpeed = walkSpeed;
                if (stamina < 6f)
                {
                    stamina += 1 * Time.deltaTime;
                    float normalizedValue = Mathf.InverseLerp(0, 6, stamina);
                    float result = Mathf.Lerp(0, 1, normalizedValue);
                    staminaLine.localScale = new Vector2(result, 1);
                }
            }

            if (stamina <= 0)
            {
                sprintAllowed = false;
            }

            if (stamina >= 2)
            {
                sprintAllowed = true;
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