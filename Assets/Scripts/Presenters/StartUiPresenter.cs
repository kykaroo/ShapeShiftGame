﻿using Ai;
using FormStateMachine.States;
using Ui;
using Zenject;

namespace Presenters
{
    public class StartUiPresenter
    {
        private readonly StartUi _startUi;
        private readonly OptionsUi _optionsUi;
        private readonly FortuneWheelUi _fortuneWheelUi;
        private readonly ShopUi _shopUi;
        private readonly LevelCompleteUi _levelCompleteUi;
        private readonly FormChangeUi _formChangeUi;
        private readonly Player.Player _player;
        private readonly EnemyHandler _enemyHandler;
        private readonly AiDifficulty[] _aiDifficulties;
        private readonly EnemyAi[] _enemyAis;

        [Inject]
        public StartUiPresenter(StartUi startUi, OptionsUi optionsUi, FortuneWheelUi fortuneWheelUi, ShopUi shopUi,
            LevelCompleteUi levelCompleteUi, FormChangeUi formChangeUi, Player.Player player, EnemyHandler enemyHandler, 
            AiDifficulty[] aiDifficulties, EnemyAi[] enemyAis)
        {
            _startUi = startUi;
            _optionsUi = optionsUi;
            _fortuneWheelUi = fortuneWheelUi;
            _shopUi = shopUi;
            _levelCompleteUi = levelCompleteUi;
            _formChangeUi = formChangeUi;
            _player = player;
            _enemyHandler = enemyHandler;
            _aiDifficulties = aiDifficulties;
            _enemyAis = enemyAis;

            _startUi.OnStartButtonClick += OnStartGame;
            _startUi.OnShopButtonClick += OpenShop;
            _startUi.OnOptionsButtonClicked += OpenOptionsWindow;
            _startUi.OnFortuneWheelButtonClick += OpenFortuneWheelWindow;
            _startUi.OnDifficultyChanged += ChangeDifficulty;
        }

        private void OnStartGame()
        {
            _startUi.gameObject.SetActive(false);
            _levelCompleteUi.gameObject.SetActive(false);
            _formChangeUi.gameObject.SetActive(true);
        
            _player.SetHumanFormState();

            foreach (var formStateMachine in _enemyHandler.AiFormStateMachine)
            {
                formStateMachine.SetState<HumanFormState>();
            }
        }

        private void OpenShop()
        {
            _startUi.gameObject.SetActive(false);
            _shopUi.gameObject.SetActive(true);
        }
    
        private void OpenOptionsWindow()
        {
            _optionsUi.gameObject.SetActive(true);
            _startUi.gameObject.SetActive(false);
        }

        public void OpenFortuneWheelWindow()
        {
            _startUi.gameObject.SetActive(false);
            _fortuneWheelUi.gameObject.SetActive(true);
        }
    
        private void ChangeDifficulty(int value)
        {
            switch (value)
            {
                case 0:
                    SetAiDifficulty(AiDifficulty.Easy);
                    break;
                case 1:
                    SetAiDifficulty(AiDifficulty.Medium);
                    break;
                case 2:
                    SetAiDifficulty(AiDifficulty.Hard);
                    break;
                case 3:
                    SetAiDifficulty(AiDifficulty.Insane);
                    break;
            }
        }

        private void SetAiDifficulty(AiDifficulty difficulty)
        {
            for (var i = 0; i < _aiDifficulties.Length; i++)
            {
                _aiDifficulties[i] = difficulty;
                _enemyAis[i].SetDifficulty(difficulty);
            }
        }
    }
}