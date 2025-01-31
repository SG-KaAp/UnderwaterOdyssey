using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Base.Inventory
{
    public class Inventory : MonoBehaviour
    {
        //[SerializeField] private GameObject inventoryGameObject;
        //[SerializeField] private Animator inventoryAnimator;
        //SerializeField] private bool lockMouseCursor;
        [SerializeField] private List<ItemButton> itemButtons;
        private DefaultAction input;

        private void Awake()
        {
            input = new DefaultAction();
        }
        private void OnEnable() { input.Enable(); }
        private void OnDisable() { input.Disable(); }
        /*private void Update()
        {
            if (input.Player.Inventory.triggered) Switch();
        }
        private void Switch()
        {
            if (!inventoryGameObject.activeSelf)
            {
                //inventoryAnimator.Play("InventoryOpen");
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                if (inventoryGameObject.activeSelf)
                {
                    //inventoryAnimator.Play("InventoryClose");
                    //if (SceneManager.GetActiveScene().name == "Inbound") lockMouseCursor = true;
                    //if (lockMouseCursor) Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }*/
        /*public void InventorySetActiveTrue()
        {
            inventoryGameObject.SetActive(true);
        }
        public void InventorySetActiveFalse()
        {
            inventoryGameObject.SetActive(false);
        }*/
        public void Add(Sprite icon, GameObject prefab, bool disposable)
        {
            for (int i = 0; i <= itemButtons.Count; i++)
            {
                if (!itemButtons[i].IsFiled)
                {
                    itemButtons[i].Add(icon,prefab, disposable);
                    i = itemButtons.Count + 1;
                    break;
                }
            }
        }

        public void ReturnAll()
        {
            for (int i = 0; i <= itemButtons.Count - 1; i++)
            {
                itemButtons[i].Return();
            }
        }
    }
}