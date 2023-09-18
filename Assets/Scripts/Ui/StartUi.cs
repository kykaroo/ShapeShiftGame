using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class StartUi : MonoBehaviour
    { 
        [SerializeField] private Button startButton;
        [SerializeField] private Button shopButton;
        
        public event Action OnStartButtonClick;
        public event Action OnShopButtonClick;

        private void StartButtonClick() => OnStartButtonClick?.Invoke();
        private void ShopButtonClick() => OnShopButtonClick?.Invoke();
        
        private void Awake()
        {
            startButton.onClick.AddListener(StartButtonClick);
            shopButton.onClick.AddListener(ShopButtonClick);
        }
    }
}