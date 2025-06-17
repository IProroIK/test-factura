using System;
using System.Collections.Generic;
using Core.Items;
using UnityEngine;
using Zenject;
using IPoolable = Core.Items.IPoolable;

namespace Core.Service
{
    public class PoolService : IPoolService
    {
        private readonly Dictionary<Type, object> _pools = new();
        [Inject] private DiContainer _container;    
        
        public void CreatePool<T>(T prefab, int initialSize = 0) where T : Component, IPoolable
        {
            var type = typeof(T);

            if (_pools.ContainsKey(type))
                return;

            var parent = new GameObject("Pool_" +type.Name);
            var pool = _container.Instantiate<ObjectPool<T>>(
                new object[] { prefab, _container, initialSize, parent.transform });
            _pools[type] = pool;
        }

        public IPool<T> GetPool<T>() where T : Component
        {
            var type = typeof(T);

            if (_pools.TryGetValue(type, out var pool))
            {
                return pool as IPool<T>;
            }

            throw new Exception($"No pool found for type {type}. Did you forget to call CreatePool?");
        }
    }
}