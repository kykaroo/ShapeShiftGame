using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class CarFormState : FormStateBase
    {
        private readonly CarForm _carForm;
        private readonly Rigidbody _playerBody;

        public CarFormState(CarForm carForm, GlobalVariables globalVariables, Ground ground, Rigidbody playerBody)
        {
            _carForm = carForm;
            _playerBody = playerBody;
            
            _carForm.playerBody = playerBody;
            _carForm.Ground = ground;
            _carForm.gravityForce = globalVariables.GravityForce;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.None;
            _playerBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            _playerBody.transform.rotation = Quaternion.identity;
            _carForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _carForm.gameObject.SetActive(false);
        }
    }
}