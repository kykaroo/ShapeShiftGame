using UnityEngine;

namespace Shop.ShopModelView
{
    public class SkinPlacement : MonoBehaviour
    {
        [SerializeField] private Rotator rotator;
        [SerializeField] private Camera modelViewCamera;
        [SerializeField] private float scrollAmount;

        public float maxCameraSize;
        public float minCameraSize;

        private GameObject _currentModel;

        public void InstantiateModel(GameObject model)
        {
            if (_currentModel != null)
            {
                Destroy(_currentModel.gameObject);
            }
            
            rotator.ResetRotation();
            _currentModel = Instantiate(model, transform);
        }

        private void Update()
        {
            if (Input.mouseScrollDelta.y > 0 && modelViewCamera.orthographicSize < maxCameraSize)
            {
                modelViewCamera.orthographicSize += scrollAmount;
            }

            if (Input.mouseScrollDelta.y < 0 && modelViewCamera.orthographicSize > minCameraSize)
            {
                modelViewCamera.orthographicSize -= scrollAmount;
            }
        }
    }
}