using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Ui
{
    public class LevelCompleteUi : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button confirmRating;
        [SerializeField] private GameObject victoryLabel;
        [SerializeField] private GameObject defeatLabel;

        private YandexGame _yandexGame;

        public event Action OnPlayAgainButtonClick;
        public event Action OnConfirmRatingButtonClick;

        private void PlayAgainButtonClick() => OnPlayAgainButtonClick?.Invoke();
        private void ConfirmRatingButtonClick() => OnConfirmRatingButtonClick?.Invoke(); 

        private void Awake()
        {
            playAgainButton.onClick.AddListener(PlayAgainButtonClick);
            confirmRating.onClick.AddListener(ConfirmRatingButtonClick);
        }

        public void ShowReviewButton()
        {
            confirmRating.gameObject.SetActive(true);
        }
        
        public void HideReviewButton()
        {
            confirmRating.gameObject.SetActive(false);
        }

        public void SetLabel(bool isPlayerWin)
        {
            victoryLabel.SetActive(isPlayerWin);
            defeatLabel.SetActive(!isPlayerWin);
        }
    }
}