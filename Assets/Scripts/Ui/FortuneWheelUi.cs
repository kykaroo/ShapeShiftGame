using System;
using FortuneWheel;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wallet;

namespace Ui
{
    public class FortuneWheelUi : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        [SerializeField] private Button freeSpinWheelButton;
        [SerializeField] private Button paidSpinWheelButton;
        [SerializeField] private WalletView walletView;
        [SerializeField] private TextMeshProUGUI paidSpinPriceText;
        [SerializeField] private WheelSectorConfig[] wheelSectors;
        [SerializeField] private Timer timer;
        [SerializeField] private Image[] rewardImages;
        [SerializeField] private TextMeshProUGUI[] rewardTexts;
        [SerializeField] private Transform wheel;
        [SerializeField] private int fullTurnovers;

        public event Action OnBackButtonClick;
        public event Action OnFreeSpinButtonClick;
        public event Action OnPaidSpinButtonClick;

        public WalletView WalletView => walletView;

        public WheelSectorConfig[] WheelSectors => wheelSectors;

        public Timer Timer => timer;

        public Image[] RewardImages => rewardImages;

        public TextMeshProUGUI[] RewardTexts => rewardTexts;

        public Transform Wheel => wheel;

        public int FullTurnovers => fullTurnovers;

        private void BackButtonClick() => OnBackButtonClick?.Invoke();

        private void FreeSpinButtonClick() => OnFreeSpinButtonClick?.Invoke();
        private void PaidSpinButtonClick() => OnPaidSpinButtonClick?.Invoke();

        private void Awake()
        {
            backButton.onClick.AddListener(BackButtonClick);
            freeSpinWheelButton.onClick.AddListener(FreeSpinButtonClick);
            // paidSpinWheelButton.onClick.AddListener(PaidSpinButtonClick); Раскомментировать для добавления круток а деньги 
            paidSpinWheelButton.gameObject.SetActive(false); //Убрать для добавления круток за деньги
        }
        
        public void UpdateButtonsVisibility(bool isWheelSpinning, bool canClaimFreeReward)
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

        public void SetPriceText(string text)
        {
            paidSpinPriceText.text = text;
        }
    }
}