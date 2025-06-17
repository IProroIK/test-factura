using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class CarModel
    {
        public event Action DeathEvent;
        
        private CarData _carData;
        private float _health;
        private Transform _transform;
        private Vector3 _startPosition;
        
        public void Init(Transform carTransform)
        {
            _transform = carTransform;
            _carData = Resources.Load<CarData>("Data/CarData");
            _health = _carData.HealthPoints;
            _startPosition = _transform.position;
        }

        public float GetSpeed()
        {
            return _carData.Speed;
        }

        public float GetMaxSpeed()
        {
            return _carData.MaxSpeed;
        }

        public void Damage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                DeathEvent?.Invoke();
            }
        }
        
        public float GetNormalizedHealth()
        {
            return _health/_carData.HealthPoints;
        }

        public void Reset()
        {
            _health = _carData.HealthPoints;
            _transform.position = _startPosition;
        }
    }
}