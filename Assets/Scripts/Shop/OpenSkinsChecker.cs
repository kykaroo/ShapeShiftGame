using System;
using System.Linq;
using Data;
using Data.PlayerGameData;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class OpenSkinsChecker : IShopItemVisitor
    {
        private readonly PersistentPlayerGameData _persistentPlayerGameData;
        
        public bool IsOpened { get; private set; }

        public OpenSkinsChecker(PersistentPlayerGameData persistentPlayerGameData) => _persistentPlayerGameData = persistentPlayerGameData;
        
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
            IsOpened = _persistentPlayerGameData.OpenHumanFormSkins.Contains(humanFormSkinItem.SkinType);

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            IsOpened = _persistentPlayerGameData.OpenCarFormSkins.Contains(carFormSkinItem.SkinType);

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            IsOpened = _persistentPlayerGameData.OpenHelicopterFormSkins.Contains(helicopterFormSkinItem.SkinType);

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            IsOpened = _persistentPlayerGameData.OpenBoatFormSkins.Contains(boatFormSkinItem.SkinType);
    }
}