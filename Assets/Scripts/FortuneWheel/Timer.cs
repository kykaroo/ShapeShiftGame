using System;
using Data;
using TMPro;
using UnityEngine;

namespace FortuneWheel
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer;
        [SerializeField] private GameObject texts;
        [SerializeField] private int freeSpinDaysCooldown;
        [SerializeField] private int freeSpinHoursCooldown;
        [SerializeField] private int freeSpinMinutesCooldown;
        [SerializeField] private int freeSpinSecondsCooldown;
        
        private IPersistentPlayerData _persistentPlayerData;
        private IDataProvider _gameDataProvider;
        private bool _canClaimReward;
        private TimeSpan _currentFreeSpinCooldown;
        private TimeSpan _freeSpinCooldown;
        private DateTime _cooldownExpireTime;
        private DateTime _currentTime;

        public bool CanClaimReward => _canClaimReward;

        public void Initialize(IPersistentPlayerData persistentPlayerData, IDataProvider gameDataProvider)
        {
            _persistentPlayerData = persistentPlayerData;
            _gameDataProvider = gameDataProvider;
            
             _canClaimReward = true;

             if (!_persistentPlayerData.PlayerGameData.LastClaimTime.HasValue)
             {
                 texts.SetActive(false);
                 return;
             }
             
            _cooldownExpireTime = _persistentPlayerData.PlayerGameData.LastClaimTime.Value;
            _freeSpinCooldown = new(freeSpinDaysCooldown,freeSpinHoursCooldown,freeSpinMinutesCooldown,freeSpinSecondsCooldown);
            _cooldownExpireTime.Add(_freeSpinCooldown);
        }

        private void Update()
        {
            _currentTime = DateTime.UtcNow;
            UpdateRewardState();
        }

        private void UpdateRewardState()
        {
            if (!_persistentPlayerData.PlayerGameData.LastClaimTime.HasValue) return;
            
            _currentFreeSpinCooldown = _cooldownExpireTime - _currentTime;
            
            if (_currentFreeSpinCooldown.TotalMilliseconds > 0)
            {
                _canClaimReward = false;
                texts.SetActive(!_canClaimReward);
                UpdateTimerValue();
                return;
            }
            
            _canClaimReward = true;
            texts.SetActive(!_canClaimReward);
        }

        private void UpdateTimerValue()
        {
            var dateTime = new TimeSpan(_currentFreeSpinCooldown.Days, _currentFreeSpinCooldown.Hours,_currentFreeSpinCooldown.Minutes,
                _currentFreeSpinCooldown.Seconds);

            timer.text = dateTime.ToString();
        }

        public void OnRewardEarned()
        {
            _cooldownExpireTime = _persistentPlayerData.PlayerGameData.LastClaimTime.Value.Add(_freeSpinCooldown);
            UpdateRewardState();
        }
    }
}