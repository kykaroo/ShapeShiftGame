using System;
using Data;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SkinUnlocker : IShopItemVisitor
    {
        private IPersistentPlayerData _persistentPlayerData;

        public SkinUnlocker(IPersistentPlayerData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;

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
            _persistentPlayerData.PlayerGameData.OpenHumanFormSkin(humanFormSkinItem.SkinType);

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            _persistentPlayerData.PlayerGameData.OpenCarFormSkin(carFormSkinItem.SkinType);

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            _persistentPlayerData.PlayerGameData.OpenHelicopterFormSkin(helicopterFormSkinItem.SkinType);

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            _persistentPlayerData.PlayerGameData.OpenBoatFormSkin(boatFormSkinItem.SkinType);
    }
}