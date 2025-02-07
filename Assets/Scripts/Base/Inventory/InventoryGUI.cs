using System.Collections.Generic;
using UnityEngine;

namespace Base.Inventory
{
    public class InventoryGUI : MonoBehaviour
    {
        [SerializeField] private InventoryLogic inventoryLogic;
        [SerializeField] private List<ItemSlotGUI> itemSlots;

        public void AddItem(InventoryItem newItem, GameObject newItemObject)
        {
            foreach (ItemSlotGUI item in itemSlots)
            {
                if (!item.IsFilled)
                {
                    item.SetItem(newItem, newItemObject);
                    break;
                }
            }
        }

        public void ActivateItem(ItemSlotGUI item)
        {
            inventoryLogic.SelectItem(item.ItemObject);
        }
    }
}