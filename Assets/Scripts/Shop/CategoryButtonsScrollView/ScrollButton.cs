using UnityEngine;
using UnityEngine.EventSystems;

namespace Shop.CategoryButtonsScrollView
{
    public class ScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool isDown;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            isDown = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isDown = false;
        }
    }
}