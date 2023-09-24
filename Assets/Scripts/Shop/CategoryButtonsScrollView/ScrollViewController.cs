using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.CategoryButtonsScrollView
{
    public class ScrollViewController : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private ScrollButton leftButton;
        [SerializeField] private ScrollButton rightButton;
        [SerializeField] private float scrollSpeed;

        private void Update()
        {
            if (leftButton.isDown)
            {
                ScrollLeft();
            }

            if (rightButton.isDown)
            {
                ScrollRight();
            }
        }

        private void ScrollRight()
        {
            if (scrollRect.horizontalNormalizedPosition <= 1)
            {
                scrollRect.horizontalNormalizedPosition += scrollSpeed;
            }
        }

        private void ScrollLeft()
        {
            if (scrollRect.horizontalNormalizedPosition >= 0)
            {
                scrollRect.horizontalNormalizedPosition -= scrollSpeed;
            }
        }
    }
}
