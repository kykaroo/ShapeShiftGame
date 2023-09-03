using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class CarFormState : FormStateBase
    {
        private CarForm _carForm;

        public CarFormState(CarForm carForm)
        {
            _carForm = carForm;
        }

        protected override void OnEnter()
        {
            _carForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _carForm.gameObject.SetActive(false);
        }
    }
}