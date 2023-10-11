using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel
{
    [CreateAssetMenu(menuName = "FortuneWheel/WheelSector", fileName = "Wheel Sector", order = 0)]
    public class WheelSectorConfig : ScriptableObject
    {
        [Header("Reward player view")]
        public Sprite image;
        public string rewardText;
        public RewardType rewardType;
        public float probabilityWeight = 1;
        [Space]
        [Header("Money reward")]
        public int moneyRewardValue;
        [Header("Item reward")]
        public ShopItem itemReward;
        public int rewardMoneyValueIfItemOpened;

        public Image RewardImagePlaceholder { get; set; }
        public TextMeshProUGUI RewardTextPlaceholder { get; set; }
    }
}