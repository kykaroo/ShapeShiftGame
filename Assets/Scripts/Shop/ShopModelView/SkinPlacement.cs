using System;
using UnityEngine;

namespace Shop.ShopModelView
{
    public class SkinPlacement : MonoBehaviour
    {
        [SerializeField] private Rotator rotator;
        [SerializeField] private Camera modelViewCamera;

        public float maxCameraSize;
        public float minCameraSize;

        private GameObject _currentModel;
        private float _originalCameraSize;

        private void Awake()
        {
            _originalCameraSize = modelViewCamera.orthographicSize;
        }

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
                modelViewCamera.orthographicSize += 0.1f;
            }

            if (Input.mouseScrollDelta.y < 0 && modelViewCamera.orthographicSize > minCameraSize)
            {
                modelViewCamera.orthographicSize -= 0.1f;
            }
        }
    }
}