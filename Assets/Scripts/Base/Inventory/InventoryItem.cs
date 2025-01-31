using UnityEngine;

namespace Base.Inventory
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        [field: SerializeField] public Sprite ItemIcon { get; private set; }
        [field: SerializeField] public string ItemName { get; private set; } = "Item";
        [field: SerializeField] public GameObject ItemPrefab { get; private set; }
        [field: SerializeField] public bool Disposable { get; private set; }
    }
}