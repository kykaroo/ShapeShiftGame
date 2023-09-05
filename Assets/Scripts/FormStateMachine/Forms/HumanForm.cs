using System;
using UnityEngine;

namespace FormStateMachine.Forms
{
    public class HumanForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;
        
        public Ground Ground;
        private bool _applyGravity;
        private Bounds _bounds;
        
        public LayerMask stairsSlopeMask;
        public LayerMask allEnvironment;
        public LayerMask waterMask;
        public LayerMask groundMask;
        public float gravityForce;
        public Transform playerTransform;

        private void Start()
        {
            var transform1 = transform;
            _bounds = new(transform1.position, transform1.localScale);
        }

        private void Update()
        {
            ApplyGravity();
            HumanFormMovement();
        }
        
        private void ApplyGravity()
        {
            if (Ground.CanMoveTo(_bounds, transform.position,  Vector3.down * (gravityForce * Time.deltaTime), Ground.GroundMask))
            { 
                playerTransform.Translate(Vector3.down * (gravityForce * Time.deltaTime));
            }
        }
        
        private void HumanFormMovement()
        {
            var transformPosition = transform.position;
            var moveDirection = Ground.GetMoveDirection(transformPosition + Vector3.forward * 
                (transform.localScale.z * 0.55f));

            var moveDistance = baseSpeed * Time.deltaTime;

            if (Ground.CanMoveTo(_bounds, transformPosition, Vector3.down * moveDistance, Ground.GroundMask))
            {
                playerTransform.Translate(Vector3.down * moveDistance);
            }

            if (Ground.CanMoveTo(_bounds, transformPosition, moveDirection * (moveDistance * 0.1f), Ground.WaterMask))
            {
                moveDistance *= 0.1f;
            }
            
            if (Ground.CanMoveTo(_bounds, transformPosition, moveDirection * moveDistance, Ground.StairsSlopeMask + Ground.GroundMask))
            {
                playerTransform.Translate(moveDirection * moveDistance);
            }
        }
    }
}