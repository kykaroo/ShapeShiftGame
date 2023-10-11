using Ui;

namespace Presenters
{
    using Player = Player.Player;
    public class ShopUiPresenter
    {
        private readonly ShopUi _shopUi;
        private readonly StartUi _startUi;
        private readonly Player _player;

        public ShopUiPresenter(ShopUi shopUi, StartUi startUi, Player player)
        {
            _shopUi = shopUi;
            _startUi = startUi;
            _player = player;

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