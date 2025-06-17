using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Items
{

    public class Ground : MonoBehaviour, IPoolable
    {
        public event Action<IPoolable> Despawned;
        public event Action GroundTriggerEnteredEvent;
        
        [SerializeField] private List<GameObject> _groundObjects;

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out CarController carController))
            {
                Despawned?.Invoke(this);
                DisableGroundObjects();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out CarController carController))
            {
                GroundTriggerEnteredEvent?.Invoke();
                GroundTriggerEnteredEvent = null;
            }
        }

        private void OnEnable()
        {
            DisableGroundObjects();
            _groundObjects[Random.Range(0, _groundObjects.Count)].SetActive(true);
        }

        private void DisableGroundObjects()
        {
            foreach (var groundObject in _groundObjects)
            {
                groundObject.SetActive(false);
            }
        }
    }
}