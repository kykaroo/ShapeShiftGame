using Data;
using Shop;
using UnityEngine;
using Wallet;

public class ShopBootstrap : MonoBehaviour
{
    [SerializeField] private Shop.Shop shop;
    [SerializeField] private WalletView walletView;

    private IDataProvider _dataProvider;
    private IPersistentData _persistentPlayerData;
    private Wallet.Wallet _wallet;

    public IDataProvider DataProvider => _dataProvider;

    public IPersistentData PersistentPlayerData => _persistentPlayerData;

    public void Awake()
    {
        InitializeData();
        shop.OnDeleteSaveButtonClick += LoadData;
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

    private void InitializeData()
    {
        _persistentPlayerData = new PersistentData();
        _dataProvider = new PlayerPrefsProvider(_persistentPlayerData);

        LoadData();
    }

    private void LoadData()
    {
        if (_dataProvider.TryLoad() == false)
        {
            _persistentPlayerData.PlayerData = new();
        }

        InitializeWallet();
        InitializeShop();
    }
}