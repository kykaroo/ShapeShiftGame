using Data;
using Shop;
using Ui;
using UnityEngine;
using Wallet;

public class ShopBootstrap : MonoBehaviour
{
    private ShopUi _shopUi;
    [SerializeField] private WalletView walletView;

    private IDataProvider _gameDataProvider;
    private IPersistentPlayerData _persistentPlayerData;
    private Wallet.Wallet _wallet;

    public Wallet.Wallet Wallet => _wallet;

    public void Initialize(IPersistentPlayerData persistentPlayerData, IDataProvider gameDataProvider, ShopUi shopUi)
    {
        _persistentPlayerData = persistentPlayerData;
        _gameDataProvider = gameDataProvider;
        _shopUi = shopUi;
        
        InitializeWindows();
    }

    public void InitializeWindows()
    {
        InitializeWallet();
        InitializeShop();
    }

    private void InitializeShop()
    {
        var openSkinsChecker = new OpenSkinsChecker(_persistentPlayerData);
        var selectedSkinChecker = new SelectedSkinChecker(_persistentPlayerData);
        var skinSelector = new SkinSelector(_persistentPlayerData);
        var skinUnlocker = new SkinUnlocker(_persistentPlayerData);
        
        _shopUi.Initialize(_gameDataProvider, _wallet, openSkinsChecker, selectedSkinChecker, skinSelector, skinUnlocker);
    }

    private void InitializeWallet()
    {
        _wallet = new(_persistentPlayerData);
        walletView.Initialize(_wallet);
    }
}