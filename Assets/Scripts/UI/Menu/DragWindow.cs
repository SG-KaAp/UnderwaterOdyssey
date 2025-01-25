using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform windowRectTransform;
        private Vector2 _mouseOffset;

        public void OnDrag(PointerEventData eventData)
        {
            windowRectTransform.position = eventData.position - _mouseOffset;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseOffset = eventData.position - (Vector2)windowRectTransform.position;
        }
    }
}