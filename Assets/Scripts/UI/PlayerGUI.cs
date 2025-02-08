using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerGUI : MonoBehaviour
    {
        [SerializeField] private Image staminaLine;
        [SerializeField] private GameObject chatPanel;
        public Image StaminaLine => staminaLine;
        private DefaultAction _input;

        private void Awake()
        {
            _input = new DefaultAction();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.UI.Chat.performed += context => OpenChat(context);
        }

        private void OpenChat(InputAction.CallbackContext context)
        {
            if (chatPanel.activeSelf)
            {
                chatPanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                chatPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
}