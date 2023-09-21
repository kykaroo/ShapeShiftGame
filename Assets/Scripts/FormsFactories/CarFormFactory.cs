using System;
using FormStateMachine.Forms;
using Shop.CarFormSkins;
using UnityEngine;

namespace FormsFactories
{
    [CreateAssetMenu(fileName = "Car Form Factory", menuName = "FormFactories/CarFormFactory")]
    public class CarFormFactory : ScriptableObject
    {
        [SerializeField] private CarForm white;
        [SerializeField] private CarForm red;
        [SerializeField] private CarForm blue;

        public CarForm Get(CarFormSkins skinType, Vector3 spawnPosition, Transform parent)
        {
            var instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, parent);
            return instance;
        }

        private CarForm GetPrefab(CarFormSkins skinType)
        {
            return skinType switch
            {
                CarFormSkins.White => white,
                CarFormSkins.Red => red,
                CarFormSkins.Blue => blue,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }
    }
}