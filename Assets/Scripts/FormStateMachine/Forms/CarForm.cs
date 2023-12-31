﻿using Level;
using UnityEngine;

namespace FormStateMachine.Forms
{
    public class CarForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;
        [SerializeField] private BoxCollider _collider;

        private RaycastHit _surfaceHit;
        private float _maxSpeed;

        [HideInInspector]
        public float gravityForce;
        [HideInInspector]
        public Rigidbody playerBody;
        [HideInInspector]
        public Ground Ground;
        
        public string Name { get; private set; }

        private void Awake()
        {
            Name = "Car";
        }

        private void FixedUpdate()
        {
            Physics.SyncTransforms();
            ApplyGravity();
            CarFormMovement();
            SpeedLimit();
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

        private void CarFormMovement()
        {
            _maxSpeed = baseSpeed;
            var formTransform = transform;
            var moveDirection = Ground.GetMoveDirection(formTransform.position + Vector3.forward * 
                (formTransform.localScale.z * 0.55f));
            
            if (Ground.VerticalObstacleCheck(_collider.bounds, transform.forward, Ground.GroundMask)) return;

            if (Ground.SurfaceCollision(_collider.bounds, playerBody.transform.rotation, 
                    Ground.StairsSlopeMask + Ground.WaterMask))
            {
                _maxSpeed = baseSpeed * 0.3f;
            }

            if (Ground.SurfaceCollision(_collider.bounds, playerBody.transform.rotation, Ground.AllEnvironment))
            {
                playerBody.AddForce(moveDirection * _maxSpeed, ForceMode.Acceleration);
            }
        }

        private void SpeedLimit()
        {
            var forwardVelocity = new Vector3(0, 0, playerBody.velocity.z);
            if (!(forwardVelocity.magnitude > _maxSpeed)) return;

            var velocity = playerBody.velocity;
            velocity = new(velocity.x, velocity.y, forwardVelocity.z * 0.4f);
            playerBody.velocity = velocity;
        }
    }
}