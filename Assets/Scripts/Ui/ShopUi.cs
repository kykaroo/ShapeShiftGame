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
using YG;
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

        private SkinsManager _skinsManager;

        private IDataProvider<PlayerGameData> _gameDataProvider;
        private Wallet _wallet;
        private ShopItemView _previewedItem;
        private YandexGame _yandexGame;
        private RewardedAd.RewardedAd _rewardedAd;

        public event Action OnBackButtonClick;

        private void BackButtonClick() => OnBackButtonClick?.Invoke();
        
        [Inject]
        public void Initialize(IDataProvider<PlayerGameData> gameDataProvider, Wallet wallet, PlayerGameData playerGameData,
            YandexGame yandexGame, SkinsManager skinsManager, RewardedAd.RewardedAd rewardedAd)
        {
            _wallet = wallet;
            _gameDataProvider = gameDataProvider;
            _yandexGame = yandexGame;
            _skinsManager = skinsManager;
            _rewardedAd = rewardedAd;
            walletView.Initialize(_wallet);

            shopPanel.Initialize(_skinsManager.OpenChecker, _skinsManager.SelectedChecker);
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

        private void SaveData(int obj)
        {
            _gameDataProvider.Save();
        }

        private void OnItemViewClicked(ShopItemView item)
        {
            _previewedItem = item;
            _skinsManager.OpenChecker.Visit(_previewedItem.Item);
            skinPlacement.InstantiateModel(_previewedItem.Model);

            if (_skinsManager.OpenChecker.IsOpened)
            {
                _skinsManager.SelectedChecker.Visit(_previewedItem.Item);

                if (_skinsManager.SelectedChecker.IsSelected)
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
            if (_previewedItem.IsAdReward)
            {
                _rewardedAd.ShopItem= _previewedItem.Item;
                _yandexGame._RewardedShow(_previewedItem.AdUnlockId);
                return;
            }
            if (!_wallet.IsEnough(_previewedItem.Price)) return;
            
            _wallet.Spend(_previewedItem.Price);
            UnlockSkin();
        }

        public void UnlockSkin()
        {
            _skinsManager.Unlocker.Visit(_previewedItem.Item);
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

            if (_wallet.IsEnough(price) || _previewedItem.IsAdReward)
            {
                buyButton.SetAvailable(_previewedItem.IsAdReward);
            }
            else
            {
                buyButton.SetNotAvailable(_previewedItem.IsAdReward);
            }

            HideSelectedText();
            HideSelectionButton();
        }

        private void UpdateButtonAvailability(int price)
        {
            if (_wallet.IsEnough(price))
            {
                buyButton.SetAvailable(_previewedItem.IsAdReward);
            }
            else
            {
                buyButton.SetNotAvailable(_previewedItem.IsAdReward);
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
            _skinsManager.Selector.Visit(_previewedItem.Item);
            shopPanel.Select(_previewedItem);
            ShowSelectedText();
        }
        
        private void DeleteGameSave()
        {
            _gameDataProvider.DeleteSave();
            _gameDataProvider.GetData();
            _wallet.UpdateCoinsView();
        }

        private void HideSelectionButton() => selectionButton.gameObject.SetActive(false);
        private void HideSelectedText() => selectedText.gameObject.SetActive(false);
        private void HideBuyButton() => buyButton.gameObject.SetActive(false);
    }
}