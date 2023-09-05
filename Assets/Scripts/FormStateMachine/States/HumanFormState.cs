using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class HumanFormState : FormStateBase
    {
        private readonly HumanForm _humanForm;

        public HumanFormState(HumanForm humanForm, GlobalVariables globalVariables)
        {
            _humanForm = humanForm;
            
            _humanForm.playerTransform = globalVariables.PlayerTransform;
            _humanForm.gravityForce = globalVariables.GravityForce;

            _humanForm.allEnvironment = globalVariables.AllEnvironment;
            _humanForm.stairsSlopeMask = globalVariables.AllEnvironment;
            _humanForm.waterMask = globalVariables.WaterMask;
            _humanForm.groundMask = globalVariables.GroundMask;
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