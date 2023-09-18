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
            var instance = shopItem switch
            {
                HumanFormSkinItem => Instantiate(humanFormSkinItemPrefab, parent),
                CarFormSkinItem => Instantiate(carFormSkinItemPrefab, parent),
                HelicopterFormSkinItem => Instantiate(helicopterFormSkinItemPrefab, parent),
                BoatFormSkinItem => Instantiate(boatFormSkinItemPrefab, parent),
                _ => throw new ArgumentException(nameof(shopItem))
            };

            instance.Initialize(shopItem);
            return instance;
        }
    }
}