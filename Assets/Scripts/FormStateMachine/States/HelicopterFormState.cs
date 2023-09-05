using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HelicopterFormState : FormStateBase
    {
        private readonly HelicopterForm _helicopterForm;
        private readonly Rigidbody _playerBody;

        public HelicopterFormState(HelicopterForm helicopterForm, GlobalVariables globalVariables, Ground ground,
            Rigidbody playerBody)
        {
            _helicopterForm = helicopterForm;
            _playerBody = playerBody;

            _helicopterForm.Ground = ground;
            _helicopterForm.playerBody = playerBody;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotation;
            _playerBody.transform.rotation = Quaternion.identity;
            _helicopterForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _helicopterForm.gameObject.SetActive(false);
        }
    }
}