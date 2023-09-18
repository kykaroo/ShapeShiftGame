using UnityEngine;

namespace Shop.HelicopterFormSkins
{
    [CreateAssetMenu(fileName = "Helicopter Skin Item", menuName = "Shop/HelicopterSkinItem")]
    public class HelicopterFormSkinItem : ShopItem
    {
        [field:SerializeField] public HelicopterFormSkins SkinType { get; private set; }
    }
}