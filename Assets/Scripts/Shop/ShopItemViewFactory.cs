using System;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "Shop Item View Factory", menuName = "Shop/ShopItemViewFactory")]
    public class ShopItemViewFactory : ScriptableObject
    {
        [SerializeField] private ShopItemView humanFormSkinItemPrefab;
        [SerializeField] private ShopItemView carFormSkinItemPrefab;
        [SerializeField] private ShopItemView helicopterFormSkinItemPrefab;
        [SerializeField] private ShopItemView boatFormSkinItemPrefab;

        public ShopItemView Get(ShopItem shopItem, Transform parent)
        {
            var visitor = new ShopItemVisitor(humanFormSkinItemPrefab, carFormSkinItemPrefab,
                helicopterFormSkinItemPrefab, boatFormSkinItemPrefab);
            visitor.Visit(shopItem);

            var instance = Instantiate(visitor.Prefab, parent);
            return instance;
        }
        
        private class ShopItemVisitor : IShopItemVisitor
        {
            private readonly ShopItemView _humanSkinItemPrefab;
            private readonly ShopItemView _carSkinItemPrefab;
            private readonly ShopItemView _helicopterSkinItemPrefab;
            private readonly ShopItemView _boatSkinItemPrefab;

            public ShopItemVisitor(ShopItemView humanSkinItemPrefab, ShopItemView carSkinItemPrefab,
                ShopItemView helicopterSkinItemPrefab, ShopItemView boatSkinItemPrefab)
            {
                _humanSkinItemPrefab = humanSkinItemPrefab;
                _carSkinItemPrefab = carSkinItemPrefab;
                _helicopterSkinItemPrefab = helicopterSkinItemPrefab;
                _boatSkinItemPrefab = boatSkinItemPrefab;
            }
            
            public ShopItemView Prefab { get; private set; }
            
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
                Prefab = _humanSkinItemPrefab;

            public void Visit(CarFormSkinItem carFormSkinItem) =>
                Prefab = _carSkinItemPrefab;

            public void Visit(HelicopterFormSkinItem helicopterFormSkinItem) =>
                Prefab = _helicopterSkinItemPrefab;

            public void Visit(BoatFormSkinItem boatFormSkinItem) =>
                Prefab = _boatSkinItemPrefab;
        }
    }
}