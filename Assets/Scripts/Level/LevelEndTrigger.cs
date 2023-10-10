using System;
using UnityEngine;

namespace Level
{
    public class LevelEndTrigger : MonoBehaviour
    {
        private bool _isLevelComplete;
        
        public event Action<bool> OnLevelComplete;
        private void OnTriggerEnter(Collider other)
        {
            if (_isLevelComplete) return;

            _isLevelComplete = true;
                
            if (other.transform.root.TryGetComponent<EnemyAi>(out var enemyAi))
            {
                OnLevelComplete?.Invoke(false);
                return;
            }
            
            OnLevelComplete?.Invoke(true);
        }
    }
}
