using UnityEngine;

namespace Base.Inventory
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private InventoryItem inventoryItem;
        [SerializeField] private Inventory inventory;
        private void OnTriggerEnter(Collider other)
        {
            inventory.Add(inventoryItem.ItemIcon, inventoryItem.ItemPrefab, inventoryItem.Disposable);
            Debug.Log("Pick up item: " + inventoryItem.ItemName);
            Destroy(gameObject);
        }
    }
}