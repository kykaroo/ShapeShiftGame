﻿using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class CarFormState : FormStateBase
    {
        private readonly CarForm _carForm;
        private readonly Rigidbody _playerBody;
        private ParticleSystem _poofParticleSystem;

        public CarFormState(CarForm carForm, Ground ground, Rigidbody playerBody,
            ParticleSystem poofParticleSystem, float gravityForce)
        {
            _carForm = carForm;
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;
            
            _carForm.playerBody = playerBody;
            _carForm.Ground = ground;
            _carForm.gravityForce = gravityForce;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            _playerBody.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _carForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _carForm.gameObject.SetActive(false);
        }
    }
}