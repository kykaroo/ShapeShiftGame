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
        private readonly LevelProgressBar.LevelProgressBar _levelProgressBar;
        private readonly FormChangeUi _formChangeUi;
        private readonly StartUi _startUi;
        private readonly Player _player;
        private readonly EnemyHandler _enemyHandler;

        [Inject]
        public VictoryUiPresenter(VictoryUi victoryUi, LevelGenerator levelGenerator,
            LevelProgressBar.LevelProgressBar levelProgressBar, Player player,
            EnemyHandler enemyHandler, FormChangeUi formChangeUi, StartUi startUi)
        {
            _victoryUi = victoryUi;
            _levelGenerator = levelGenerator;
            _levelProgressBar = levelProgressBar;
            _player = player;
            _enemyHandler = enemyHandler;
            _formChangeUi = formChangeUi;
            _startUi = startUi;

            _victoryUi.OnPlayAgainButtonClick += RestartLevel;
            RestartLevel();
        }

        private void RestartLevel()
        {
            _levelGenerator.ClearLevel();
            _levelGenerator.GenerateLevel();
            _levelGenerator.VictoryTrigger.OnLevelComplete += LevelComplete;
            _levelProgressBar.SetLevelLenght(_levelGenerator.LevelStartZ, _levelGenerator.LevelEndZ);
        
            _player.SetNoneFormState();
            _player.ClearPoofParticleSystem();

            foreach (var formStateMachine in _enemyHandler.AiFormStateMachine)
            {
                formStateMachine.SetState<NoneFormState>();
            }

            _player.MoveToStartPosition();

            _enemyHandler.RestartAllBots();

            _formChangeUi.gameObject.SetActive(false);
            _victoryUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
            _player.CameraHolder.ResetCamera();
        }
        
        private void LevelComplete()
        {
            _player.CameraHolder.ReleaseCamera();
            _formChangeUi.gameObject.SetActive(false);
            _victoryUi.gameObject.SetActive(true);
        }
    }
}