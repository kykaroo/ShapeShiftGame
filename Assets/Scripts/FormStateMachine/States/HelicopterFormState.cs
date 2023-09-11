using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HelicopterFormState : FormStateBase
    {
        private readonly HelicopterForm _helicopterForm;
        private readonly Rigidbody _playerBody;
        private ParticleSystem _poofParticleSystem;

        public HelicopterFormState(HelicopterForm helicopterForm, Ground ground,
            Rigidbody playerBody, ParticleSystem poofParticleSystem, float gravityForce)
        {
            _helicopterForm = helicopterForm;
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;

            _helicopterForm.Ground = ground;
            _helicopterForm.playerBody = playerBody;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotation;
            _playerBody.transform.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _helicopterForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _helicopterForm.gameObject.SetActive(false);
        }
    }
}