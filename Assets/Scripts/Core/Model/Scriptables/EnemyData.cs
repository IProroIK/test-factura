using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "GameData/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        public float Damage;
        public float Health;
        public float Speed;
    }
}