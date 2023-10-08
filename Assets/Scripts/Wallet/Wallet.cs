using System;
using Data.PlayerGameData;
using Zenject;

namespace Wallet
{
    public class Wallet
    {
        public event Action<int> CoinsChanged;

        private readonly PersistentPlayerGameData _persistentPlayerGameData;
        
        [Inject]
        public Wallet(PersistentPlayerGameData persistentPlayerGameData) => _persistentPlayerGameData = persistentPlayerGameData;

        public void AddCoins(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _persistentPlayerGameData.Money += coins;
            CoinsChanged?.Invoke(_persistentPlayerGameData.Money);
        }

        public int GetCurrentCoins() => _persistentPlayerGameData.Money;

        public bool IsEnough(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            return _persistentPlayerGameData.Money >= coins;
        }

        public void Spend(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _persistentPlayerGameData.Money -= coins;
            CoinsChanged?.Invoke(_persistentPlayerGameData.Money);
        }

        public void SetValue(int coins)
        {
            _persistentPlayerGameData.Money = coins;
            CoinsChanged?.Invoke(_persistentPlayerGameData.Money);
        }

        public void UpdateCoinsView()
        {
            CoinsChanged?.Invoke(_persistentPlayerGameData.Money);
        }
    }
}