using System;
using Data.PlayerGameData;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SkinSelector : IShopItemVisitor
    {
        private readonly PersistentGameData _persistentData;
        

        public SkinSelector(PersistentGameData persistentData) => _persistentData = persistentData;
        
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
            _persistentData.SelectedHumanFormSkin = humanFormSkinItem.SkinType;

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            _persistentData.SelectedCarFormSkin = carFormSkinItem.SkinType;

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            _persistentData.SelectedHelicopterFormSkin = helicopterFormSkinItem.SkinType;

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            _persistentData.SelectedBoatFormSkin = boatFormSkinItem.SkinType;
    }
}