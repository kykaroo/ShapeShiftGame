using UnityEngine;

namespace Shop
{
    public abstract class ShopItem : ScriptableObject
    {
        [field:SerializeField] public GameObject ShopModel { get; private set; }
        [field:SerializeField] public Sprite Image { get; private set; }
        [field:SerializeField] public int Price { get; private set; }
        [field:SerializeField] public bool IsAdReward { get; private set; }

        private const int ADUnlockId = 2;

        public int AdUnlockId => ADUnlockId;
    }
}