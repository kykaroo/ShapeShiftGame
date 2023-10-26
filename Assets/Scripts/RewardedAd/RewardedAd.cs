using System;
using Shop;
using Ui;
using Zenject;

namespace RewardedAd
{
    public class RewardedAd
    {
        private readonly Wallet.Wallet _wallet;
        private readonly SkinsManager _skinsManager;
        private readonly ShopUi _shopUi;

        public ShopItem ShopItem { get; set; }

        [Inject]
        public RewardedAd(Wallet.Wallet wallet, SkinsManager skinsManager,ShopUi shopUi)
        {
            _wallet = wallet;
            _skinsManager = skinsManager;
            _shopUi = shopUi;

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
                case 2:
                    _skinsManager.OpenChecker.Visit(ShopItem);
                    
                    if (_skinsManager.OpenChecker.IsOpened)
                    {
                        throw new ArgumentException($"Попытка открыть скин \"{ShopItem}\", когда он уже открыт");
                    }
                    
                    _shopUi.UnlockSkin();
                    break;
            }
        }
    }
}