using System;
using Data;
using Data.PlayerGameData;
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
        
        private PlayerGameData _playerData;
        private IDataProvider<PlayerGameData> _gameDataProvider;
        private TimeSpan _currentFreeSpinCooldown;
        private TimeSpan _freeSpinCooldown;
        private DateTime _cooldownExpireTime;
        private DateTime _currentTime;

        public bool CanClaimFreeReward { get; private set; }

        public event Action OnDebugTimerChange;
        public event Action OnCanFreeSpinVariableChange;

        [Inject]
        public void Initialize(PlayerGameData playerGameData, IDataProvider<PlayerGameData> gameDataProvider)
        {
            _playerData = playerGameData;
            _gameDataProvider = gameDataProvider;
            _freeSpinCooldown = new(spinCooldownConfig.freeSpinDaysCooldown,spinCooldownConfig.freeSpinHoursCooldown,
                spinCooldownConfig.freeSpinMinutesCooldown,spinCooldownConfig.freeSpinSecondsCooldown);
            
             _playerData.LastClaimTime ??= DateTime.UtcNow.Subtract(_freeSpinCooldown);
             
            _cooldownExpireTime = _playerData.LastClaimTime.Value;
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
                if (CanClaimFreeReward)
                {
                    CanClaimFreeReward = false;
                    OnCanFreeSpinVariableChange?.Invoke();
                }
                
                texts.SetActive(!CanClaimFreeReward);
                UpdateTimerValue();
                return;
            }

            if (CanClaimFreeReward == false)
            {
                CanClaimFreeReward = true;
                OnCanFreeSpinVariableChange?.Invoke();
            }
            
            CanClaimFreeReward = true;
            texts.SetActive(!CanClaimFreeReward);
        }

        private void UpdateTimerValue()
        {
            var dateTime = new TimeSpan(_currentFreeSpinCooldown.Days, _currentFreeSpinCooldown.Hours,_currentFreeSpinCooldown.Minutes,
                _currentFreeSpinCooldown.Seconds);

            timer.text = dateTime.ToString();
        }

        public void OnRewardEarned()
        {
            _playerData.LastClaimTime = _currentTime;
            _cooldownExpireTime = _playerData.LastClaimTime.Value.Add(_freeSpinCooldown);
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
            _playerData.LastClaimTime = _currentTime.Subtract(_freeSpinCooldown - _currentFreeSpinCooldown);
            _gameDataProvider.Save();
            UpdateTimerText();
            OnDebugTimerChange?.Invoke();
        }
    }
}