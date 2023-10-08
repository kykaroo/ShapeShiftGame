using UnityEngine.UI;
using Zenject;

namespace LevelProgressBar
{
    public class ProgressBarUi
    {
        private readonly Slider _playerProgressIndicator;
        private readonly Slider[] _aiProgressIndicator;

        public Slider PlayerProgressIndicator => _playerProgressIndicator;

        public Slider[] AiProgressIndicator => _aiProgressIndicator;

        [Inject]
        public ProgressBarUi(Slider playerProgressIndicator, Slider[] aiProgressIndicator)
        {
            _playerProgressIndicator = playerProgressIndicator;
            _aiProgressIndicator = aiProgressIndicator;
        }
    }
}