using Core.Service;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class EnemyView
    {
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
        private ICameraService _cameraService;

        private Transform _selfTransform;
        private Animator _animator;
        
        private Image _healthBarImage;
        private Image _secondHealthBarImage;
        private Canvas _canvas;
        
        private float _timeDecreaseHealthBase = 0.15f;
        private Ease _easeTypeBase = Ease.OutQuint;
        private float _timeDecreaseHealthSecond = 0.25f;
        private Ease _easeTypeSecond = Ease.InQuint;
        
        private Tween _baseTween;
        private Tween _secondTween;
        private Tween _hideTween;
        private Tween _blinkTween;

        private Renderer _renderer;

        private Material _material;

        public EnemyView(Transform selfTransform, ICameraService cameraService)
        {
            _cameraService = cameraService;
            _selfTransform = selfTransform;
            _animator = _selfTransform.GetComponent<Animator>();
            
            _canvas = selfTransform.Find("Canvas").GetComponent<Canvas>();
            _healthBarImage = _selfTransform.Find("Canvas/Image_Background/Image_HealthBar").GetComponent<Image>();
            _secondHealthBarImage = _selfTransform.Find("Canvas/Image_Background/Image_HealthBarSecond").GetComponent<Image>();
            _renderer = _selfTransform.Find("stickman").GetComponent<Renderer>();
            _material = _renderer.material;
        }

        public void SetRunAnimation()
        {
            _animator.SetBool("IsTargetFound", true);
        }

        public void SetIdleAnimation()
        {
            _animator.SetBool("IsTargetFound", false);
        }

        public void Damage(float damage)
        {
            AnimateHealth(damage);
            BlinkEmission();
        }

        public void FixedUpdate()
        {
            if(!_canvas.isActiveAndEnabled)
                return;
            
            RotateCanvasToCamera();
        }
        
        private void BlinkEmission()
        {
            var duration = 0.3f;
            _blinkTween?.Kill();
            
            Color targetColor = Color.white;

            _blinkTween = DOTween.Sequence()
                .Append(DOTween.To(
                    () => _material.GetColor(EmissionColor),
                    c => _material.SetColor(EmissionColor, c),
                    targetColor,
                    duration * 0.5f
                ))
                .Append(DOTween.To(
                    () => _material.GetColor(EmissionColor),
                    c => _material.SetColor(EmissionColor, c),
                    Color.black,
                    duration * 0.5f
                ))
                .SetEase(Ease.OutQuad);
        }
        
        private void AnimateHealth(float normalizedHealth)
        {
            _canvas.gameObject.SetActive(true);
            RotateCanvasToCamera();
            
            _baseTween?.Kill();
            _secondTween?.Kill();

            _baseTween = _healthBarImage.DOFillAmount(normalizedHealth, _timeDecreaseHealthBase).SetEase(_easeTypeBase);
            _secondTween = _secondHealthBarImage.DOFillAmount(normalizedHealth, _timeDecreaseHealthSecond).SetEase(_easeTypeSecond);
            _hideTween?.Kill();
            
            _hideTween = DOVirtual.DelayedCall(3, Hide);
        }

        private void Hide()
        {
            _canvas.gameObject.SetActive(false);
        }

        private void RotateCanvasToCamera()
        {
            _canvas.transform.LookAt(_cameraService.MainVirtualCamera.transform);
        }
    }
}