using UnityEngine;

namespace Model
{
    [CreateAssetMenu(fileName = "CarData", menuName = "GameData/CarData")]
    public class CarData : ScriptableObject
    {
        public float Speed;
        public float HealthPoints;
        public float MaxSpeed;
    }
}