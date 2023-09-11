using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class StartUi : MonoBehaviour
    { 
        [SerializeField] private Button startButton;

        public Button StartButton => startButton;
    }
}