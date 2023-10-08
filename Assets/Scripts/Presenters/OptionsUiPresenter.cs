using Audio;
using Ui;
using Zenject;

namespace Presenters
{
    public class OptionsUiPresenter
    {
        private readonly AudioManager _audioManager;
        private readonly OptionsUi _optionsUi;
        private readonly StartUi _startUi;
        
        [Inject]
        public OptionsUiPresenter(AudioManager audioManager, OptionsUi optionsUi, StartUi startUi)
        {
            _audioManager = audioManager;
            _optionsUi = optionsUi;
            _startUi = startUi;
            
            _audioManager.OnNewTrackPlay += (songName) => _optionsUi.UpdateCurrentTrack(songName);
            
            _optionsUi.OnMusicSliderValueChanged += ChangeMusicVolume;
            _optionsUi.OnSfxSliderValueChanged += ChangeSfxVolume;
            _optionsUi.OnMusicMuteButtonClick += ToggleMusicMute;
            _optionsUi.OnSfxMuteButtonClick += ToggleSfxMute;
            _optionsUi.OnNextTrackButtonClicked += SetNextMusicTrack;
            _optionsUi.OnBackButtonClick += CloseOptionsWindow;
        }
        
        private void ChangeSfxVolume(float newValue)
        {
            _audioManager.ChangeSfxVolume(newValue);
        }

        private void ChangeMusicVolume(float newValue)
        {
            _audioManager.ChangeMusicVolume(newValue);
        }

        private void SetNextMusicTrack()
        {
            _optionsUi.UpdateCurrentTrack(_audioManager.PlayAllMusic());
        }
        
        private void CloseOptionsWindow()
        {
            _audioManager.SaveSettings();
            _optionsUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
        }
        
        private void ToggleSfxMute()
        {
            _audioManager.ToggleSfx();
            _optionsUi.UpdateSfxMuteIcon();
        }

        private void ToggleMusicMute()
        {
            _audioManager.ToggleMusic();
            _optionsUi.UpdateMusicMuteIcon();
        }
    }
}