using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HelicopterFormState : FormStateBase
    {
        private readonly HelicopterForm _helicopterForm;
        private Rigidbody _playerBody;

        public HelicopterFormState(HelicopterForm helicopterForm, GlobalVariables globalVariables)
        {
            _helicopterForm = helicopterForm;
            
            _helicopterForm.playerBody = globalVariables.PlayerBody;
            _helicopterForm.gravityForce = globalVariables.GravityForce;

            _helicopterForm.allEnvironment = globalVariables.AllEnvironment;
            _helicopterForm.balloonsMask = globalVariables.BalloonsMask;
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