using System;
using FormStateMachine.Forms;
using Shop.BoatFormSkins;
using UnityEngine;

namespace FormsFactories
{
    [CreateAssetMenu(fileName = "Boat Form Factory", menuName = "FormFactories/BoatFormFactory")]
    public class BoatFormFactory : ScriptableObject
    {
        [SerializeField] private BoatForm boat1;
        [SerializeField] private BoatForm boat2;

        public BoatForm Get(BoatFormSkins skinType, Vector3 spawnPosition, Transform parent)
        {
            var instance = Instantiate(GetPrefab(skinType), spawnPosition, Quaternion.identity, parent);
            return instance;
        }

        private BoatForm GetPrefab(BoatFormSkins skinType)
        {
            return skinType switch
            {
                BoatFormSkins.Boat1 => boat1,
                BoatFormSkins.Boat2 => boat2,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }
    }
}