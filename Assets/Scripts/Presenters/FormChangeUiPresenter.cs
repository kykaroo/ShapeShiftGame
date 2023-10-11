using Ui;
using Zenject;

namespace Presenters
{
    using Player = Player.Player;
    public class FormChangeUiPresenter
    {
        private readonly FormChangeUi _formChangeUi;

        [Inject]
        public FormChangeUiPresenter(FormChangeUi formChangeUi, Player player)
        {
            _formChangeUi = formChangeUi;
            
            
            _formChangeUi.OnHumanFormButtonClick += player.SetHumanFormState;
            _formChangeUi.OnCarFormButtonClick += player.SetCarFormState;
            _formChangeUi.OnHelicopterFormButtonClick += player.SetHelicopterFormState;
            _formChangeUi.OnBoatFormButtonClick += player.SetBoatFormState;
        }
    }
}