﻿using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class HumanFormState : FormStateBase
    {
        private readonly HumanForm _humanForm;
        private Rigidbody _playerBody;
        private ParticleSystem _poofParticleSystem;

        public HumanFormState(HumanForm humanForm, Ground ground, Rigidbody playerBody,
            ParticleSystem poofParticleSystem, float gravityForce)
        {
            _humanForm = humanForm;
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;

            _humanForm.playerBody = playerBody;
            _humanForm.Ground = ground;
            _humanForm.gravityForce = gravityForce;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotation;
            _playerBody.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _humanForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _humanForm.gameObject.SetActive(false);
        }
    }
}