using System;
using System.Linq;
using Data;
using Data.PlayerGameData;
using Ui;
using UnityEngine;
using Wallet;
using Zenject;
using Random = UnityEngine.Random;

namespace FortuneWheel
{
    public class WheelManager : ITickable
    {
        private readonly WalletView _walletView;
        private int _paidSpinPrice;
        private float _currentRotation;
        private float _currentRotationSpeed;
        private bool _isWheelSpinning;
        private readonly Wallet.Wallet _wallet;
        private WheelSectorConfig _finalSectorConfig;
        private float _finalAngle;
        private readonly int[] _wheelSectorsAngles;
        private float _currentLerpRotationTime;
        private float _startAngle;
        private readonly IDataProvider<PersistentGameData> _gameDataProvider;
        private readonly ShopUi _shopUi;
        private bool _isFreeSpin;
        private readonly FortuneWheelUi _fortuneWheelUi;

        private const float MaxLerpRotationTime = 4f;

        [Inject]
        public WheelManager(IDataProvider<PersistentGameData> gameDataProvider, Wallet.Wallet wallet, ShopUi shopUi, FortuneWheelUi fortuneWheelUi)
        {
            _gameDataProvider = gameDataProvider;
            _wallet = wallet;
            _shopUi = shopUi;
            _fortuneWheelUi = fortuneWheelUi;

            _fortuneWheelUi.WalletView.Initialize(wallet);
            
            InitializeWheelsSectors();
            UpdateButtonsVisibility();
            
            _fortuneWheelUi.SetPriceText(_paidSpinPrice.ToString());
            _fortuneWheelUi.Timer.OnCanFreeSpinVariableChange += UpdateButtonsVisibility;
            _fortuneWheelUi.Timer.OnDebugTimerChange += UpdateButtonsVisibility;

            _fortuneWheelUi.OnFreeSpinButtonClick += FreeSpinButtonClick;
            _fortuneWheelUi.OnPaidSpinButtonClick += PaidSpinButtonClick;

            _wheelSectorsAngles = new int[fortuneWheelUi.WheelSectors.Length];
            for (var i = 0; i < _wheelSectorsAngles.Length; i++)
            {
                _wheelSectorsAngles[i] = 360 / fortuneWheelUi.WheelSectors.Length * (i + 1);
            }
        }

        private void InitializeWheelsSectors()
        {
            for (var i = 0; i < _fortuneWheelUi.WheelSectors.Length; i++)
            {
                var sector = _fortuneWheelUi.WheelSectors[i];
                
                sector.RewardImagePlaceholder = _fortuneWheelUi.RewardImages[i];
                sector.RewardImagePlaceholder.sprite = sector.image;

                sector.RewardTextPlaceholder = _fortuneWheelUi.RewardTexts[i];
                sector.RewardTextPlaceholder.text = sector.rewardText;
            }
        }

        private void UpdateButtonsVisibility() =>
            _fortuneWheelUi.UpdateButtonsVisibility(_isWheelSpinning, _fortuneWheelUi.Timer.CanClaimFreeReward);

        private void FreeSpinButtonClick()
        {
            _isFreeSpin = true;
            ToggleWheelSpin();
        }

        private void PaidSpinButtonClick()
        {
            if (!_wallet.IsEnough(_paidSpinPrice)) return;
            
            _wallet.Spend(_paidSpinPrice);
            _isFreeSpin = false;
            ToggleWheelSpin();
        }

        public void Tick()
        {
            if (!_isWheelSpinning) return;

            _currentLerpRotationTime += Time.deltaTime;

            if (_currentLerpRotationTime > MaxLerpRotationTime || _fortuneWheelUi.Wheel.transform.eulerAngles.z == _finalAngle) {
                _currentLerpRotationTime = MaxLerpRotationTime;
                _isWheelSpinning = false;
                _startAngle = _finalAngle % 360;

                GetReward();
                UpdateButtonsVisibility();
                
            } else {
                var t = _currentLerpRotationTime / MaxLerpRotationTime;

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                var angle = Mathf.Lerp (_startAngle, _finalAngle, t);
                _fortuneWheelUi.Wheel.transform.eulerAngles = new(0, 0, angle);	
            }
        }

        private void ToggleWheelSpin()
        {
            _currentLerpRotationTime = 0;
            _isWheelSpinning = true;
            UpdateButtonsVisibility();

            var randomNumber = Random.Range(1, _fortuneWheelUi.WheelSectors.Sum(sector => sector.probabilityWeight));
            float cumulativeProbability = 0;
            var randomFinalAngle = _wheelSectorsAngles[0];
            
            _finalSectorConfig = _fortuneWheelUi.WheelSectors[0];

            for (var i = 0; i < _fortuneWheelUi.WheelSectors.Length; i++) {
                cumulativeProbability += _fortuneWheelUi.WheelSectors[i].probabilityWeight;

                if (randomNumber > cumulativeProbability) continue;
                
                randomFinalAngle = _wheelSectorsAngles[i];
                _finalSectorConfig = _fortuneWheelUi.WheelSectors[i];
                break;
            }
            
            _finalAngle = _fortuneWheelUi.FullTurnovers * 360 + randomFinalAngle;
        }

        private void GetReward()
        {
            switch (_finalSectorConfig.rewardType)
            {
                case RewardType.Money:
                    _wallet.AddCoins(_finalSectorConfig.moneyRewardValue);
                    break;
                case RewardType.Skin:
                    _shopUi.OpenSkinsChecker.Visit(_finalSectorConfig.itemReward);
                    if (_shopUi.OpenSkinsChecker.IsOpened)
                    { 
                        _wallet.AddCoins(_finalSectorConfig.rewardMoneyValueIfItemOpened);
                        break;   
                    }
                    _shopUi.SkinUnlocker.Visit(_finalSectorConfig.itemReward);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (_isFreeSpin)
            { 
                _fortuneWheelUi.Timer.OnRewardEarned();
            }
            
            UpdateButtonsVisibility();
            _gameDataProvider.Save();
        }

        public void ChangePrice(int newPaidSpinPrice)
        {
            _paidSpinPrice = newPaidSpinPrice;
            _fortuneWheelUi.SetPriceText(_paidSpinPrice.ToString());
        }
    }
}