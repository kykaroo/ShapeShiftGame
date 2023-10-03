using System;
using FortuneWheel;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class FortuneWheelUi : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private WheelManager wheelManager;

        public WheelManager WheelManager => wheelManager;

        public event Action OnBackButtonClick;

        private void BackButtonClick() => OnBackButtonClick?.Invoke();

        private void Awake()
        {
            backButton.onClick.AddListener(BackButtonClick);
            wheelManager.OnSpinStart += () => backButton.enabled = false;
            wheelManager.OnSpinEnd += () => backButton.enabled = true;
        }
    }
}