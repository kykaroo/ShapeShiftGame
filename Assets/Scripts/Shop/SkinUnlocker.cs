using System;
using Data.PlayerGameData;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SkinUnlocker : IShopItemVisitor
    {
        private readonly PersistentGameData _persistentData;

        public SkinUnlocker(PersistentGameData persistentData) => _persistentData = persistentData;

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
            _persistentData.OpenHumanFormSkin(humanFormSkinItem.SkinType);

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            _persistentData.OpenCarFormSkin(carFormSkinItem.SkinType);

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            _persistentData.OpenHelicopterFormSkin(helicopterFormSkinItem.SkinType);

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            _persistentData.OpenBoatFormSkin(boatFormSkinItem.SkinType);
    }
}