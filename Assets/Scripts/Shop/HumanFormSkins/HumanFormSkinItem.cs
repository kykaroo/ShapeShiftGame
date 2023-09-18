using UnityEngine;

namespace Shop.HumanFormSkins
{
    [CreateAssetMenu(fileName = "Human Skin Item", menuName = "Shop/HumanSkinItem")]
    public class HumanFormSkinItem : ShopItem
    {
        [field:SerializeField] public HumanFormSkins SkinType { get; private set; }
    }
}