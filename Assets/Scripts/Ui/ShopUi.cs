using System;
using System.Collections.Generic;
using Data;
using Data.PlayerGameData;
using FormStateMachine;
using Shop;
using Shop.ShopModelView;
using UnityEngine;
using UnityEngine.UI;
using Wallet;
using Zenject;

namespace Ui
{
    using Wallet = Wallet.Wallet;
    public class ShopUi : MonoBehaviour
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
        [SerializeField] private WalletView walletView;

        private ShopCategoryButton _currentButtonSelect;

        private SkinSelector _skinSelector;
        private SkinUnlocker _skinUnlocker;
        private OpenSkinsChecker _openSkinsChecker;
        private SelectedSkinChecker _selectedSkinChecker;

        private IDataProvider<PersistentGameData> _gameDataProvider;
        private PersistentGameData _persistentGameData;
        private Wallet _wallet;
        private ShopItemView _previewedItem;
        private Dictionary<IFormState, bool> _skinChanges;

        public SkinUnlocker SkinUnlocker => _skinUnlocker;
        

        public OpenSkinsChecker OpenSkinsChecker => _openSkinsChecker;

        public event Action OnBackButtonClick;

        private void BackButtonClick() => OnBackButtonClick?.Invoke();
        
        [Inject]
        public void Initialize(IDataProvider<PersistentGameData> gameDataProvider, Wallet wallet, PersistentGameData persistentGameData)
        {
            _wallet = wallet;
            _gameDataProvider = gameDataProvider;
            _persistentGameData = persistentGameData;
            walletView.Initialize(_wallet);
            InitializeCheckers(_persistentGameData);

            shopPanel.Initialize(_openSkinsChecker, _selectedSkinChecker);
            shopPanel.ItemViewClicked += OnItemViewClicked;
            _wallet.CoinsChanged += SaveData;
            
            humanFormSkinsButton.Click += OnHumanFormSkinsButtonClick;
            carFormSkinsButton.Click += OnCarFormSkinsButtonClick;
            helicopterFormSkinsButton.Click += OnHelicopterFormSkinsButtonClick;
            boatFormSkinsButton.Click += OnBoatFormSkinsButtonClick;
            
            buyButton.Click += OnBuyButtonClick;
            _wallet.CoinsChanged += _ => ShowBuyButton(_previewedItem.Price);
            
            backButton.onClick.AddListener(BackButtonClick);
            clearSaveButton.onClick.AddListener(DeleteGameSave);
            selectionButton.onClick.AddListener(OnSelectionButtonClick);
            
            OnHumanFormSkinsButtonClick();
        }

        private void InitializeCheckers(PersistentGameData persistentGameData)
        {
            _openSkinsChecker = new(persistentGameData);
            _selectedSkinChecker = new(persistentGameData);
            _skinSelector = new(persistentGameData);
            _skinUnlocker = new(persistentGameData);
        }

        private void SaveData(int obj)
        {
            _gameDataProvider.Save();
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
            _gameDataProvider.Save();
        }

        private void OnSelectionButtonClick()
        {
            SelectSkin();
            _gameDataProvider.Save();
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
                buyButton.SetAvailable();
            }
            else
            {
                buyButton.SetNotAvailable();
            }

            HideSelectedText();
            HideSelectionButton();
        }

        private void UpdateButtonAvailability(int price)
        {
            if (_wallet.IsEnough(price))
            {
                buyButton.SetAvailable();
            }
            else
            {
                buyButton.SetNotAvailable();
            }
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
        
        private void DeleteGameSave()
        {
            _gameDataProvider.DeleteSave();
            _gameDataProvider.GetData();
            _wallet.UpdateCoinsView();
            InitializeCheckers(_persistentGameData);
        }

        private void HideSelectionButton() => selectionButton.gameObject.SetActive(false);
        private void HideSelectedText() => selectedText.gameObject.SetActive(false);
        private void HideBuyButton() => buyButton.gameObject.SetActive(false);
    }
}