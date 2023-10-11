using Audio;
using Data;
using Data.PlayerGameData;
using FortuneWheel;
using Ui;
using UnityEngine;
using Zenject;

namespace Presenters
{
    public class DebugUiPresenter : ITickable
    {
        private readonly DebugUi _debugUi;
        private readonly AudioManager _audioManager;
        private Timer _timer;
        private Wallet.Wallet _wallet;
        private readonly IDataProvider<PersistentGameData> _gameDataProvider;

        [Inject]
        public DebugUiPresenter(DebugUi debugUi, AudioManager audioManager, IDataProvider<PersistentGameData> gameDataProvider)
        {
            _debugUi = debugUi;
            _audioManager = audioManager;
            _gameDataProvider = gameDataProvider;
            
            _audioManager.OnNewTrackPlay += (songName) => _debugUi.UpdateCurrentTrack(songName);
            _debugUi.OnNextTrackButtonClicked += () => _debugUi.UpdateCurrentTrack(_audioManager.PlayAllMusic());
            _debugUi.OnDeleteSaveButtonClicked += DeleteSave;
        }

        private void DeleteSave()
        {
            _gameDataProvider.DeleteSave();
            _gameDataProvider.GetData();
        }

        public void Tick()
        {
            if (!Input.GetKeyDown(KeyCode.P)) return;

            if (_debugUi.gameObject.activeSelf)
            {
                _debugUi.gameObject.SetActive(false);
                return;
            }
        
            _debugUi.PrepareButtons();
            _debugUi.gameObject.SetActive(true);
        }
    }
}