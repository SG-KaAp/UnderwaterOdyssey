using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Base.Inventory
{
    public class ItemSlotGUI : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private InventoryItem item;
        [SerializeField] public bool IsFilled { get; private set; }

        public void SetItem(InventoryItem newItem)
        {
            itemImage.sprite = newItem.ItemIcon;
            itemName.text = newItem.ItemName;
            item = newItem;
            IsFilled = true;
        }
    }
}