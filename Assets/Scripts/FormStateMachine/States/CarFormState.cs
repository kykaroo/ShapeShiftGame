using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class CarFormState : FormStateBase
    {
        private readonly CarForm _carForm;
        private Rigidbody _playerBody;

        public CarFormState(CarForm carForm, GlobalVariables globalVariables)
        {
            _carForm = carForm;
            
            _carForm.playerBody = globalVariables.PlayerBody;
            _carForm.gravityForce = globalVariables.GravityForce;
            _carForm.slopeGravityForce = globalVariables.SlopeGravityForce;

            _carForm.allEnvironment = globalVariables.AllEnvironment;
            _carForm.waterMask = globalVariables.WaterMask;
            _carForm.stairsSlopeMask = globalVariables.StairsSlopeMask;
        }

        protected override void OnEnter()
        {
            _carForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _carForm.gameObject.SetActive(false);
        }
    }
}