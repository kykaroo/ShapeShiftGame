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
        [SerializeField] private Button freeSpinWheelButton;
        [SerializeField] private Button paidSpinWheelButton;

        public event Action OnBackButtonClick;

        private void BackButtonClick() => OnBackButtonClick?.Invoke();

        private void Awake()
        {
            backButton.onClick.AddListener(BackButtonClick);
            freeSpinWheelButton.onClick.AddListener(wheelManager.FreeSpinButtonClick);
            // paidSpinWheelButton.onClick.AddListener(wheelManager.PaidSpinButtonClick); Раскомментировать для добавления круток а деньги 
            wheelManager.OnChangeButtonVisibility += UpdateButtonsVisibility;
            paidSpinWheelButton.gameObject.SetActive(false); //Убрать для добавления круток за деньги
        }
        
        private void UpdateButtonsVisibility(bool isWheelSpinning, bool canClaimFreeReward)
        {
            if (isWheelSpinning)
            {
                freeSpinWheelButton.gameObject.SetActive(false);
                // paidSpinWheelButton.gameObject.SetActive(false); Раскомментировать для добавления круток за деньги 
                return;
            }
            
            freeSpinWheelButton.gameObject.SetActive(canClaimFreeReward);
            // paidSpinWheelButton.gameObject.SetActive(!timer.CanClaimFreeReward); Раскомментировать для добавления круток за деньги 
        }
    }
}