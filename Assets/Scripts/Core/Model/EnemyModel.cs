using System;
using UnityEngine;

namespace Model
{
    public class EnemyModel
    {
        public event Action DeathEvent;
        
        private EnemyData _enemyData;
        private float _currentHealth;

        public EnemyModel(EnemyData enemyData)
        {
            _enemyData = enemyData;
        }
        public void Init()
        {
            
        }
        
        public float GetDamage()
        {
            return _enemyData.Damage;
        }

        public void SetStartValue()
        {
            _currentHealth = _enemyData.Health;
        }

        public void Damage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                DeathEvent?.Invoke();
            }
        }

        public float GetNormalizedHealth()
        {
            return _currentHealth/_enemyData.Health;
        }

        public float GetSpeed()
        {
            return _enemyData.Speed;
        }
    }
}