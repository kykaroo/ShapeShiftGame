using System;
using Ai;
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
                
            if (other.transform.root.TryGetComponent<EnemyAi>(out _))
            {
                OnLevelComplete?.Invoke(false);
                return;
            }
            
            OnLevelComplete?.Invoke(true);
        }
    }
}
