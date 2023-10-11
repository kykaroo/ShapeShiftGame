using UnityEngine;
using Zenject;

namespace Player
{
    public class CameraHolder : ITickable
    {
        private readonly Transform _cameraHolderGo;
        private readonly Transform _playerTransform;
        private readonly Camera _camera;
        private readonly Transform _cameraGamePosition;

        [Inject]
        public CameraHolder(Transform cameraHolderGo, Transform playerTransform, Camera camera, Transform cameraGamePosition)
        {
            _cameraHolderGo = cameraHolderGo;
            _camera = camera;
            _cameraGamePosition = cameraGamePosition;
            _playerTransform = playerTransform;
        }

        public void Tick()
        {
            _cameraHolderGo.localEulerAngles = -_playerTransform.eulerAngles;
        }

        public void ReleaseCamera()
        {
            _cameraHolderGo.DetachChildren();
        }

        public void ResetCamera()
        {
            var cameraTransform = _camera.transform;
            cameraTransform.SetParent(_cameraHolderGo, true);
            cameraTransform.position = _cameraGamePosition.position;
        }
    }
}