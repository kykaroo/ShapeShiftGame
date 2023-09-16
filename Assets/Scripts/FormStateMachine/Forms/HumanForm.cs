using System;
using UnityEngine;

namespace FormStateMachine.Forms
{
    public class HumanForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;
        [SerializeField] private BoxCollider _collider;

        public BoxCollider Collider => _collider;

        private bool _applyGravity;
        private float _maxSpeed;
        
        [HideInInspector]
        public Ground Ground;
        [HideInInspector]
        public float gravityForce;
        [HideInInspector]
        public Rigidbody playerBody;
        
        public string Name { get; private set; }

        private void Awake()
        {
            Name = "Human";
        }

        private void FixedUpdate()
        {
            Physics.SyncTransforms();
            ApplyGravity();
            HumanFormMovement();
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