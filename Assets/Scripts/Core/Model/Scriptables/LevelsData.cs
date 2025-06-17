using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "LevelsData", menuName = "GameData/LevelsData")]
    public class LevelsData : ScriptableObject
    {
        public int MaxLevelIndex;
        public List<EnemySpawnData> EnemySpawnsData;
        public List<EnemyData> EnemiesData;
        public List<GroundData> GroundsData;
    }
}