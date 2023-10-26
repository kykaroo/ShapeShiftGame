using Ai;
using Audio;
using FormStateMachine.States;
using Level;
using Ui;
using YG;
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
        private readonly AudioManager _audioManager;
        private readonly ReviewYG _reviewYg;
        

        [Inject]
        public LevelCompleteUiPresenter(LevelCompleteUi levelCompleteUi, LevelGenerator levelGenerator,
            Ui.ProgressBar.LevelProgressBar levelProgressBar, Player.Player player,
            EnemyHandler enemyHandler, FormChangeUi formChangeUi, StartUi startUi, 
            AudioManager audioManager, YandexGame yandexGame, ReviewYG reviewYg)
        {
            _levelCompleteUi = levelCompleteUi;
            _levelGenerator = levelGenerator;
            _levelProgressBar = levelProgressBar;
            _player = player;
            _enemyHandler = enemyHandler;
            _formChangeUi = formChangeUi;
            _startUi = startUi;
            _audioManager = audioManager;
            _reviewYg = reviewYg;

            _levelCompleteUi.OnPlayAgainButtonClick += RestartLevel;
            _levelCompleteUi.OnConfirmRatingButtonClick += OpenReview;
            _levelCompleteUi.OnDoubleRewardButtonClick += () => yandexGame._RewardedShow(1);
            
            _reviewYg.ReviewAvailable.AddListener(_levelCompleteUi.ShowReviewButton);
            _reviewYg.ReviewNotAvailable.AddListener(_levelCompleteUi.HideReviewButton);

            RestartLevel();
        }

        private void OpenReview()
        {
            _reviewYg.ReviewShow();
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
            _startUi.gameObject.SetActive(true);
            _player.CameraHolder.ResetCamera();
        }

        private void LevelComplete(bool isPlayerWin)
        {
            _player.CameraHolder.ReleaseCamera();
            _formChangeUi.gameObject.SetActive(false);
            _audioManager.isLevelEnd = true;
            _levelCompleteUi.SetLabel(isPlayerWin);
            _levelCompleteUi.gameObject.SetActive(true);
        }
    }
}