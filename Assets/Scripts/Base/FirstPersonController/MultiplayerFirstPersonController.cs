using UnityEngine;
using UnityEngine.InputSystem;
using FishNet.Object;

namespace Base.FirstPersonController
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private float walkingSpeed = 7.5f;
        [SerializeField] private float runningSpeed = 11.5f;
        [SerializeField] private float jumpSpeed = 8.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float lookSpeed = 2.0f;
        [SerializeField] private float lookXLimit = 45.0f;

        [SerializeField] private CharacterController characterController;
        private Vector3 moveDirection = Vector3.zero;
        private float rotationX = 0;
        private DefaultAction inputActions;
        private InputAction move;
        private InputAction look;
        private bool canMove = true;
        private bool isRunning;
        private bool isJumping;
        private float movementDirectionY;

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!IsOwner) return;
            inputActions = new DefaultAction();
            playerCamera = Camera.main;
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0, 0.5f, 0);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            if (!IsOwner) return;
            //move = inputActions.Player.MovementVector;
            //look = inputActions.Player.CameraLook;
            inputActions.Player.Jump.performed += OnJump;
            inputActions.Player.Sprint.performed += ctx => { isRunning = true; };
            inputActions.Player.Sprint.canceled += ctx => { isRunning = false; };
            inputActions.Enable();
        }

        private void OnDisable()
        {
            if (!IsOwner) return;
            move = null;
            inputActions.Player.Sprint.performed -= ctx => { isRunning = true; };
            inputActions.Player.Sprint.canceled -= ctx => { isRunning = false; };
            inputActions.Disable();
        }

        private void Update()
        {
            if (!IsOwner) return;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed)  * move.ReadValue<Vector2>().y : 0;
            float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * move.ReadValue<Vector2>().x : 0;
            movementDirectionY = moveDirection.y;

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);  

            if (!characterController.isGrounded)
                moveDirection.y -= gravity * Time.deltaTime;

            characterController.Move(moveDirection * Time.deltaTime);

            if(isJumping)
                moveDirection.y = jumpSpeed;
            else
                moveDirection.y = movementDirectionY;

            if (canMove)
            {
                rotationX += -look.ReadValue<Vector2>().y * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, look.ReadValue<Vector2>().x * lookSpeed, 0);
            }
        }

        private void OnJump(InputAction.CallbackContext callbackContext)
        {
            if (canMove && characterController.isGrounded)
                isJumping = true;
            else
                isJumping = false;
        }
    }
}