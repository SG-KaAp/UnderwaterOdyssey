using System.Collections.Generic;
using UnityEngine;

namespace Base.Inventory
{
    public class InventoryLogic : MonoBehaviour
    {
        [SerializeField] private List<GameObject> items;
        [SerializeField] private GameObject itemsParent;
        [SerializeField] private InventoryGUI gui;

        private void Awake()
        {
            itemsParent = Camera.main.transform.gameObject;
            gui = FindFirstObjectByType<InventoryGUI>();
        }

        public void AddItem(InventoryItem item)
        {
            GameObject newItem = Instantiate(item.ItemPrefab, itemsParent.transform);
            newItem.SetActive(false);
            items.Add(newItem);
            gui.AddItem(item, newItem);
        }

        public void SelectItem(GameObject item)
        {
            foreach (GameObject itemObject in items)
            {
                if (itemObject == item)
                {
                    itemObject.SetActive(true);
                }
                else
                {
                    itemObject.SetActive(false);
                }
            }
        }
    }
}