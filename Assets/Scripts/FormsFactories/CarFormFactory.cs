using System;
using FormStateMachine.Forms;
using Shop.CarFormSkins;
using UnityEngine;

namespace FormsFactories
{
    [CreateAssetMenu(fileName = "Car Form Factory", menuName = "FormFactories/CarFormFactory")]
    public class CarFormFactory : ScriptableObject
    {
        [SerializeField] private CarForm Blue;
        [SerializeField] private CarForm Green;
        [SerializeField] private CarForm GreenTruck;
        [SerializeField] private CarForm Police;
        [SerializeField] private CarForm Purple;
        [SerializeField] private CarForm Silver;
        [SerializeField] private CarForm sportWhite;
        [SerializeField] private CarForm sportRed;
        [SerializeField] private CarForm sportBlue;
        [SerializeField] private CarForm Yellow;

        public CarForm Get(CarFormSkins skinType, Vector3 spawnPosition, Transform parent)
        {
            var instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, parent);
            return instance;
        }

        private CarForm GetPrefab(CarFormSkins skinType)
        {
            return skinType switch
            {
                CarFormSkins.Blue => Blue,
                CarFormSkins.Green => Green,
                CarFormSkins.GreenTruck => GreenTruck,
                CarFormSkins.Police => Police,
                CarFormSkins.Purple => Purple,
                CarFormSkins.Silver => Silver,
                CarFormSkins.SportWhite => sportWhite,
                CarFormSkins.SportRed => sportRed,
                CarFormSkins.SportBlue => sportBlue,
                CarFormSkins.Yellow => Yellow,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }
    }
}