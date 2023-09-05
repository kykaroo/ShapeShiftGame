using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HelicopterFormState : FormStateBase
    {
        private readonly HelicopterForm _helicopterForm;
        private Rigidbody _playerBody;

        public HelicopterFormState(HelicopterForm helicopterForm, GlobalVariables globalVariables, Ground ground)
        {
            _helicopterForm = helicopterForm;
            
            _helicopterForm.playerTransform = globalVariables.PlayerTransform;

            _helicopterForm.allEnvironment = globalVariables.AllEnvironment;
            _helicopterForm.balloonsMask = globalVariables.BalloonsMask;
            _helicopterForm.groundMask = globalVariables.GroundMask;
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