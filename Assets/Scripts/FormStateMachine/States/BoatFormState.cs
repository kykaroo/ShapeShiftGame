using FormStateMachine.Forms;

namespace FormStateMachine.States
{
    public class BoatFormState : FormStateBase
    {
        private readonly BoatForm _boatForm;

        public BoatFormState(BoatForm boatForm, GlobalVariables globalVariables)
        {
            _boatForm = boatForm;
            
            _boatForm.playerBody = globalVariables.PlayerBody;
            _boatForm.gravityForce = globalVariables.GravityForce;
            _boatForm.slopeGravityForce = globalVariables.SlopeGravityForce;
            
            _boatForm.allEnvironment = globalVariables.AllEnvironment;
            _boatForm.waterMask = globalVariables.WaterMask;
            _boatForm.stairsSlopeMask = globalVariables.StairsSlopeMask;
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