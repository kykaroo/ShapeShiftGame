﻿using System;
using Data;
using Shop.ShopModelView;
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
        [SerializeField] private BuyButton buyButton;
        [SerializeField] private Button selectionButton;
        [SerializeField] private Button clearSaveButton;
        [SerializeField] private Image selectedText;
        [SerializeField] private SkinPlacement skinPlacement;

        private ShopCategoryButton _currentButtonSelect;

        private SkinSelector _skinSelector;
        private SkinUnlocker _skinUnlocker;
        private OpenSkinsChecker _openSkinsChecker;
        private SelectedSkinChecker _selectedSkinChecker;

        private IDataProvider _dataProvider;
        private Wallet.Wallet _wallet;
        private ShopItemView _previewedItem;

        public event Action OnBackButtonClick;
        public event Action OnDeleteSaveButtonClick;
        
        private void BackButtonClick()
        {
            OnBackButtonClick?.Invoke();
        }

        private void Awake()
        {
            humanFormSkinsButton.Click += OnHumanFormSkinsButtonClick;
            carFormSkinsButton.Click += OnCarFormSkinsButtonClick;
            helicopterFormSkinsButton.Click += OnHelicopterFormSkinsButtonClick;
            boatFormSkinsButton.Click += OnBoatFormSkinsButtonClick;
            backButton.onClick.AddListener(BackButtonClick);
            buyButton.Click += OnBuyButtonClick;
            clearSaveButton.onClick.AddListener(DeleteSave);
            selectionButton.onClick.AddListener(OnSelectionButtonClick);
        }

        public void Initialize(IDataProvider dataProvider, Wallet.Wallet wallet, OpenSkinsChecker openSkinsChecker,
            SelectedSkinChecker selectedSkinChecker, SkinSelector skinSelector, SkinUnlocker skinUnlocker)
        {
            _wallet = wallet;
            _openSkinsChecker = openSkinsChecker;
            _selectedSkinChecker = selectedSkinChecker;
            _skinSelector = skinSelector;
            _skinUnlocker = skinUnlocker;
            _dataProvider = dataProvider;
            shopPanel.Initialize(openSkinsChecker, selectedSkinChecker);
            
            shopPanel.ItemViewClicked += OnItemViewClicked;
            
            OnHumanFormSkinsButtonClick();
        }

        private void OnItemViewClicked(ShopItemView item)
        {
            _previewedItem = item;
            _openSkinsChecker.Visit(_previewedItem.Item);
            skinPlacement.InstantiateModel(_previewedItem.Model);

            if (_openSkinsChecker.IsOpened)
            {
                _selectedSkinChecker.Visit(_previewedItem.Item);

                if (_selectedSkinChecker.IsSelected)
                {
                    ShowSelectedText();
                    return;
                }
                
                ShowSelectionButton();
            }
            else
            {
                ShowBuyButton(_previewedItem.Price);
            }
        }

        private void OnBuyButtonClick()
        {
            if (!_wallet.IsEnough(_previewedItem.Price)) return;
            
            _wallet.Spend(_previewedItem.Price);
            _skinUnlocker.Visit(_previewedItem.Item);
            SelectSkin();
            _previewedItem.Unlock();
            _dataProvider.Save();
        }

        private void OnSelectionButtonClick()
        {
            SelectSkin();
            _dataProvider.Save();
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

        private void ShowBuyButton(int price)
        {
            buyButton.gameObject.SetActive(true);
            buyButton.UpdateText(price);

            if (_wallet.IsEnough(price))
            {
                buyButton.Unlock();
            }
            else
            {
                buyButton.Lock();
            }

            HideSelectedText();
            HideSelectionButton();
        }

        private void ShowSelectionButton()
        {
            selectionButton.gameObject.SetActive(true);
            HideBuyButton();
            HideSelectedText();
        }

        private void ShowSelectedText()
        {
            selectedText.gameObject.SetActive(true);
            HideBuyButton();
            HideSelectionButton();
        }

        private void SelectSkin()
        {
            _skinSelector.Visit(_previewedItem.Item);
            shopPanel.Select(_previewedItem);
            ShowSelectedText();
        }
        
        private void DeleteSave()
        {
            _dataProvider.DeleteSave();
            OnDeleteSaveButtonClick?.Invoke();
        }

        private void HideSelectionButton() => selectionButton.gameObject.SetActive(false);
        private void HideSelectedText() => selectedText.gameObject.SetActive(false);
        private void HideBuyButton() => buyButton.gameObject.SetActive(false);
    }
}