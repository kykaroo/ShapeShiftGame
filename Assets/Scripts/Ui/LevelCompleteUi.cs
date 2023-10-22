using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class LevelCompleteUi : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;

        public event Action OnPlayAgainButtonClick;

        private void PlayAgainButtonClick() => OnPlayAgainButtonClick?.Invoke();

        private void Awake()
        {
            playAgainButton.onClick.AddListener(PlayAgainButtonClick);
        }
    }
}