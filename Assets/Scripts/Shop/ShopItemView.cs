using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shop
{
    [RequireComponent(typeof(Image))]
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Sprite defaultBackground;
        [SerializeField] private Sprite highlightBackground;
        [SerializeField] private Image contentImage;
        [SerializeField] private Image lockImage;
        [SerializeField] private Image selectionText;
        [SerializeField] private TextMeshProUGUI priceText;

        private Image _backgroundImage;
        
        public ShopItem Item { get; private set; }
        
        public bool IsLock { get; private set; }

        public int Price => Item.Price;

        public GameObject Model => Item.Model;

        public event Action<ShopItemView> OnClick;

        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this);

        public void Initialize(ShopItem item)
        {
            _backgroundImage = GetComponent<Image>();
            _backgroundImage.sprite = defaultBackground;

            Item = item;

            contentImage.sprite = item.Image;

            priceText.text = $"{Price}";
        }

        public void Lock()
        {
            IsLock = true;
            lockImage.gameObject.SetActive(IsLock);
            priceText.gameObject.SetActive(IsLock);
        }

        public void Unlock()
        {
            IsLock = false;
            lockImage.gameObject.SetActive(IsLock);
            priceText.gameObject.SetActive(IsLock);
        }

        public void Select() => selectionText.gameObject.SetActive(true);
        public void Unselect() => selectionText.gameObject.SetActive(false);

        public void Highlight() => _backgroundImage.sprite = highlightBackground;
        public void UnHighlight() => _backgroundImage.sprite = defaultBackground;
    }
}