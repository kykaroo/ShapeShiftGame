using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopCategoryButton : MonoBehaviour
    {
        [SerializeField] private bool useSprites;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Sprite selectSprite;
        [SerializeField] private Sprite unselectSprite;
        [SerializeField] private Color selectColor;
        [SerializeField] private Color unselectColor;

        public event Action Click;

        private void OnClick() => Click?.Invoke();

        private void Awake() => button.onClick.AddListener(OnClick);

        public void Select()
        {
            if (useSprites)
            {
                image.sprite = selectSprite;
            }
            else
            { 
                image.color = selectColor;
            }
        }

        public void Unselect()
        {
            if (useSprites)
            {
                image.sprite = unselectSprite;
            }
            else
            { 
                image.color = unselectColor;
            }
        }
    }
}