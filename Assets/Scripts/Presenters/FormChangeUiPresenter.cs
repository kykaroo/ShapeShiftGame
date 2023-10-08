using Ui;
using Zenject;

namespace Presenters
{
    public class FormChangeUiPresenter
    {
        private readonly FormChangeUi _formChangeUi;
        private readonly Player _player;
        
        [Inject]
        public FormChangeUiPresenter(FormChangeUi formChangeUi,
            Player player)
        {
            _formChangeUi = formChangeUi;
            _player = player;
            
            _formChangeUi.OnHumanFormButtonClick += () => _player.SetHumanFormState();
            _formChangeUi.OnCarFormButtonClick += () => _player.SetCarFormState();
            _formChangeUi.OnHelicopterFormButtonClick += () => _player.SetHelicopterFormState();
            _formChangeUi.OnBoatFormButtonClick += () => _player.SetBoatFormState();
        }
    }
}