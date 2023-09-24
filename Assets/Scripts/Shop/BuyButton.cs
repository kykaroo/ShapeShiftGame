using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class BuyButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Color lockColor;
        [SerializeField] private Color unlockColor;

        private bool _isLock;

        public event Action Click; 

        private void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        public void UpdateText(int price) => text.text = price.ToString();

        public void Lock()
        {
            _isLock = true;
            text.color = lockColor;
        }

        public void Unlock()
        {
            _isLock = false;
            text.color = unlockColor;
        }

        private void OnButtonClick()
        {
            if (_isLock)
            {
                return;
            }
            
            Click?.Invoke();
        }
    }
}