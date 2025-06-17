using System;
using Core.Service;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.Items
{
    public class DamageText : MonoBehaviour, IPoolable
    {
        [Inject] private ICameraService _cameraService;
        public event Action<IPoolable> Despawned;
        private TextMeshPro _text;
        private Tween _textAnim;
        
        public void Awake()
        {
            _text = GetComponent<TextMeshPro>();
        }

        public void AnimateText(float textValue)
        {
            _textAnim?.Kill();
            gameObject.SetActive(true);
            _text.text = textValue.ToString();
            _textAnim = transform.DOLocalMoveY(transform.localPosition.y + 3, 0.7f)
                .OnComplete(()=> Despawned?.Invoke(this));
        }

        private void Update()
        {
            transform.LookAt(_cameraService.MainVirtualCamera.transform);
            transform.Rotate(0, 180f, 0);
        }
    }
}