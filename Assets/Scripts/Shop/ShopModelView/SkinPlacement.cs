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
            var orthographicSize = modelViewCamera.orthographicSize;
            switch (Input.mouseScrollDelta.y)
            {
                case > 0 when orthographicSize < maxCameraSize:
                    orthographicSize += scrollAmount;
                    modelViewCamera.orthographicSize = orthographicSize;
                    break;
                case < 0 when orthographicSize > minCameraSize:
                    orthographicSize -= scrollAmount;
                    modelViewCamera.orthographicSize = orthographicSize;
                    break;
            }
        }
    }
}