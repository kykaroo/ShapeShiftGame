using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "FortuneWheel/SpinCooldown", fileName = "Spin Cooldown", order = 1)]
    public class SpinCooldownConfig : ScriptableObject
    {
        public int freeSpinDaysCooldown;
        public int freeSpinHoursCooldown;
        public int freeSpinMinutesCooldown;
        public int freeSpinSecondsCooldown;
    }
}