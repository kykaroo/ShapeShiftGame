using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HumanFormState : FormStateBase
    {
        private readonly HumanForm _humanForm;
        private Rigidbody _playerBody;

        public HumanFormState(HumanForm humanForm, GlobalVariables globalVariables, Ground ground, Rigidbody playerBody)
        {
            _humanForm = humanForm;
            _playerBody = playerBody;

            _humanForm.playerBody = playerBody;
            _humanForm.Ground = ground;
            _humanForm.gravityForce = globalVariables.GravityForce;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotation;
            _playerBody.transform.rotation = Quaternion.identity;
            _humanForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _humanForm.gameObject.SetActive(false);
        }
    }
}