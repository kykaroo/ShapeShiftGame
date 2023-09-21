using Data;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SkinUnlocker : IShopItemVisitor
    {
        private IPersistentData _persistentData;

        public SkinUnlocker(IPersistentData persistentData) => _persistentData = persistentData;

        public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

        public void Visit(HumanFormSkinItem humanFormSkinItem) =>
            _persistentData.PlayerData.OpenHumanFormSkin(humanFormSkinItem.SkinType);

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            _persistentData.PlayerData.OpenCarFormSkin(carFormSkinItem.SkinType);

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            _persistentData.PlayerData.OpenHelicopterFormSkin(helicopterFormSkinItem.SkinType);

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            _persistentData.PlayerData.OpenBoatFormSkin(boatFormSkinItem.SkinType);
    }
}