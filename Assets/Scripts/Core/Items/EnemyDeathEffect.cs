using System;
using UnityEngine;

namespace Core.Items
{
    public class EnemyDeathEffect : MonoBehaviour, IPoolable
    {
        public event Action<IPoolable> Despawned;
        [SerializeField] private ParticleSystem _deathParticles;

        private void OnParticleSystemStopped()
        {
            Despawned?.Invoke(this);
        }
    }
}