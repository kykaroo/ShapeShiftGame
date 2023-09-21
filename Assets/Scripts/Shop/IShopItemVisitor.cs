using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;

namespace Shop
{
    public interface IShopItemVisitor
    {
        void Visit(ShopItem shopItem);
        void Visit(HumanFormSkinItem humanFormSkinItem);
        void Visit(CarFormSkinItem carFormSkinItem);
        void Visit(HelicopterFormSkinItem helicopterFormSkinItem);
        void Visit(BoatFormSkinItem boatFormSkinItem);
    }
}