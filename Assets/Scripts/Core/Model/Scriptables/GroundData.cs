using Core.Items;
using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "GroundData", menuName = "GameData/GroundData")]
    public class GroundData : ScriptableObject
    {
        public int LevelLength;
        public float GroundLength;
        public Finish EndGroundPrefab;
        public Ground GroundPrefab;
    }
}