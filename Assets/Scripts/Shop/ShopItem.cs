using UnityEngine;

namespace Shop
{
    public abstract class ShopItem : ScriptableObject
    {
        [field:SerializeField] public GameObject GameModel { get; private set; }
        [field:SerializeField] public GameObject ShopModel { get; private set; }
        [field:SerializeField] public Sprite Image { get; private set; }
        [field:SerializeField] public int Price { get; private set; }
    }
}