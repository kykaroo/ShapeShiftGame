using UnityEngine.UI;
using Zenject;

namespace LevelProgressBar
{
    public class LevelProgressBar : ITickable
    {
        private readonly ProgressBarUi _progressBarUi;
        private Player _player;
        private EnemyHandler _enemyHandler;
        private float _levelEndZ;
        private float _levelStartZ;
        private float _levelRemainDistance;
        private float _levelLenght;

        [Inject]
        public LevelProgressBar(ProgressBarUi progressBarUi, Player player, EnemyHandler enemyHandler)
        {
            _progressBarUi = progressBarUi;
            _player = player;
            _enemyHandler = enemyHandler;
        }
        public void SetLevelLenght(float levelStartZ, float levelEndZ)
        {
            var position = _player.PlayerBody.position;
            // var position = _playerTransform.position;
            _levelRemainDistance = levelEndZ - position.z;
            _levelEndZ = levelEndZ;
            _levelStartZ = levelStartZ;
            _levelLenght = levelEndZ - position.z;
        }
        
       private float GetRemainDistance(float playerZ)
       {
           return _levelRemainDistance - playerZ - _levelStartZ;
       }
       
       static void SetProgress(Slider indicator ,float progress)
       {
           indicator.value = progress;
       }
       
       public void Tick()
       {
           if (!_progressBarUi.PlayerProgressIndicator.gameObject.activeSelf) return;
           
           if (_player.PlayerBody.position.z <= _levelLenght && _player.PlayerBody.position.z <= _levelEndZ)
           {
               var distance = 1 - GetRemainDistance(_player.PlayerBody.position.z) / _levelLenght;
               SetProgress(_progressBarUi.PlayerProgressIndicator, distance);
           }
           for (var i = 0; i < _progressBarUi.AiProgressIndicator.Length; i++)
           {
               if (!(_enemyHandler.AIBodies[i].position.z <= _levelRemainDistance) ||
                   !(_enemyHandler.AIBodies[i].position.z <= _levelEndZ)) continue;
               var distance = 1 - GetRemainDistance(_enemyHandler.AIBodies[i].position.z) / _levelLenght;
               SetProgress(_progressBarUi.AiProgressIndicator[i], distance);
           }
       }
    }
}