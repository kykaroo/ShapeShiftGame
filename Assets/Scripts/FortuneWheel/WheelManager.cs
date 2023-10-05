using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Wallet;
using Random = UnityEngine.Random;

namespace FortuneWheel
{
    public class WheelManager : MonoBehaviour
    {
        [SerializeField] private Button spinWheelButton;
        [SerializeField] private Transform wheel;
        [SerializeField] private int fullTurnovers;
        [SerializeField] private WalletView walletView;
        [SerializeField] private WheelSector[] wheelSectors;
        [SerializeField] private Timer timer;

        private float _currentRotation;
        private float _currentRotationSpeed;
        private bool _isWheelSpinning;
        private Wallet.Wallet _wallet;
        private WheelSector _finalSector;
        private float _finalAngle;
        private int[] _wheelSectorsAngles;
        private float _currentLerpRotationTime;
        private float _startAngle;
        private IDataProvider _gameDataProvider;
        private Shop.Shop _shop;
        private IPersistentPlayerData _persistentPlayerData;

        private const float MaxLerpRotationTime = 4f;

        public Timer Timer => timer;

        public event Action OnSpinStart;
        public event Action OnSpinEnd;

        public void Initialize(IDataProvider gameDataProvider, IPersistentPlayerData persistentPlayerData, Wallet.Wallet wallet, Shop.Shop shop)
        {
            _gameDataProvider = gameDataProvider;
            _persistentPlayerData = persistentPlayerData;
            _wallet = wallet;
            _shop = shop;
            timer.Initialize(_persistentPlayerData, _gameDataProvider);
            
            if (timer.CanClaimReward)
            {
                spinWheelButton.enabled = false;
            }

            InitializeWallet(wallet);

            InitializeWheelsSectors();
        }

        public void InitializeWallet(Wallet.Wallet wallet)
        {
            walletView.Initialize(wallet);
        }

        private void InitializeWheelsSectors()
        {
            foreach (var sector in wheelSectors)
            {
                sector.rewardText.text = sector.text;
                sector.rewardImage.sprite = sector.image;
            }
        }

        private void Awake()
        {
            spinWheelButton.onClick.AddListener(ToggleWheelSpin);

            _wheelSectorsAngles = new int[wheelSectors.Length];
            for (var i = 0; i < _wheelSectorsAngles.Length; i++)
            {
                _wheelSectorsAngles[i] = 360 / wheelSectors.Length * (i + 1);
            }
        }

        private void Update()
        {
            UpdateTimer();
            if (!_isWheelSpinning) return;

            _currentLerpRotationTime += Time.deltaTime;

            if (_currentLerpRotationTime > MaxLerpRotationTime || wheel.transform.eulerAngles.z == _finalAngle) {
                _currentLerpRotationTime = MaxLerpRotationTime;
                _isWheelSpinning = false;
                _startAngle = _finalAngle % 360;

                GetReward();
                OnSpinEnd?.Invoke();
                spinWheelButton.enabled = true;
            } else {
                var t = _currentLerpRotationTime / MaxLerpRotationTime;

                t = t * t * t * (t * (6f * t - 15f) + 10f);

                var angle = Mathf.Lerp (_startAngle, _finalAngle, t);
                wheel.transform.eulerAngles = new(0, 0, angle);	
            }
        }

        private void UpdateTimer()
        {
            spinWheelButton.enabled = timer.CanClaimReward;
        }

        private void ToggleWheelSpin()
        {
            OnSpinStart?.Invoke();
            spinWheelButton.enabled = false;
            _currentLerpRotationTime = 0;
            _isWheelSpinning = true;

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
                    _shop.OpenSkinsChecker.Visit(_finalSector.itemReward);
                    if (_shop.OpenSkinsChecker.IsOpened)
                    { 
                        _wallet.AddCoins(_finalSector.rewardMoneyValueIfItemOpened);
                        return;   
                    }
                    _shop.SkinUnlocker.Visit(_finalSector.itemReward);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            timer.OnRewardEarned();
            _gameDataProvider.Save();
        }
    }
}