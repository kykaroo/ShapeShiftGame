using UnityEngine;
using UnityEngine.UI;

namespace LevelProgressBar
{
    public class LevelProgressBar
    {
         private Slider _playerProgressIndicator;
         private Slider[] _aiProgressIndicator;
         private Transform _playerTransform;
         private Transform[] _aiTransform;
         private float _levelEndZ;
         private float _levelStartZ;
         private float _levelRemainDistance;
         private float _levelLenght;

         public void Initialize(Slider playerProgressIndicator, Slider[] aiProgressIndicator,
             Transform playerTransform, Transform[] botsTransform)
         {
             _playerProgressIndicator = playerProgressIndicator;
             _aiProgressIndicator = aiProgressIndicator;
             _playerTransform = playerTransform;
             _aiTransform = botsTransform;
         }

         public void SetLevelLenght(float levelStartZ, float levelEndZ)
         {
             var position = _playerTransform.position;
             _levelRemainDistance = levelEndZ - position.z;
             _levelEndZ = levelEndZ;
             _levelStartZ = levelStartZ;
             _levelLenght = levelEndZ - position.z;
         }

        public void UpdateData()
        {
            if (_playerTransform.position.z <= _levelLenght && _playerTransform.position.z <= _levelEndZ)
            {
                var distance = 1 - GetRemainDistance(_playerTransform.position.z) / _levelLenght;
                SetProgress(_playerProgressIndicator, distance);
            }

            for (var i = 0; i < _aiProgressIndicator.Length; i++)
            {
                if (!(_aiTransform[i].position.z <= _levelRemainDistance) ||
                    !(_aiTransform[i].position.z <= _levelEndZ)) continue;
                var distance = 1 - GetRemainDistance(_aiTransform[i].position.z) / _levelLenght;
                SetProgress(_aiProgressIndicator[i], distance);
            }
        }

        private float GetRemainDistance(float playerZ)
        {
            return _levelRemainDistance - playerZ - _levelStartZ;
        }

        static void SetProgress(Slider indicator ,float progress)
        {
            indicator.value = progress;
        }
    }
}