using System;
using Cinemachine;
using Core.Service;
using DG.Tweening;
using Settings;
using UnityEngine;
using Zenject;

namespace Core.Controllers
{
    public class CameraController : IInitializable, IDisposable
    {
        private readonly Vector3 _topViewOffset = new(0, 30, 13.4f);
        private readonly Vector3 _topViewRot = new(60, 0, 0);

        private readonly Vector3 _carViewOffset = new(0, 9.2f, 13.4f);
        private readonly Vector3 _carViewRot = new(10, 0, 0);

        private const float duration = 0.6f;

        private CinemachineBasicMultiChannelPerlin _noise;
        private CinemachineTransposer _transposer;
        private CinemachineVirtualCamera _virtualCamera;
        private Sequence _shakeSequence;
        
        private readonly IAppStateService _appStateService;
        private readonly ICameraService _cameraService;

        private CameraController(IAppStateService appStateService, ICameraService cameraService)
        {
            _cameraService = cameraService;
            _appStateService = appStateService;
        }
        
        public void Initialize()
        {
            _virtualCamera = _cameraService.MainVirtualCamera;
            _noise = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
        }

        public void Dispose()
        {
            _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
        }

        public void AnimateToTopView(Action callback = null)
        {
            Animate(_topViewOffset, _topViewRot, callback);
        }

        public void AnimateToCarView(Action callback = null)
        {
            Animate(_carViewOffset, _carViewRot, callback);
        }

        private void Animate(Vector3 offset, Vector3 rotation, Action callback = null)
        {
            if (_transposer != null)
            {
                DOTween.To(() => _transposer.m_FollowOffset, x => _transposer.m_FollowOffset = x, offset, duration);
            }

            _virtualCamera.transform.DORotate(rotation, duration).OnComplete(() => callback?.Invoke());
        }

        public void Shake(float intensity, float duration)
        {
            _shakeSequence?.Kill();

            _noise.m_AmplitudeGain = intensity;
            _shakeSequence.Append(DOTween.To(
                () => _noise.m_AmplitudeGain,
                x => _noise.m_AmplitudeGain = x, 
                0f,
                duration
            ));


            _shakeSequence.AppendCallback(StopShake);
        }

        private void StopShake()
        {
            _noise.m_AmplitudeGain = 0;
            _shakeSequence?.Kill();
        }

        private void AppStateChangedEventHandler()
        {
            if (_appStateService.AppState == Enumerators.AppState.Main)
            {
                AnimateToTopView();
            }
        }
    }
}