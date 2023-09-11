using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class VictoryUi : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;

        public Button PlayAgainButton => playAgainButton;
    }
}