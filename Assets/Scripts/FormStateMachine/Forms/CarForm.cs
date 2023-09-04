using UnityEngine;

namespace FormStateMachine.Forms
{
    public class CarForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;

        private RaycastHit _surfaceHit;
        private bool _useSlopeGravity;
        
        public LayerMask allEnvironment;
        public LayerMask stairsSlopeMask;
        public LayerMask waterMask;
        public float gravityForce;
        public float slopeGravityForce;
        public Rigidbody playerBody;

        private void FixedUpdate()
        {
            ApplyGravity();
            CarFormMovement();
            SpeedLimit();
        }

        private void ApplyGravity()
        {
            if (_useSlopeGravity)
            {
                playerBody.AddForce(Vector3.down * slopeGravityForce, ForceMode.Acceleration);
            }
            else
            {
                playerBody.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
            }
        }

        private void CarFormMovement()
        {
            if (Physics.CheckBox(gameObject.transform.position,
                    gameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    stairsSlopeMask))
            {
                playerBody.AddForce(GetMoveDirection() * (baseSpeed * 0.1f), ForceMode.Acceleration);
                _useSlopeGravity = true;
                return;
            }

            _useSlopeGravity = false;

            if (Physics.CheckBox(gameObject.transform.position,
                    gameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    waterMask))
            {
                playerBody.AddForce(GetMoveDirection() * (baseSpeed * 0.1f), ForceMode.Acceleration);
                return;
            }
        
            playerBody.AddForce(GetMoveDirection() * baseSpeed, ForceMode.Acceleration);
        }

        private Vector3 GetMoveDirection()
        {
            return Physics.Raycast(transform.position + Vector3.forward * (transform.localScale.z * 0.55f), 
                Vector3.down, out _surfaceHit, 5f, allEnvironment)
                ? Vector3.ProjectOnPlane(Vector3.forward, _surfaceHit.normal).normalized
                : Vector3.forward;
        }

        private void SpeedLimit()
        {
            Vector3 forwardVelocity = new Vector3(0, 0, playerBody.velocity.z);
            if (!(forwardVelocity.magnitude > baseSpeed)) return;
            
            var velocity = playerBody.velocity;
            velocity = new(velocity.x, velocity.y,
                forwardVelocity.normalized.z * baseSpeed);
            playerBody.velocity = velocity;
        }
    }
}