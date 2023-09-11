using UnityEngine;

namespace FormStateMachine.States
{
    public class NoneFormState : FormStateBase
    {
        private readonly Rigidbody _playerBody;
        public NoneFormState(Rigidbody playerBody)
        {
            _playerBody = playerBody;
        }
        
        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotation;
            _playerBody.velocity = Vector3.zero;
            _playerBody.transform.rotation = Quaternion.identity;
        }

        protected override void OnExit()
        {
            
        }
    }
}