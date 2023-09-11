﻿using UnityEngine;

namespace FormStateMachine.Forms
{
    public class HumanForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;
        
        private bool _applyGravity;
        private BoxCollider _collider;
        private float _maxSpeed;

        public Ground Ground;
        public float gravityForce;
        public Rigidbody playerBody;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void FixedUpdate()
        {
            Physics.SyncTransforms();
            ApplyGravity();
            HumanFormMovement();
            SpeedLimit();
            playerBody.transform.rotation = Quaternion.identity;
        }
        
        private void ApplyGravity()
        {
            if (Ground.SurfaceCollision(_collider.bounds, transform.rotation, Ground.AllEnvironment))
            { 
                playerBody.AddForce(Vector3.down * (gravityForce * 0.2f * playerBody.mass), ForceMode.Acceleration);
            }
            else
            {
                playerBody.AddForce(Vector3.down * (gravityForce * playerBody.mass), ForceMode.Acceleration);
            }
        }
        
        private void HumanFormMovement()
        {
            _maxSpeed = baseSpeed;
            var transformPosition = transform.position;
            var transformRotation = transform.rotation;
            var moveDirection = Ground.GetMoveDirection(transformPosition + Vector3.forward * 
                (transform.localScale.z * 0.55f));
            
            if (Ground.SurfaceCollision(_collider.bounds, transformRotation, Ground.WaterMask))
            {
                _maxSpeed = baseSpeed * 0.1f;
            }

            if (Ground.SurfaceCollision(_collider.bounds, transformRotation, Ground.AllEnvironment))
            {
                playerBody.AddForce(moveDirection * _maxSpeed, ForceMode.Acceleration);
            }
        }

        private void SpeedLimit()
        {
            Vector3 forwardVelocity = new Vector3(0, 0, playerBody.velocity.z);
            if (!(forwardVelocity.magnitude > _maxSpeed)) return;
            
            var velocity = playerBody.velocity;
            velocity = new(velocity.x, velocity.y,
                forwardVelocity.z * 0.4f);
            playerBody.velocity = velocity;
        }
    }
}