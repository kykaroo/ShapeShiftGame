using System;
using Data;
using Data.PlayerGameData;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SkinSelector : IShopItemVisitor
    {
        private PersistentPlayerGameData _persistentPlayerData;
        

        public SkinSelector(PersistentPlayerGameData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;
        
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
            _persistentPlayerData.SelectedHumanFormSkin = humanFormSkinItem.SkinType;

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            _persistentPlayerData.SelectedCarFormSkin = carFormSkinItem.SkinType;

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            _persistentPlayerData.SelectedHelicopterFormSkin = helicopterFormSkinItem.SkinType;

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            _persistentPlayerData.SelectedBoatFormSkin = boatFormSkinItem.SkinType;
    }
}