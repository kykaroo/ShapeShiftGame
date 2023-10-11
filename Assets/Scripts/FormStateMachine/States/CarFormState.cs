﻿using FormStateMachine.Forms;
using Level;
using UnityEngine;

namespace FormStateMachine.States
{
    public class CarFormState : FormStateBase
    {
        private readonly CarForm _carForm;
        private readonly Rigidbody _playerBody;
        private readonly ParticleSystem _poofParticleSystem;

        public CarFormState(CarForm carForm, Ground ground, Rigidbody playerBody,
            ParticleSystem poofParticleSystem)
        {
            _carForm = carForm;
            _carForm.gameObject.SetActive(false);
            _playerBody = playerBody;
            _poofParticleSystem = poofParticleSystem;
            
            _carForm.playerBody = playerBody;
            _carForm.Ground = ground;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = (RigidbodyConstraints)98;
            _playerBody.rotation = Quaternion.identity;
            _poofParticleSystem.Play();
            _carForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _carForm.gameObject.SetActive(false);
        }
        
        public override string GetFormName()
        {
            return _carForm.Name;
        }
    }
}