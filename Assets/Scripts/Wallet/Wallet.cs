using System;
using Data.PlayerGameData;
using Zenject;

namespace Wallet
{
    public class Wallet
    {
        public event Action<int> CoinsChanged;

        private readonly PersistentGameData _persistentGameData;
        
        [Inject]
        public Wallet(PersistentGameData persistentGameData) => _persistentGameData = persistentGameData;

        public void AddCoins(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _persistentGameData.Money += coins;
            CoinsChanged?.Invoke(_persistentGameData.Money);
        }

        public int GetCurrentCoins() => _persistentGameData.Money;

        public bool IsEnough(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            return _persistentGameData.Money >= coins;
        }

        public void Spend(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _persistentGameData.Money -= coins;
            CoinsChanged?.Invoke(_persistentGameData.Money);
        }

        public void SetValue(int coins)
        {
            _persistentGameData.Money = coins;
            CoinsChanged?.Invoke(_persistentGameData.Money);
        }

        public void UpdateCoinsView()
        {
            CoinsChanged?.Invoke(_persistentGameData.Money);
        }
    }
}