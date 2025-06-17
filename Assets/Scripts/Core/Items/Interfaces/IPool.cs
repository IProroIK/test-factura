using UnityEngine;

namespace Core.Items
{
    public interface IPool<T> where T : Component
    {
        T Spawn(Vector3 position, Quaternion rotation);
        void Despawn(IPoolable instance);
    }
}