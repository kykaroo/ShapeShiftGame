using UnityEngine;

namespace Shop.CarFormSkins
{
    [CreateAssetMenu(fileName = "Car Skin Item", menuName = "Shop/CarSkinItem")]
    public class CarFormSkinItem : ShopItem
    {
        [field:SerializeField] public CarFormSkins SkinType { get; private set; }
    }
}