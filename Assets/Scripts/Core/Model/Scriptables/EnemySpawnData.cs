using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "EnemySpawnData", menuName = "GameData/EnemySpawnData")]
    public class EnemySpawnData : ScriptableObject
    {
        public Vector3 SpawnAreaSize;
        public Vector3 SpawnOffset;
        public Vector3 RandomOffset;
        public bool IsRandomRotation = true;
    }
}