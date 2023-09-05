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
        public LayerMask groundMask;
        public Transform playerTransform;

        private void Update()
        {
            HelicopterFormMovement();
            HandleHelicopterHeight();
        }

        private void HelicopterFormMovement()
        {
            if (Physics.Raycast(transform.position + Vector3.forward * (transform.localScale.z * 0.5f), Vector3.forward,
                    1f, groundMask)) return;
            
            if (Physics.CheckBox(transform.position,
                    transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    balloonsMask))
            {
                playerTransform.Translate(Vector3.forward * (baseSpeed * 0.1f * Time.deltaTime));
                return;
            }
        
            playerTransform.Translate(Vector3.forward * (baseSpeed * Time.deltaTime));
        }

        private void HandleHelicopterHeight()
        {
            Physics.Raycast(transform.position, Vector3.down, out _groundHit,
                flyHeight * 10f, allEnvironment);
            if (_groundHit.distance < flyHeight - 0.001f)
            {
                playerTransform.Translate(Vector3.up * (upwardsSpeed * Time.deltaTime));
            }

            if (_groundHit.distance > flyHeight + 0.001f)
            {
                playerTransform.Translate(Vector3.down * (upwardsSpeed * Time.deltaTime));
            }
        }
    }
}