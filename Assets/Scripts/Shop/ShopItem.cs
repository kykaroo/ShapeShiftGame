using UnityEngine;

namespace Shop
{
    public abstract class ShopItem : ScriptableObject
    {
        [field:SerializeField] public GameObject Model { get; private set; }
        [field:SerializeField] public Sprite Image { get; private set; }
        [field:SerializeField] public int Price { get; private set; }
    }
}