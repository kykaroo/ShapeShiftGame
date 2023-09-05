using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class BoatFormState : FormStateBase
    {
        private readonly BoatForm _boatForm;

        public BoatFormState(BoatForm boatForm, GlobalVariables globalVariables, Ground ground)
        {
            _boatForm = boatForm;

            _boatForm.playerTransform = globalVariables.PlayerTransform;
            _boatForm.gravityForce = globalVariables.GravityForce;

            _boatForm.allEnvironment = globalVariables.AllEnvironment;
            _boatForm.waterMask = globalVariables.WaterMask;
            _boatForm.stairsSlopeMask = globalVariables.StairsSlopeMask;
            _boatForm.groundMask = globalVariables.GroundMask;
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