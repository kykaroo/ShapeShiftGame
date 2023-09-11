using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class BoatFormState : FormStateBase
    {
        private readonly BoatForm _boatForm;
        private readonly Rigidbody _playerBody;
        private ParticleSystem _poofParticleSystem;

        public BoatFormState(BoatForm boatForm, Ground ground, Rigidbody playerBody,
            ParticleSystem poofParticleSystem, float gravityForce)
        {
            _boatForm = boatForm;
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;

            _boatForm.playerBody = playerBody;
            _boatForm.Ground = ground;
            _boatForm.gravityForce = gravityForce;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            _playerBody.transform.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _boatForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _boatForm.gameObject.SetActive(false);
        }
    }
}