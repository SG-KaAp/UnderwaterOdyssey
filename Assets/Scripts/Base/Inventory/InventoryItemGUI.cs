using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Base.Inventory
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField] private Inventory inventory;
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private GameObject uiGroup;
        [SerializeField] private Button itemButton;
        [SerializeField] private Image itemIcon;
        //[SerializeField] private TextMeshProUGUI itemLabel;
        [SerializeField] public GameObject ItemPrefab { get; private set; }
        [SerializeField] private GameObject item;
        private bool isPulledOut;
        private bool itemDisposable;
        public void PullingOut()
        {
            if (!isPulledOut)
            {
                inventory.ReturnAll();
                PullOut();
            }
            else
            {
                if (isPulledOut)
                {
                    Return();
                }
            }
        }
        public void PullOut()
        {
            item.SetActive(true);
            isPulledOut = true;
            if (itemDisposable)
            {
                Delete();
            }
        }
        public void Return()
        {
            if (item != null)
            {
                item.SetActive(false);
                isPulledOut = false;
            }
        }
        public bool IsFiled { get; private set; }
        public void Add(Sprite icon, GameObject prefab, bool disposable)
        {
            IsFiled = true;
            //uiGroup.SetActive(true);
            itemButton.enabled = true;
            itemIcon.sprite = icon;
            //itemLabel.text = label;
            ItemPrefab = prefab;
            itemDisposable = disposable;
            item = Instantiate(ItemPrefab, playerCamera.transform);
            item.SetActive(false);
        }
        public void Delete()
        {
            //uiGroup.SetActive(false);
            itemButton.enabled = false;
            isPulledOut = false;
            IsFiled = false;
        }
    }
}