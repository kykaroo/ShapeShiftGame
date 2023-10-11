using System;
using FormStateMachine.Forms;
using Shop.CarFormSkins;
using UnityEngine;

namespace FormsFactories
{
    [CreateAssetMenu(fileName = "Car Form Factory", menuName = "FormFactories/CarFormFactory")]
    public class CarFormFactory : ScriptableObject
    {
        [SerializeField] private CarForm blue;
        [SerializeField] private CarForm green;
        [SerializeField] private CarForm greenTruck;
        [SerializeField] private CarForm police;
        [SerializeField] private CarForm purple;
        [SerializeField] private CarForm silver;
        [SerializeField] private CarForm sportWhite;
        [SerializeField] private CarForm sportRed;
        [SerializeField] private CarForm sportBlue;
        [SerializeField] private CarForm yellow;

        public CarForm Get(CarFormSkins skinType, Vector3 spawnPosition, Transform parent)
        {
            var instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, parent);
            return instance;
        }

        private CarForm GetPrefab(CarFormSkins skinType)
        {
            return skinType switch
            {
                CarFormSkins.Blue => blue,
                CarFormSkins.Green => green,
                CarFormSkins.GreenTruck => greenTruck,
                CarFormSkins.Police => police,
                CarFormSkins.Purple => purple,
                CarFormSkins.Silver => silver,
                CarFormSkins.SportWhite => sportWhite,
                CarFormSkins.SportRed => sportRed,
                CarFormSkins.SportBlue => sportBlue,
                CarFormSkins.Yellow => yellow,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }
    }
}