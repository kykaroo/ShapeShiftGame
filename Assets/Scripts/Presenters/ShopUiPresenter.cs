using Data;
using Data.PlayerGameData;
using Ui;

namespace Presenters
{
    public class ShopUiPresenter
    {
        private readonly ShopUi _shopUi;
        private readonly StartUi _startUi;
        private PersistentPlayerGameData _persistentPlayerGameData;
        private Player _player;
        private IDataProvider<PersistentPlayerGameData> _playerGameDataProvider;

        public ShopUiPresenter(ShopUi shopUi, StartUi startUi, PersistentPlayerGameData persistentPlayerGameData, 
            Player player, IDataProvider<PersistentPlayerGameData> playerGameDataProvider)
        {
            _shopUi = shopUi;
            _startUi = startUi;
            _persistentPlayerGameData = persistentPlayerGameData;
            _player = player;
            _playerGameDataProvider = playerGameDataProvider;
            
            _shopUi.OnBackButtonClick += CloseShop;
        }

        private void CloseShop()
        {
            _shopUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
        
            _player.SpawnHumanForm();
            _player.SpawnCarForm();
            _player.SpawnHelicopterForm();
            _player.SpawnBoatForm();
            
            _player.CreatePlayerForms();
        }
    }
}