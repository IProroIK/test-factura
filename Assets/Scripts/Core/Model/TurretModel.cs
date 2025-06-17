using UnityEngine;

namespace Model
{
    public class TurretModel
    {
        private TurretData _data;
        private float _lastShotTime;

        public float Damage => _data.damage;
        public float AttackSpeed => _data.attackTimeDelay;

        public TurretModel(TurretData data)
        {
            _data = data;
            _lastShotTime = -data.attackTimeDelay;
        }

        public bool CanShoot()
        {
            return Time.time - _lastShotTime >= _data.attackTimeDelay;
        }

        public void RegisterShot()
        {
            _lastShotTime = Time.time;
        }

        public float GetDamage()
        {
            return _data.damage;
        }
    }
}