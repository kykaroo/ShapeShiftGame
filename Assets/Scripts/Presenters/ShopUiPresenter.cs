using Data;
using Data.PlayerGameData;
using Ui;

namespace Presenters
{
    public class ShopUiPresenter
    {
        private ShopUi _shopUi;
        private StartUi _startUi;
        private PersistentPlayerGameData _persistentPlayerGameData;
        private FortuneWheelUi _fortuneWheelUi;
        private IDataProvider<PersistentPlayerGameData> _gameDataProvider;
        private Player _player;

        public ShopUiPresenter(ShopUi shopUi, StartUi startUi, PersistentPlayerGameData persistentPlayerGameData,
            FortuneWheelUi fortuneWheelUi, IDataProvider<PersistentPlayerGameData> gameDataProvider, Player player)
        {
            _shopUi = shopUi;
            _startUi = startUi;
            _persistentPlayerGameData = persistentPlayerGameData;
            _fortuneWheelUi = fortuneWheelUi;
            _gameDataProvider = gameDataProvider;
            _player = player;
            
            _shopUi.OnDeleteSaveButtonClick += () => _persistentPlayerGameData = new();
            _shopUi.OnBackButtonClick += CloseShop;
        }

        private void CloseShop()
        {
            _shopUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
        
            //TODO проверка на изменение скинов
        }
    }
}