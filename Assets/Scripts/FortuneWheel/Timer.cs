using System;
using Data;
using Data.PlayerGameData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using Zenject;

namespace FortuneWheel
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private GameObject texts;
        [SerializeField] private SpinCooldownConfig spinCooldownConfig;
        
        private PersistentPlayerGameData _persistentPlayerData;
        private IDataProvider<PersistentPlayerGameData> _gameDataProvider;
        private bool _canClaimFreeReward;
        private TimeSpan _currentFreeSpinCooldown;
        private TimeSpan _freeSpinCooldown;
        private DateTime _cooldownExpireTime;
        private DateTime _currentTime;

        public bool CanClaimFreeReward => _canClaimFreeReward;

        public event Action OnDebugTimerChange;
        public event Action OnCanFreeSpinVariableChange;

        [Inject]
        public void Initialize(PersistentPlayerGameData persistentPlayerGameData, IDataProvider<PersistentPlayerGameData> gameDataProvider)
        {
            _persistentPlayerData = persistentPlayerGameData;
            _gameDataProvider = gameDataProvider;
            _freeSpinCooldown = new(spinCooldownConfig.freeSpinDaysCooldown,spinCooldownConfig.freeSpinHoursCooldown,
                spinCooldownConfig.freeSpinMinutesCooldown,spinCooldownConfig.freeSpinSecondsCooldown);
            
             _persistentPlayerData.LastClaimTime ??= DateTime.UtcNow.Subtract(_freeSpinCooldown);
             
            _cooldownExpireTime = _persistentPlayerData.LastClaimTime.Value;
            _cooldownExpireTime = _cooldownExpireTime.Add(_freeSpinCooldown);
            Update();
        }

        private void Update()
        {
            _currentTime = DateTime.UtcNow;
            UpdateRewardState();
            UpdateTimerText();
        }

        private void UpdateRewardState()
        {
            _currentFreeSpinCooldown = _cooldownExpireTime - _currentTime;
        }

        private void UpdateTimerText()
        {
            if (_currentFreeSpinCooldown.TotalMilliseconds > 0)
            {
                if (_canClaimFreeReward)
                {
                    _canClaimFreeReward = false;
                    OnCanFreeSpinVariableChange?.Invoke();
                }
                
                texts.SetActive(!_canClaimFreeReward);
                UpdateTimerValue();
                return;
            }

            if (_canClaimFreeReward == false)
            {
                _canClaimFreeReward = true;
                OnCanFreeSpinVariableChange?.Invoke();
            }
            
            _canClaimFreeReward = true;
            texts.SetActive(!_canClaimFreeReward);
        }

        private void UpdateTimerValue()
        {
            var dateTime = new TimeSpan(_currentFreeSpinCooldown.Days, _currentFreeSpinCooldown.Hours,_currentFreeSpinCooldown.Minutes,
                _currentFreeSpinCooldown.Seconds);

            timer.text = dateTime.ToString();
        }

        public void OnRewardEarned()
        {
            _persistentPlayerData.LastClaimTime = _currentTime;
            _cooldownExpireTime = _persistentPlayerData.LastClaimTime.Value.Add(_freeSpinCooldown);
            UpdateRewardState();
        }

        public void SetCooldownTime(TimeSpan freeSpinCooldown)
        {
            _freeSpinCooldown = freeSpinCooldown;
        }

        public void SetCurrentCooldownTime(TimeSpan targetedCooldown)
        {
            if ((_currentFreeSpinCooldown - targetedCooldown).TotalMilliseconds < 0)
            {
                UpdateTimerValue();
            }
            
            OnRewardEarned();
            _currentFreeSpinCooldown = _currentFreeSpinCooldown.Subtract(_currentFreeSpinCooldown - targetedCooldown);
            _cooldownExpireTime = _currentTime.Add(_currentFreeSpinCooldown);
            _persistentPlayerData.LastClaimTime = _currentTime.Subtract(_freeSpinCooldown - _currentFreeSpinCooldown);
            _gameDataProvider.Save();
            UpdateTimerText();
            OnDebugTimerChange?.Invoke();
        }
    }
}