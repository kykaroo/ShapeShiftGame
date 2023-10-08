using System;
using System.Linq;
using Data;
using Data.PlayerGameData;
using TMPro;
using Ui;
using UnityEngine;
using Wallet;
using Zenject;
using Random = UnityEngine.Random;

namespace FortuneWheel
{
    public class WheelManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI paidSpinPriceText;
        [SerializeField] private Transform wheel;
        [SerializeField] private int fullTurnovers;
        [SerializeField] private WalletView walletView;
        [SerializeField] private WheelSector[] wheelSectors;
        [SerializeField] private Timer timer;
        [SerializeField] private int paidSpinPrice;

        private float _currentRotation;
        private float _currentRotationSpeed;
        private bool _isWheelSpinning;
        private Wallet.Wallet _wallet;
        private WheelSector _finalSector;
        private float _finalAngle;
        private int[] _wheelSectorsAngles;
        private float _currentLerpRotationTime;
        private float _startAngle;
        private IDataProvider<PersistentPlayerGameData> _gameDataProvider;
        private ShopUi _shopUi;
        private bool _isFreeSpin;

        private const float MaxLerpRotationTime = 4f;

        public Timer Timer => timer;

        public event Action<bool, bool> OnChangeButtonVisibility;

        [Inject]
        public void Initialize(IDataProvider<PersistentPlayerGameData> gameDataProvider, Wallet.Wallet wallet, ShopUi shopUi)
        {
            _gameDataProvider = gameDataProvider;
            _wallet = wallet;
            _shopUi = shopUi;

            walletView.Initialize(wallet);
            InitializeWheelsSectors();
            UpdateButtonsVisibility();
        }

        private void InitializeWheelsSectors()
        {
            foreach (var sector in wheelSectors)
            {
                sector.rewardText.text = sector.text;
                sector.rewardImage.sprite = sector.image;
            }
        }

        private void UpdateButtonsVisibility() =>
            OnChangeButtonVisibility?.Invoke(_isWheelSpinning, timer.CanClaimFreeReward);

        private void Awake()
        {
            paidSpinPriceText.text = paidSpinPrice.ToString();
            timer.OnCanFreeSpinVariableChange += UpdateButtonsVisibility;
            timer.OnDebugTimerChange += UpdateButtonsVisibility;

            _wheelSectorsAngles = new int[wheelSectors.Length];
            for (var i = 0; i < _wheelSectorsAngles.Length; i++)
            {
                _wheelSectorsAngles[i] = 360 / wheelSectors.Length * (i + 1);
            }
        }

        public void FreeSpinButtonClick()
        {
            _isFreeSpin = true;
            ToggleWheelSpin();
        }

        public void PaidSpinButtonClick()
        {
            if (!_wallet.IsEnough(paidSpinPrice)) return;
            
            _wallet.Spend(paidSpinPrice);
            _isFreeSpin = false;
            ToggleWheelSpin();
        }

        private void Update()
        {
            if (!_isWheelSpinning) return;

            _currentLerpRotationTime += Time.deltaTime;

            if (_currentLerpRotationTime > MaxLerpRotationTime || wheel.transform.eulerAngles.z == _finalAngle) {
                _currentLerpRotationTime = MaxLerpRotationTime;
                _isWheelSpinning = false;
                _startAngle = _finalAngle % 360;

                GetReward();
                UpdateButtonsVisibility();
                
            } else {
                var t = _currentLerpRotationTime / MaxLerpRotationTime;

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                var angle = Mathf.Lerp (_startAngle, _finalAngle, t);
                wheel.transform.eulerAngles = new(0, 0, angle);	
            }
        }

        private void ToggleWheelSpin()
        {
            _currentLerpRotationTime = 0;
            _isWheelSpinning = true;
            UpdateButtonsVisibility();

            var randomNumber = Random.Range(1, wheelSectors.Sum(sector => sector.probabilityWeight));
            float cumulativeProbability = 0;
            var randomFinalAngle = _wheelSectorsAngles[0];
            
            _finalSector = wheelSectors[0];

            for (var i = 0; i < wheelSectors.Length; i++) {
                cumulativeProbability += wheelSectors[i].probabilityWeight;

                if (randomNumber > cumulativeProbability) continue;
                
                randomFinalAngle = _wheelSectorsAngles[i];
                _finalSector = wheelSectors[i];
                break;
            }
            
            _finalAngle = fullTurnovers * 360 + randomFinalAngle;
        }

        private void GetReward()
        {
            switch (_finalSector.rewardType)
            {
                case RewardType.Money:
                    _wallet.AddCoins(int.Parse(_finalSector.text));
                    break;
                case RewardType.Skin:
                    _shopUi.OpenSkinsChecker.Visit(_finalSector.itemReward);
                    if (_shopUi.OpenSkinsChecker.IsOpened)
                    { 
                        _wallet.AddCoins(_finalSector.rewardMoneyValueIfItemOpened);
                        break;   
                    }
                    _shopUi.SkinUnlocker.Visit(_finalSector.itemReward);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_isFreeSpin)
            { 
                timer.OnRewardEarned();
            }
            
            UpdateButtonsVisibility();
            _gameDataProvider.Save();
        }

        public void ChangePrice(int newPaidSpinPrice)
        {
            paidSpinPrice = newPaidSpinPrice;
            paidSpinPriceText.text = paidSpinPrice.ToString();
        }
    }
}