using System;
using System.Linq;
using Data.PlayerGameData;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class OpenSkinsChecker : IShopItemVisitor
    {
        private readonly PlayerGameData _playerGameData;
        
        public bool IsOpened { get; private set; }

        public OpenSkinsChecker(PlayerGameData playerGameData) => _playerGameData = playerGameData;
        
        public void Visit(ShopItem shopItem)
        {
            switch (shopItem)
            {
                case HumanFormSkinItem humanFormSkinItem:
                    Visit(humanFormSkinItem);
                    break;
                case CarFormSkinItem carFormSkinItem:
                    Visit(carFormSkinItem);
                    break;
                case HelicopterFormSkinItem helicopterFormSkinItem:
                    Visit(helicopterFormSkinItem);
                    break;
                case BoatFormSkinItem boatFormSkinItem:
                    Visit(boatFormSkinItem);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public void Visit(HumanFormSkinItem humanFormSkinItem) => 
            IsOpened = _playerGameData.OpenHumanFormSkins.Contains(humanFormSkinItem.SkinType);

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            IsOpened = _playerGameData.OpenCarFormSkins.Contains(carFormSkinItem.SkinType);

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            IsOpened = _playerGameData.OpenHelicopterFormSkins.Contains(helicopterFormSkinItem.SkinType);

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            IsOpened = _playerGameData.OpenBoatFormSkins.Contains(boatFormSkinItem.SkinType);
    }
}