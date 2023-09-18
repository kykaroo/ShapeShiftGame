using UnityEngine;

namespace Shop.BoatFormSkins
{
    [CreateAssetMenu(fileName = "Boat Skin Item", menuName = "Shop/BoatSkinItem")]
    public class BoatFormSkinItem : ShopItem
    {
        [field:SerializeField] public BoatFormSkins SkinType { get; private set; }
    }
}