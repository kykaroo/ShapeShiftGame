using System.Linq;
using Data;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class OpenSkinsChecker : IShopItemVisitor
    {
        private IPersistentData _persistentData;
        
        public bool IsOpened { get; private set; }

        public OpenSkinsChecker(IPersistentData persistentData) => _persistentData = persistentData;
        
        public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

        public void Visit(HumanFormSkinItem humanFormSkinItem) => 
            IsOpened = _persistentData.PlayerData.OpenHumanFormSkins.Contains(humanFormSkinItem.SkinType);

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            IsOpened = _persistentData.PlayerData.OpenCarFormSkins.Contains(carFormSkinItem.SkinType);

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            IsOpened = _persistentData.PlayerData.OpenHelicopterFormSkins.Contains(helicopterFormSkinItem.SkinType);

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            IsOpened = _persistentData.PlayerData.OpenBoatFormSkins.Contains(boatFormSkinItem.SkinType);
    }
}