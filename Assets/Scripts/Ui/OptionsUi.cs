using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class OptionsUi : MonoBehaviour
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Button backButton;
        [SerializeField] private Button musicMuteButton;
        [SerializeField] private Button sfxMuteButton;
        [SerializeField] private Button nextTrackButton;
        [SerializeField] private TextMeshProUGUI currentTrackText;
        [Header("Scene images")]
        [SerializeField] private Image musicSliderHandleImage;
        [SerializeField] private Image sfxSliderHandleImage;
        [SerializeField] private Image musicMuteButtonImage;
        [SerializeField] private Image sfxMuteButtonImage;
        [Header("Sprites On")]
        [SerializeField] private Sprite volumeSliderHandleOnSprite;
        [SerializeField] private Sprite musicMuteOnSprite;
        [SerializeField] private Sprite sfxMuteOnSprite;
        [Header("Sprites Off")]
        [SerializeField] private Sprite volumeSliderHandleOffSprite;
        [SerializeField] private Sprite musicMuteOffSprite;
        [SerializeField] private Sprite sfxMuteOffSprite;

        public event Action<float> OnMusicSliderValueChanged;
        public event Action<float> OnSfxSliderValueChanged;
        public event Action OnBackButtonClick;
        public event Action OnMusicMuteButtonClick;
        public event Action OnSfxMuteButtonClick;
        public event Action OnNextTrackButtonClicked;
        
        private void NextTrackButtonClick() => OnNextTrackButtonClicked?.Invoke();

        public void Initialize(IPersistentPlayerData persistentPlayerData)
        {
            nextTrackButton.onClick.AddListener(NextTrackButtonClick);
            MusicSliderValueChanged(persistentPlayerData.PlayerOptionsData.MusicVolume);
            SfxSliderValueChanged(persistentPlayerData.PlayerOptionsData.SfxVolume);
        }
        
        private void MusicSliderValueChanged(float value)
        {
            OnMusicSliderValueChanged?.Invoke(value);

            if (musicVolumeSlider.value == 0)
            {
                musicSliderHandleImage.sprite = volumeSliderHandleOffSprite;
                return;
            }
            
            musicSliderHandleImage.sprite = volumeSliderHandleOnSprite;
        }

        private void SfxSliderValueChanged(float value)
        {
            OnSfxSliderValueChanged?.Invoke(value);
            
            if (sfxVolumeSlider.value == 0)
            {
                sfxSliderHandleImage.sprite = volumeSliderHandleOffSprite;
                return;
            }
            
            sfxSliderHandleImage.sprite = volumeSliderHandleOnSprite;
        }

        private void BackButtonClick() => OnBackButtonClick?.Invoke();
        private void MusicMuteButtonClick()
        {
            OnMusicMuteButtonClick?.Invoke();
        }

        public void UpdateMusicMuteIcon(IPersistentPlayerData persistentPlayerData)
        {
            if (persistentPlayerData.PlayerOptionsData.MuteMusic)
            {
                musicMuteButtonImage.sprite = musicMuteOffSprite;
                return;
            }

            musicMuteButtonImage.sprite = musicMuteOnSprite;
        }

        private void SfxMuteButtonClick()
        {
            OnSfxMuteButtonClick?.Invoke();
        }

        public void UpdateSfxMuteIcon(IPersistentPlayerData persistentPlayerData)
        {
            if (persistentPlayerData.PlayerOptionsData.MuteSfx)
            {
                sfxMuteButtonImage.sprite = sfxMuteOffSprite;
                return;
            }

            sfxMuteButtonImage.sprite = sfxMuteOnSprite;
        }

        private void Awake()
        {
            musicVolumeSlider.onValueChanged.AddListener(MusicSliderValueChanged);
            sfxVolumeSlider.onValueChanged.AddListener(SfxSliderValueChanged);
            backButton.onClick.AddListener(BackButtonClick);
            musicMuteButton.onClick.AddListener(MusicMuteButtonClick);
            sfxMuteButton.onClick.AddListener(SfxMuteButtonClick);
        }
        
        public void UpdateCurrentTrack(string trackName)
        {
            currentTrackText.text = $"Now playing: {trackName}";
        }
    }
}