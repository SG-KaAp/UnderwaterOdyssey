using UnityEngine;
namespace Base.Inventory
{
    [CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/InventoryItem")]
    public class InventoryItem : ScriptableObject
    {
        [SerializeField] private GameObject item;
        [SerializeField] private Sprite icon;
        [SerializeField] private float needCeils = 1;
    }
}