using System;
using FormStateMachine.Forms;
using Shop.HumanFormSkins;
using UnityEngine;

namespace FormsFactories
{
    [CreateAssetMenu(fileName = "Human Form Factory", menuName = "FormFactories/HumanFormFactory")]
    public class HumanFormFactory : ScriptableObject
    {
        [SerializeField] private HumanForm white;
        [SerializeField] private HumanForm red;
        [SerializeField] private HumanForm yellow;

        public HumanForm Get(HumanFormSkins skinType, Vector3 spawnPosition, Transform parent)
        {
            var instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, parent);
            return instance;
        }

        private HumanForm GetPrefab(HumanFormSkins skinType)
        {
            return skinType switch
            {
                HumanFormSkins.White => white,
                HumanFormSkins.Red => red,
                HumanFormSkins.Yellow => yellow,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }
    }
}