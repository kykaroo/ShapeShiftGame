using Ai;
using FormStateMachine.States;
using Level;
using Ui;
using Zenject;

namespace Presenters
{
    public class VictoryUiPresenter
    {
        private readonly VictoryUi _victoryUi;
        private readonly LevelGenerator _levelGenerator;
        private readonly Ui.ProgressBar.LevelProgressBar _levelProgressBar;
        private readonly FormChangeUi _formChangeUi;
        private readonly StartUi _startUi;
        private readonly Player.Player _player;
        private readonly EnemyHandler _enemyHandler;
        private readonly DefeatUi _defeatUi;

        [Inject]
        public VictoryUiPresenter(VictoryUi victoryUi, LevelGenerator levelGenerator,
            Ui.ProgressBar.LevelProgressBar levelProgressBar, Player.Player player,
            EnemyHandler enemyHandler, FormChangeUi formChangeUi, StartUi startUi, DefeatUi defeatUi)
        {
            _victoryUi = victoryUi;
            _levelGenerator = levelGenerator;
            _levelProgressBar = levelProgressBar;
            _player = player;
            _enemyHandler = enemyHandler;
            _formChangeUi = formChangeUi;
            _startUi = startUi;
            _defeatUi = defeatUi;

            _victoryUi.OnPlayAgainButtonClick += RestartLevel;
            _defeatUi.OnPlayAgainButtonClick += RestartLevel;
            RestartLevel();
        }

        private void RestartLevel()
        {
            _levelGenerator.ClearLevel();
            _levelGenerator.GenerateLevel();
            _levelGenerator.LevelEndTrigger.OnLevelComplete += LevelComplete;
        
            _player.SetNoneFormState();
            _player.ClearPoofParticleSystem();

            foreach (var formStateMachine in _enemyHandler.AiFormStateMachine)
            {
                formStateMachine.SetState<NoneFormState>();
            }

            _player.MoveToStartPosition();
            _enemyHandler.RestartAllBots();
            
            _levelProgressBar.SetLevelLenght(_levelGenerator.LevelStartZ, _levelGenerator.LevelEndZ);

            _formChangeUi.gameObject.SetActive(false);
            _victoryUi.gameObject.SetActive(false);
            _defeatUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
            _player.CameraHolder.ResetCamera();
        }

        private void LevelComplete(bool playerVictory)
        {
            _player.CameraHolder.ReleaseCamera();
            _formChangeUi.gameObject.SetActive(false);
            
            if (playerVictory)
            {
                _victoryUi.gameObject.SetActive(true);
                return;
            }
            
            _defeatUi.gameObject.SetActive(true);
        }
    }
}