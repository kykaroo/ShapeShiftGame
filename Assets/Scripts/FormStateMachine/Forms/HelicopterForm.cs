using UnityEngine;

namespace FormStateMachine.Forms
{
    public class HelicopterForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;
        [SerializeField] private float flyHeight;
        [SerializeField] private float upwardsSpeed;

        private RaycastHit _groundHit;

        public LayerMask allEnvironment;
        public LayerMask balloonsMask;
        public float gravityForce;
        public Rigidbody playerBody;

        private void FixedUpdate()
        {
            ApplyGravity();
            HelicopterFormMovement();
            HandleHelicopterHeight();
            SpeedLimit();
        }
        
        private void ApplyGravity()
        {
            playerBody.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
        }
        
        private void HelicopterFormMovement()
        {
            HandleHelicopterHeight();

            if (Physics.CheckBox(gameObject.transform.position,
                    gameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    balloonsMask))
            {
                playerBody.AddForce(Vector3.forward * (baseSpeed * 0.1f), ForceMode.Acceleration);
                return;
            }
        
            playerBody.AddForce(Vector3.forward * baseSpeed, ForceMode.Acceleration);
        }

        private void HandleHelicopterHeight()
        {
            Physics.Raycast(gameObject.transform.position, Vector3.down, out _groundHit,
                flyHeight * 10f, allEnvironment);
            if (_groundHit.distance <= flyHeight)
                playerBody.AddForce(Vector3.up * upwardsSpeed, ForceMode.Acceleration);
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