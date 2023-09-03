using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class HelicopterFormState : FormStateBase
    {
        private HelicopterForm _helicopterForm;

        public HelicopterFormState(HelicopterForm helicopterForm)
        {
            _helicopterForm = helicopterForm;
        }

        protected override void OnEnter()
        {
            _helicopterForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _helicopterForm.gameObject.SetActive(false);
        }
    }
}