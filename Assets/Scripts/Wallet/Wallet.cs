using System;
using Data.PlayerGameData;
using Zenject;

namespace Wallet
{
    public class Wallet
    {
        public event Action<int> CoinsChanged;

        private readonly PlayerGameData _playerGameData;
        
        [Inject]
        public Wallet(PlayerGameData playerGameData) => _playerGameData = playerGameData;

        public void AddCoins(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _playerGameData.Money += coins;
            CoinsChanged?.Invoke(_playerGameData.Money);
        }

        public int GetCurrentCoins() => _playerGameData.Money;

        public bool IsEnough(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            return _playerGameData.Money >= coins;
        }

        public void Spend(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _playerGameData.Money -= coins;
            CoinsChanged?.Invoke(_playerGameData.Money);
        }

        public void SetValue(int coins)
        {
            _playerGameData.Money = coins;
            CoinsChanged?.Invoke(_playerGameData.Money);
        }

        public void UpdateCoinsView()
        {
            CoinsChanged?.Invoke(_playerGameData.Money);
        }
    }
}