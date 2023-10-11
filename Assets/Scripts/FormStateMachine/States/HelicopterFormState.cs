using FormStateMachine.Forms;
using Level;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HelicopterFormState : FormStateBase
    {
        private readonly HelicopterForm _helicopterForm;
        private readonly Rigidbody _playerBody;
        private readonly ParticleSystem _poofParticleSystem;

        public HelicopterFormState(HelicopterForm helicopterForm, Ground ground,
            Rigidbody playerBody, ParticleSystem poofParticleSystem)
        {
            _helicopterForm = helicopterForm;
            _helicopterForm.gameObject.SetActive(false);
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;

            _helicopterForm.Ground = ground;
            _helicopterForm.playerBody = playerBody;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = (RigidbodyConstraints)114;
            _playerBody.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _helicopterForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _helicopterForm.gameObject.SetActive(false);
        }
        
        public override string GetFormName()
        {
            return _helicopterForm.Name;
        }
    }
}