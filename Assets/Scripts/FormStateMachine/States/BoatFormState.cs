﻿using FormStateMachine.Forms;
using UnityEngine;

namespace FormStateMachine.States
{
    public class BoatFormState : FormStateBase
    {
        private readonly BoatForm _boatForm;
        private readonly Rigidbody _playerBody;

        public BoatFormState(BoatForm boatForm, GlobalVariables globalVariables, Ground ground, Rigidbody playerBody)
        {
            _boatForm = boatForm;
            _playerBody = playerBody;

            _boatForm.playerBody = playerBody;
            _boatForm.Ground = ground;
            _boatForm.gravityForce = globalVariables.GravityForce;
        }

        protected override void OnEnter()
        {
            _playerBody.constraints = RigidbodyConstraints.None;
            _playerBody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            _playerBody.transform.rotation = Quaternion.identity;
            _boatForm.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _boatForm.gameObject.SetActive(false);
        }
    }
}