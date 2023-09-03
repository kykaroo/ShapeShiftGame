using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class HumanFormState : FormStateBase
    {
        private readonly HumanForm _humanForm;

        public HumanFormState(HumanForm humanForm, GlobalVariables globalVariables)
        {
            _humanForm = humanForm;
            
            _humanForm.playerBody = globalVariables.PlayerBody;
            _humanForm.gravityForce = globalVariables.GravityForce;
            _humanForm.slopeGravityForce = globalVariables.SlopeGravityForce;

            _humanForm.allEnvironment = globalVariables.AllEnvironment;
            _humanForm.stairsSlopeMask = globalVariables.AllEnvironment;
            _humanForm.waterMask = globalVariables.WaterMask;
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