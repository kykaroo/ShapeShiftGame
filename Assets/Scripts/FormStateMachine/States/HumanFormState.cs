using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HumanFormState : FormStateBase
    {
        private readonly HumanForm _humanForm;
        private Rigidbody _playerBody;
        private ParticleSystem _poofParticleSystem;

        public HumanFormState(HumanForm humanForm, Ground ground, Rigidbody playerBody,
            ParticleSystem poofParticleSystem)
        {
            _humanForm = humanForm;
            _humanForm.gameObject.SetActive(false);
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;

            _humanForm.playerBody = playerBody;
            _humanForm.Ground = ground;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            _playerBody.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _humanForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _humanForm.gameObject.SetActive(false);
        }

        public override string GetFormName()
        {
            return _humanForm.Name;
        }
    }
}