using UnityEngine;

namespace FormStateMachine.Forms
{
    public class CarForm : MonoBehaviour
    {
        [SerializeField] private float baseSpeed;

        private RaycastHit _surfaceHit;

        public LayerMask allEnvironment;
        public LayerMask stairsSlopeMask;
        public LayerMask waterMask;
        public LayerMask groundMask;
        public float gravityForce;
        public Transform playerTransform;

        private void Update()
        {
            ApplyGravity();
            CarFormMovement();
        }

        private void ApplyGravity()
        {
            if (!Physics.CheckBox(transform.position,
                    transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    allEnvironment))
            { 
                playerTransform.Translate(Vector3.down * (gravityForce * Time.deltaTime));
            }
        }

        private void CarFormMovement()
        {
            if (Physics.Raycast(transform.position + Vector3.forward * (transform.localScale.z * 0.5f), Vector3.forward,
                    0.1f, groundMask)) return;

            if (Physics.CheckBox(transform.position,
                    transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    stairsSlopeMask))
            {
                playerTransform.Translate(GetMoveDirection() * (baseSpeed * 0.1f * Time.deltaTime));
                return;   
            }
            
        
            if (Physics.CheckBox(gameObject.transform.position,
                    transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                    waterMask))
            {
                playerTransform.Translate(GetMoveDirection() * (baseSpeed * 0.1f * Time.deltaTime));
                return;
            }
        
            playerTransform.Translate(GetMoveDirection() * (baseSpeed * Time.deltaTime));
        }
        
        private Vector3 GetMoveDirection()
        {
            return Physics.Raycast(transform.position + Vector3.forward * (transform.localScale.z * 0.55f), 
                Vector3.down, out _surfaceHit, 5f, allEnvironment)
                ? Vector3.ProjectOnPlane(Vector3.forward, _surfaceHit.normal).normalized
                : Vector3.forward;
        }
    }
}