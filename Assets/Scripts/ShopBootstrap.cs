using System;
using Data;
using Shop;
using UnityEngine;
using Wallet;

public class ShopBootstrap : MonoBehaviour
{
    [SerializeField] private Shop.Shop shop;
    [SerializeField] private WalletView walletView;

    private IDataProvider _dataProvider;
    private IPersistentPlayerData _persistentPlayerData;
    private Wallet.Wallet _wallet;

    public Wallet.Wallet Wallet => _wallet;

    public void Initialize(IPersistentPlayerData persistentPlayerData, IDataProvider dataProvider)
    {
        _persistentPlayerData = persistentPlayerData;
        _dataProvider = dataProvider;
        
        InitializeWallet();
        InitializeShop();
    }

    private void InitializeShop()
    {
        var openSkinsChecker = new OpenSkinsChecker(_persistentPlayerData);
        var selectedSkinChecker = new SelectedSkinChecker(_persistentPlayerData);
        var skinSelector = new SkinSelector(_persistentPlayerData);
        var skinUnlocker = new SkinUnlocker(_persistentPlayerData);
        
        shop.Initialize(_dataProvider, _wallet, openSkinsChecker, selectedSkinChecker, skinSelector, skinUnlocker);
    }

    private void InitializeWallet()
    {
        _wallet = new(_persistentPlayerData);
        walletView.Initialize(_wallet);
    }
}