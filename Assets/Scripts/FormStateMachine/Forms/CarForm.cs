using UnityEngine;

namespace FormStateMachine.Forms
{
    public class CarForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;

        private RaycastHit _surfaceHit;
        private BoxCollider _collider;
        private float _maxSpeed;

        public float gravityForce;
        public Rigidbody playerBody;
        public Ground Ground;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void Update()
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
                playerBody.AddForce(Vector3.down * (gravityForce * 0.2f), ForceMode.Acceleration);
            }
            else
            {
                playerBody.AddForce(Vector3.down * gravityForce, ForceMode.Acceleration);
            }
        }

        private void CarFormMovement()
        {
            _maxSpeed = baseSpeed;
            var moveDirection = Ground.GetMoveDirection(transform.position + Vector3.forward * 
                (transform.localScale.z * 0.55f));
            
            if (Ground.VerticalObstacleCheck(_collider.bounds, transform.forward, Ground.GroundMask)) return;

            if (Ground.SurfaceCollision(_collider.bounds, playerBody.transform.rotation, 
                    Ground.StairsSlopeMask + Ground.WaterMask))
            {
                _maxSpeed = baseSpeed * 0.1f;
            }

            if (Ground.SurfaceCollision(_collider.bounds, playerBody.transform.rotation, Ground.AllEnvironment))
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
                forwardVelocity.z * 0.9f);
            playerBody.velocity = velocity;
        }
    }
}