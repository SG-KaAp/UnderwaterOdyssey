using System.Collections.Generic;
using UnityEngine;

namespace Base.Inventory
{
    public class InventoryGUI : MonoBehaviour
    {
        [SerializeField] private List<ItemSlotGUI> itemSlots;

        public void AddItem(InventoryItem newItem)
        {
            foreach (ItemSlotGUI item in itemSlots)
            {
                if (!item.IsFilled)
                {
                    item.SetItem(newItem);
                    break;
                }
            }
        }
    }
}