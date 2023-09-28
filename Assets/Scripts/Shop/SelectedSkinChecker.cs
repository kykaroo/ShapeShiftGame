using System;
using System.Linq;
using Data;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SelectedSkinChecker : IShopItemVisitor
    {
        private IPersistentPlayerData _persistentPlayerData;
        
        public bool IsSelected { get; private set; }

        public SelectedSkinChecker(IPersistentPlayerData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;
        
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
            IsSelected = _persistentPlayerData.PlayerGameData.SelectedHumanFormSkin == humanFormSkinItem.SkinType;

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            IsSelected = _persistentPlayerData.PlayerGameData.SelectedCarFormSkin == carFormSkinItem.SkinType;

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            IsSelected = _persistentPlayerData.PlayerGameData.SelectedHelicopterFormSkin == helicopterFormSkinItem.SkinType;

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            IsSelected = _persistentPlayerData.PlayerGameData.SelectedBoatFormSkin == boatFormSkinItem.SkinType;
    }
}