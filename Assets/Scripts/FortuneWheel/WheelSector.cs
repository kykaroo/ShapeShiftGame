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
        [Tooltip("Вероятность обозначает")] 
        public float probability;
        public RewardType rewardType;
        [Header("Money reward")]
        public int rewardMoneyValue;
        [Header("Item reward")]
        public ShopItem itemReward;
        public int rewardMoneyValueIfItemOpened;
    }
}