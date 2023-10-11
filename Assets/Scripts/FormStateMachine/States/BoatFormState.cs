using FormStateMachine.Forms;
using Level;
using UnityEngine;

namespace FormStateMachine.States
{
    public class BoatFormState : FormStateBase
    {
        private readonly BoatForm _boatForm;
        private readonly Rigidbody _playerBody;
        private readonly ParticleSystem _poofParticleSystem;

        public BoatFormState(BoatForm boatForm, Ground ground, Rigidbody playerBody,
            ParticleSystem poofParticleSystem)
        {
            _boatForm = boatForm;
            _boatForm.gameObject.SetActive(false);
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;

            _boatForm.playerBody = playerBody;
            _boatForm.Ground = ground;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = (RigidbodyConstraints)98;
            _playerBody.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _boatForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _boatForm.gameObject.SetActive(false);
        }
        
        public override string GetFormName()
        {
            return _boatForm.Name;
        }
    }
}