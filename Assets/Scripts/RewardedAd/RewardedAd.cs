using Presenters;
using Zenject;

namespace RewardedAd
{
    public class RewardedAd
    {
        private readonly Wallet.Wallet _wallet;

        [Inject]
        public RewardedAd(Wallet.Wallet wallet)
        {
            _wallet = wallet;

            YG.YandexGame.RewardVideoEvent += GetReward;
        }

        private void GetReward(int id)
        {
            switch (id)
            {
                case 0:
                    break;
                case 1:
                    _wallet.AddCoins(50);
                    break;
            }
        }
    }
}