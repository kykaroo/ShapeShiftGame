using System;
using UnityEngine;

namespace Level
{
    public class VictoryTrigger : MonoBehaviour
    {
        public event Action OnLevelComplete;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent<EnemyAi>(out var enemyAi)) return;
            OnLevelComplete?.Invoke();
        }
    }
}
