using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class HumanFormState : FormStateBase
    {
        private HumanForm _humanForm;

        public HumanFormState(HumanForm humanForm)
        {
            _humanForm = humanForm;
        }

        protected override void OnEnter()
        {
            _humanForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _humanForm.gameObject.SetActive(false);
        }
    }
}