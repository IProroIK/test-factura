using Core.Controllers;
using Core.Items;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.View
{
    public class CarView
    {
        private CameraController _cameraController;
        private Transform _selfTransform;

        private Image _healthBarImage;
        private Image _secondHealthBarImage;
        private Canvas _canvas;
        private CarExplosion _carExplosionPrefab;

        private float _timeDecreaseHealthBase = 0.15f;
        private Ease _easeTypeBase = Ease.OutQuint;
        private float _timeDecreaseHealthSecond = 0.25f;
        private Ease _easeTypeSecond = Ease.InQuint;

        private Tween _baseTween;
        private Tween _secondTween;

        public CarView(Transform selfTransform, CameraController cameraController)
        {
            _carExplosionPrefab = Resources.Load<CarExplosion>("Prefabs/Gameplay/CarExplosion");
            _selfTransform = selfTransform;
            _cameraController = cameraController;
        }

        public void Init()
        {
            _healthBarImage = _selfTransform.Find("Canvas/Image_Background/Image_HealthBar").GetComponent<Image>();
            _secondHealthBarImage = _selfTransform.Find("Canvas/Image_Background/Image_HealthBarSecond")
                .GetComponent<Image>();
        }

        public void Damage(float normalizedHealth)
        {
            AnimateHealth(normalizedHealth);
            _cameraController.Shake(1.5f, 0.2f);
        }

        public void CarExplosion()
        {
            _selfTransform.gameObject.SetActive(false);
            GameObject.Instantiate(_carExplosionPrefab, _selfTransform.position, Quaternion.identity);
        }

        public void Reset()
        {
            _baseTween?.Kill();
            _secondTween?.Kill();
            _selfTransform.gameObject.SetActive(true);
            _healthBarImage.fillAmount = 1f;
            _secondHealthBarImage.fillAmount = 1f;
        }

        private void AnimateHealth(float normalizedHealth)
        {
            _baseTween?.Kill();
            _secondTween?.Kill();

            _baseTween = _healthBarImage.DOFillAmount(normalizedHealth, _timeDecreaseHealthBase).SetEase(_easeTypeBase);
            _secondTween = _secondHealthBarImage.DOFillAmount(normalizedHealth, _timeDecreaseHealthSecond)
                .SetEase(_easeTypeSecond);
        }
    }
}