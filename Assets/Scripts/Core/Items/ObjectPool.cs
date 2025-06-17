using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Items
{
    public class ObjectPool<T> : IPool<T> where T : Component, IPoolable
    {
        private readonly T _prefab;
        private readonly Queue<T> _pool = new();
        private readonly Transform _parent;
        [Inject] private DiContainer _container;


        public ObjectPool(T prefab, DiContainer container, int initialSize = 0, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _container = container;

            for (int i = 0; i < initialSize; i++)
            {
                var obj = _container.InstantiatePrefabForComponent<T>(_prefab, _parent);
                obj.Despawned += Despawn;
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        public T Spawn(Vector3 position, Quaternion rotation)
        {
            T instance;

            instance = _pool.Count > 0
                ? _pool.Dequeue()
                : _container.InstantiatePrefabForComponent<T>(_prefab, _parent);

            instance.transform.SetPositionAndRotation(position, rotation);
            instance.gameObject.SetActive(true);

            return instance;
        }

        public void Despawn(IPoolable instance)
        {
            if (instance is not T tinstance)
                return;

            tinstance.gameObject.SetActive(false);
            _pool.Enqueue(tinstance);
        }
    }
}