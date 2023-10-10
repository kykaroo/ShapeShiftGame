using System;
using Data;
using Data.PlayerOptionsData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui
{
    public class OptionsUi : MonoBehaviour
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Button backButton;
        [SerializeField] private Button musicMuteButton;
        [SerializeField] private Button sfxMuteButton;
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

        private PersistentPlayerOptionsData _persistentPlayerData;

        [Inject]
        public void Initialize(PersistentPlayerOptionsData persistentPlayerOptionsData)
        {
            _persistentPlayerData = persistentPlayerOptionsData;
            MusicSliderValueChanged(persistentPlayerOptionsData.MusicVolume);
            SfxSliderValueChanged(persistentPlayerOptionsData.SfxVolume);
            UpdateMusicMuteIcon();
            UpdateSfxMuteIcon();
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

        public void UpdateMusicMuteIcon()
        {
            if (_persistentPlayerData.MuteMusic)
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

        public void UpdateSfxMuteIcon()
        {
            if (_persistentPlayerData.MuteSfx)
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
    }
}