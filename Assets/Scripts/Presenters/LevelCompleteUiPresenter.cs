using Ai;
using Audio;
using FormStateMachine.States;
using Level;
using Ui;
using Zenject;

namespace Presenters
{
    public class LevelCompleteUiPresenter
    {
        private readonly LevelCompleteUi _levelCompleteUi;
        private readonly LevelGenerator _levelGenerator;
        private readonly Ui.ProgressBar.LevelProgressBar _levelProgressBar;
        private readonly FormChangeUi _formChangeUi;
        private readonly StartUi _startUi;
        private readonly Player.Player _player;
        private readonly EnemyHandler _enemyHandler;
        private readonly DefeatUi _defeatUi;
        private readonly AudioManager _audioManager;
        private readonly YG.YandexGame _yandexGame;

        private bool _firstLevelGenerate = true;

        [Inject]
        public LevelCompleteUiPresenter(LevelCompleteUi levelCompleteUi, LevelGenerator levelGenerator,
            Ui.ProgressBar.LevelProgressBar levelProgressBar, Player.Player player,
            EnemyHandler enemyHandler, FormChangeUi formChangeUi, StartUi startUi, DefeatUi defeatUi, 
            AudioManager audioManager, YG.YandexGame yandexGame)
        {
            _levelCompleteUi = levelCompleteUi;
            _levelGenerator = levelGenerator;
            _levelProgressBar = levelProgressBar;
            _player = player;
            _enemyHandler = enemyHandler;
            _formChangeUi = formChangeUi;
            _startUi = startUi;
            _defeatUi = defeatUi;
            _audioManager = audioManager;
            _yandexGame = yandexGame;

            _levelCompleteUi.OnPlayAgainButtonClick += RestartLevel;
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
            _audioManager.isLevelEnd = false;

            foreach (var formStateMachine in _enemyHandler.AiFormStateMachine)
            {
                formStateMachine.SetState<NoneFormState>();
            }

            _player.MoveToStartPosition();
            _enemyHandler.RestartAllBots();
            
            _levelProgressBar.SetLevelLenght(_levelGenerator.LevelStartZ, _levelGenerator.LevelEndZ);

            _formChangeUi.gameObject.SetActive(false);
            _levelCompleteUi.gameObject.SetActive(false);
            _defeatUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
            _player.CameraHolder.ResetCamera();

            if (_firstLevelGenerate)
            {
                _yandexGame._RewardedShow(0);
                _firstLevelGenerate = false;
                return;
            }
            
            _yandexGame._RewardedShow(1);
        }

        private void LevelComplete(bool playerVictory)
        {
            _player.CameraHolder.ReleaseCamera();
            _formChangeUi.gameObject.SetActive(false);
            _audioManager.isLevelEnd = true;

            if (playerVictory)
            {
                _levelCompleteUi.gameObject.SetActive(true);
                return;
            }
            
            _defeatUi.gameObject.SetActive(true);
        }
    }
}