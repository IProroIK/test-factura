using System;
using DG.Tweening;
using Settings;
using UnityEngine;

namespace Core.Items
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        public event Action<IPoolable> Despawned;

        [SerializeField] private ParticleSystem _particleEffect;
        [SerializeField] private Rigidbody _rigidbody;
        
        private float _lifeTimeTimer;
        private float _damage;

        public void Shot(Vector3 direction)
        {
            _particleEffect.Clear(true);
            _rigidbody.AddForce(direction * 50, ForceMode.Impulse);
        }

        public void SetData(float damage)
        {
            _damage = damage;
        }

        private void Update()
        {
            _lifeTimeTimer -= Time.deltaTime;

            if (_lifeTimeTimer <= 0)
            {
                Despawned?.Invoke(this);
            }            
        }

        private void OnEnable()
        {
            _lifeTimeTimer = Constants.BulletLifeTime;
        }

        private void OnDisable()
        {
            _rigidbody.velocity = Vector3.zero;
            _particleEffect.Clear(true);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log(other.gameObject.name);

                damageable.Damage(_damage);
            }
            
            Despawned?.Invoke(this);
        }
    }
}