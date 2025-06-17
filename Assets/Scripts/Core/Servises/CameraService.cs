using Cinemachine;
using UnityEngine;

namespace Core.Service
{
    public class CameraService : ICameraService
    {
        private Camera _unityCamera;
        private CinemachineVirtualCamera _mainVirtualCamera;

        public CameraService()
        {
            _unityCamera = Camera.main;
            _mainVirtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        }

        public Camera UnityCamera => _unityCamera;

        public CinemachineVirtualCamera MainVirtualCamera => _mainVirtualCamera;
    }
}