using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Inventory
{
    public class InventoryLogic : MonoBehaviour
    {
        [SerializeField] private List<GameObject> items;
        [SerializeField] private InventoryGUI gui;

        private void Awake()
        {
            gui = FindFirstObjectByType<InventoryGUI>();
        }

        public void AddItem(InventoryItem item)
        {
            items.Add(item.ItemPrefab);
            gui.AddItem(item);
        }
    }
}