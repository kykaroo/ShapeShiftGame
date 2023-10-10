using System;
using UnityEngine;

namespace Level
{
    public class LevelEndTrigger : MonoBehaviour
    {
        public event Action OnLevelComplete;
        public event Action OnLevelFailed;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.root.TryGetComponent<EnemyAi>(out var enemyAi))
            {
                OnLevelFailed?.Invoke();
            }
            
            OnLevelComplete?.Invoke();
        }
    }
}
