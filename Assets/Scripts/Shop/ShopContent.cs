using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shop
{ 
    using HumanFormSkinItem = HumanFormSkins.HumanFormSkinItem;
    using CarFormSkinItem = CarFormSkins.CarFormSkinItem;
    using HelicopterFormSkinItem = HelicopterFormSkins.HelicopterFormSkinItem;
    using BoatFormSkinItem = BoatFormSkins.BoatFormSkinItem;
    [CreateAssetMenu(fileName = "Shop Content", menuName = "Shop/ShopContent")]
    public class ShopContent : ScriptableObject
    {
        [SerializeField] private List<HumanFormSkinItem> humanFormSkinItems;
        [SerializeField] private List<CarFormSkinItem> carFormSkinItems;
        [SerializeField] private List<HelicopterFormSkinItem> helicopterFormSkinItems;
        [SerializeField] private List<BoatFormSkinItem> boatFormSkinItems;

        public IEnumerable<HumanFormSkinItem> HumanFormSkinItems => humanFormSkinItems;

        public IEnumerable<CarFormSkinItem> CarFormSkinItems => carFormSkinItems;

        public IEnumerable<HelicopterFormSkinItem> HelicopterFormSkinItems => helicopterFormSkinItems;

        public IEnumerable<BoatFormSkinItem> BoatFormSkinItems => boatFormSkinItems;

        private void OnValidate()
        {
            var humanFormSkinsDuplicates = humanFormSkinItems.GroupBy(item => item.SkinType)
                .Where(array => array.Count() > 1);
            if (humanFormSkinsDuplicates.Any())
            {
                throw new InvalidOperationException(nameof(humanFormSkinItems));
            }
            
            var carFormSkinsDuplicates = carFormSkinItems.GroupBy(item => item.SkinType)
                .Where(array => array.Count() > 1);
            var formSkinsDuplicates = carFormSkinsDuplicates.ToList();  
            if (formSkinsDuplicates.Any())
            {
                throw new InvalidOperationException(nameof(carFormSkinItems));
            }
            
            var helicopterFormSkinsDuplicates = helicopterFormSkinItems.GroupBy(item => item.SkinType)
                .Where(array => array.Count() > 1);
            if (helicopterFormSkinsDuplicates.Any())
            {
                throw new InvalidOperationException(nameof(helicopterFormSkinItems));
            }
            
            var boatFormSkinsDuplicates = boatFormSkinItems.GroupBy(item => item.SkinType)
                .Where(array => array.Count() > 1);
            if (boatFormSkinsDuplicates.Any())
            {
                throw new InvalidOperationException(nameof(boatFormSkinItems));
            }
        }
    }
}