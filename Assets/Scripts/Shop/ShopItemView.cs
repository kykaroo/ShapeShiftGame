using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Shop
{
    [RequireComponent(typeof(Image))]
    public class ShopItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool useSprites;
        [SerializeField] private Sprite defaultBackgroundSprite;
        [SerializeField] private Sprite highlightBackgroundSprite;
        [SerializeField] private Color defaultBackgroundColour;
        [SerializeField] private Color highlightBackgroundColour;
        [SerializeField] private Image contentImage;
        [SerializeField] private Image lockImage;
        [SerializeField] private Image selectionText;
        [SerializeField] private Image priceImage;
        [SerializeField] private TextMeshProUGUI priceText;

        private Image _backgroundImage;

        public ShopItem Item { get; private set; }

        private bool IsLock { get; set; }

        public int Price => Item.Price;

        public GameObject Model => Item.ShopModel;

        public event Action<ShopItemView> OnClick;

        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this);

        public void Initialize(ShopItem item)
        {
            _backgroundImage = GetComponent<Image>();
            
            if (useSprites)
            {
                _backgroundImage.sprite = defaultBackgroundSprite;
            }
            else
            {
                _backgroundImage.color = defaultBackgroundColour;
            }

            Item = item;

            contentImage.sprite = item.Image;

            priceText.text = $"{Price}";
        }

        public void Lock()
        {
            IsLock = true;
            lockImage.gameObject.SetActive(IsLock);
            priceImage.gameObject.SetActive(IsLock);
        }

        public void Unlock()
        {
            IsLock = false;
            lockImage.gameObject.SetActive(IsLock);
            priceImage.gameObject.SetActive(IsLock);
        }

        public void Select() => selectionText.gameObject.SetActive(true);
        public void Unselect() => selectionText.gameObject.SetActive(false);

        public void Highlight()
        {
            if (useSprites)
            { 
                _backgroundImage.sprite = highlightBackgroundSprite;
                return;
            }
            
            _backgroundImage.color = highlightBackgroundColour;
        }

        public void UnHighlight()
        {
            if (useSprites)
            { 
                _backgroundImage.sprite = defaultBackgroundSprite;
                return;
            }
            
            _backgroundImage.color = defaultBackgroundColour;
        }
    }
}