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
        private IPersistentData _persistentData;
        
        public bool IsSelected { get; private set; }

        public SelectedSkinChecker(IPersistentData persistentData) => _persistentData = persistentData;
        
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
            IsSelected = _persistentData.PlayerData.SelectedHumanFormSkin == humanFormSkinItem.SkinType;

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            IsSelected = _persistentData.PlayerData.SelectedCarFormSkin == carFormSkinItem.SkinType;

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            IsSelected = _persistentData.PlayerData.SelectedHelicopterFormSkin == helicopterFormSkinItem.SkinType;

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            IsSelected = _persistentData.PlayerData.SelectedBoatFormSkin == boatFormSkinItem.SkinType;
    }
}