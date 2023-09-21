using Data;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public class SkinSelector : IShopItemVisitor
    {
        private IPersistentData _persistentData;
        

        public SkinSelector(IPersistentData persistentData) => _persistentData = persistentData;
        
        public void Visit(ShopItem shopItem) => Visit((dynamic)shopItem);

        public void Visit(HumanFormSkinItem humanFormSkinItem) => 
            _persistentData.PlayerData.SelectedHumanFormSkin = humanFormSkinItem.SkinType;

        public void Visit(CarFormSkinItem carFormSkinItem) =>
            _persistentData.PlayerData.SelectedCarFormSkin = carFormSkinItem.SkinType;

        public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
            _persistentData.PlayerData.SelectedHelicopterFormSkin = helicopterFormSkinItem.SkinType;

        public void Visit(BoatFormSkinItem boatFormSkinItem) =>
            _persistentData.PlayerData.SelectedBoatFormSkin = boatFormSkinItem.SkinType;
    }
}