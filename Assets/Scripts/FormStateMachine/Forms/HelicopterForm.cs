using Level;
using UnityEngine;

namespace FormStateMachine.Forms
{
    public class HelicopterForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private float maxHeightSpeedMultiplier;
        [SerializeField] private float flyHeight;
        [SerializeField] private float upwardsSpeed;
        [SerializeField] private Animator animator;

        private RaycastHit _surfaceHit;
        private float _maxSpeed;
        private Vector3 _currentVelocity;
        private bool _onMaxHeight;

        [HideInInspector]
        public Rigidbody playerBody;
        [HideInInspector]
        public Ground Ground;
        [HideInInspector]
        public Vector3 velocity;
        
        public string Name { get; private set; }

        private static readonly int OnMaxHeight = Animator.StringToHash("onMaxHeight");

        private void Awake()
        {
            Name = "Helicopter";
        }

        private void FixedUpdate()
        {
            velocity = playerBody.velocity;
            Physics.SyncTransforms();
            HelicopterFormMovement();
            HandleHeight();
            HandleMaxHeightRotation();
            SpeedLimit();
        }

        private void HandleMaxHeightRotation()
        {
            animator.SetBool(OnMaxHeight, _onMaxHeight);
        }

        private void HelicopterFormMovement()
        {
            _maxSpeed = baseSpeed;
            if (Ground.VerticalObstacleCheck(_collider.bounds, transform.forward, Ground.AllEnvironment)) return;

            if (Ground.SurfaceCollision(_collider.bounds, transform.rotation, Ground.BalloonsMask))
            {
                _maxSpeed = baseSpeed * 0.1f;
            }

            if (_onMaxHeight)
            {
                _maxSpeed = baseSpeed * maxHeightSpeedMultiplier;
            }
            playerBody.AddForce(Vector3.forward * _maxSpeed, ForceMode.Acceleration);
        }

        private void HandleHeight()
        {
            Physics.BoxCast(_collider.bounds.center, transform.lossyScale * 0.5f, Vector3.down, out _surfaceHit);
            if (_surfaceHit.distance < flyHeight * 0.98f)
            {
                _onMaxHeight = false;
                playerBody.AddForce(Vector3.up * upwardsSpeed);
                return;
            }
            
            _onMaxHeight = true;

            if (_surfaceHit.distance > flyHeight * 1.02f)
            {
                playerBody.AddForce(Vector3.down * upwardsSpeed);
                return;
            }

            if (_surfaceHit.distance > flyHeight * 0.98f && _surfaceHit.distance < flyHeight * 1.02f)
            {
                playerBody.velocity = new(velocity.x, 0, velocity.z);
            }
        }
        
        private void SpeedLimit()
        {
            HorizontalSpeedLimit();
        }

        private void HorizontalSpeedLimit()
        {
            var forwardVelocity = new Vector3(0, 0, playerBody.velocity.z);
            if (!(forwardVelocity.magnitude > _maxSpeed)) return;

            var playerVelocity = playerBody.velocity;
            
            playerVelocity = new(playerVelocity.x, playerVelocity.y,
                forwardVelocity.z * 0.9f);
            playerBody.velocity = playerVelocity;
        }
    }
}