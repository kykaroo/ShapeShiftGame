using System;
using Data;

namespace Wallet
{
    public class Wallet
    {
        public event Action<int> CoinsChanged;

        private IPersistentPlayerData _persistentPlayerData;

        public Wallet(IPersistentPlayerData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;

        public void AddCoins(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _persistentPlayerData.PlayerGameData.Money += coins;
            CoinsChanged?.Invoke(_persistentPlayerData.PlayerGameData.Money);
        }

        public int GetCurrentCoins() => _persistentPlayerData.PlayerGameData.Money;

        public bool IsEnough(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            return _persistentPlayerData.PlayerGameData.Money >= coins;
        }

        public void Spend(int coins)
        {
            if (coins < 0)
            {
                throw new ArgumentException(nameof(coins));
            }

            _persistentPlayerData.PlayerGameData.Money -= coins;
            CoinsChanged?.Invoke(_persistentPlayerData.PlayerGameData.Money);
        }

        public void SetValue(int coins)
        {
            _persistentPlayerData.PlayerGameData.Money = coins;
            CoinsChanged?.Invoke(_persistentPlayerData.PlayerGameData.Money);
        }
    }
}