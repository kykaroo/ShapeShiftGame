using System;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ShopCategoryButton humanFormSkinsButton;
        [SerializeField] private ShopCategoryButton carFormSkinsButton;
        [SerializeField] private ShopCategoryButton helicopterFormSkinsButton;
        [SerializeField] private ShopCategoryButton boatFormSkinsButton;
        [SerializeField] private ShopPanel shopPanel;
        [SerializeField] private ShopContent contentItems;
        [SerializeField] private Button backButton; 

        private ShopCategoryButton _currentButtonSelect;

        public event Action OnBackButtonClick;
        
        private void BackButtonClick() => OnBackButtonClick?.Invoke();

        private void Awake()
        {
            humanFormSkinsButton.Click += OnHumanFormSkinsButtonClick;
            carFormSkinsButton.Click += OnCarFormSkinsButtonClick;
            helicopterFormSkinsButton.Click += OnHelicopterFormSkinsButtonClick;
            boatFormSkinsButton.Click += OnBoatFormSkinsButtonClick;
            backButton.onClick.AddListener(BackButtonClick);
            OnHumanFormSkinsButtonClick();
        }
        
        

        private void OnBoatFormSkinsButtonClick()
        {
            _currentButtonSelect.Unselect();
            _currentButtonSelect = boatFormSkinsButton;
            _currentButtonSelect.Select();
            shopPanel.Show(contentItems.BoatFormSkinItems);
        }

        private void OnHelicopterFormSkinsButtonClick()
        {
            _currentButtonSelect.Unselect();
            _currentButtonSelect = helicopterFormSkinsButton;
            _currentButtonSelect.Select();
            shopPanel.Show(contentItems.HelicopterFormSkinItems);
        }

        private void OnCarFormSkinsButtonClick()
        {
            _currentButtonSelect.Unselect();
            _currentButtonSelect = carFormSkinsButton;
            _currentButtonSelect.Select();
            shopPanel.Show(contentItems.CarFormSkinItems);
        }

        private void OnHumanFormSkinsButtonClick()
        {
            _currentButtonSelect?.Unselect();
            _currentButtonSelect = humanFormSkinsButton;
            _currentButtonSelect.Select();
            shopPanel.Show(contentItems.HumanFormSkinItems);
        }
    }
}