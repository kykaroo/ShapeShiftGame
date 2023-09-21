using UnityEngine;

namespace Shop.ShopModelView
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        private float _currentRotation;

        private void FixedUpdate()
        {
            _currentRotation -= Time.fixedDeltaTime * rotationSpeed;
            transform.rotation = Quaternion.Euler(0, _currentRotation, 0);
        }

        public void ResetRotation() => _currentRotation = 0;
    }
}