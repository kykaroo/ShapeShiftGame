using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class BoatFormState : FormStateBase
    {
        private BoatForm _boatForm;

        public BoatFormState(BoatForm boatForm)
        {
            _boatForm = boatForm;
        }

        protected override void OnEnter()
        {
            _boatForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _boatForm.gameObject.SetActive(false);
        }
    }
}