using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopCategoryButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private Color selectColor;
        [SerializeField] private Color unselectColor;

        public event Action Click;

        private void OnClick() => Click?.Invoke();

        private void Awake() => button.onClick.AddListener(OnClick);

        public void Select() => image.color = selectColor;
        public void Unselect() => image.color = unselectColor;
    }
}