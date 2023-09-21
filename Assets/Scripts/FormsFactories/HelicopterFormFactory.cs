using System;
using FormStateMachine.Forms;
using Shop.HelicopterFormSkins;
using UnityEngine;

namespace FormsFactories
{
    [CreateAssetMenu(fileName = "Helicopter Form Factory", menuName = "FormFactories/HelicopterFormFactory")]
    public class HelicopterFormFactory : ScriptableObject
    {
        [SerializeField] private HelicopterForm scout;
        [SerializeField] private HelicopterForm transport;
        [SerializeField] private HelicopterForm attack;

        public HelicopterForm Get(HelicopterFormSkins skinType, Vector3 spawnPosition, Transform parent)
        {
            var instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, parent);
            return instance;
        }

        private HelicopterForm GetPrefab(HelicopterFormSkins skinType)
        {
            return skinType switch
            {
                HelicopterFormSkins.Scout => scout,
                HelicopterFormSkins.Transport => transport,
                HelicopterFormSkins.Attack => attack,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }
    }
}