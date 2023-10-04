using System;
using Shop;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FortuneWheel
{
    [Serializable]
    public class WheelSector
    {
        [Header("Reward player view")]
        public Image rewardImage;
        public Sprite image;
        public TextMeshProUGUI rewardText;
        public string text;
        public float probabilityWeight = 1;
        public RewardType rewardType;
        [Header("Item reward")]
        public ShopItem itemReward;
        public int rewardMoneyValueIfItemOpened;
    }
}