using UnityEngine.UI;
using Zenject;

namespace Ui.ProgressBar
{
    public class ProgressBarUi
    {
        public Slider PlayerProgressIndicator { get; }

        public Slider[] AiProgressIndicator { get; }

        [Inject]
        public ProgressBarUi(Slider playerProgressIndicator, Slider[] aiProgressIndicator)
        {
            PlayerProgressIndicator = playerProgressIndicator;
            AiProgressIndicator = aiProgressIndicator;
        }
    }
}