using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class FormChangeUi : MonoBehaviour
    {
        [SerializeField] private Button humanFormButton;
        [SerializeField] private Button carFormButton;
        [SerializeField] private Button helicopterFormButton;
        [SerializeField] private Button boatFormButton;

        public event Action OnHumanFormButtonClick;
        public event Action OnCarFormButtonClick;
        public event Action OnHelicopterFormButtonClick;
        public event Action OnBoatFormButtonClick;

        private void HumanFormButtonClick() => OnHumanFormButtonClick?.Invoke();
        private void CarFormButtonClick() => OnCarFormButtonClick?.Invoke();
        private void HelicopterFormButtonClick() => OnHelicopterFormButtonClick?.Invoke();
        private void BoatFormButtonClick() => OnBoatFormButtonClick?.Invoke();
        
        private void Awake()
        {
            humanFormButton.onClick.AddListener(HumanFormButtonClick);
            carFormButton.onClick.AddListener(CarFormButtonClick);
            helicopterFormButton.onClick.AddListener(HelicopterFormButtonClick);
            boatFormButton.onClick.AddListener(BoatFormButtonClick);
        }
    }
}