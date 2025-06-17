using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "GameData/TurretData")]
    public class TurretData : ScriptableObject
    {
        public float attackTimeDelay;
        public float damage;
    }
}