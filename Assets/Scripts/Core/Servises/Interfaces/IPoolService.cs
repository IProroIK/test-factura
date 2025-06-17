using Core.Items;
using UnityEngine;

namespace Core.Service
{
    public interface IPoolService
    {
        void CreatePool<T>(T prefab, int initialSize = 0) where T : Component, IPoolable;
        IPool<T> GetPool<T>() where T : Component;
    }
}