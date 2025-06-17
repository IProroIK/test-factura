using System;
using DG.Tweening;
using UnityEngine;

namespace Core.Items
{
    public class CarExplosion : MonoBehaviour
    {
        [SerializeField] private float explosionForce = 0;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private ForceMode forceMode = ForceMode.Impulse;

        [SerializeField] private Rigidbody[] _parts;

        private void OnValidate()
        {
            _parts = GetComponentsInChildren<Rigidbody>();
        }

        private void OnEnable()
        {
            Explode();
        }

        private void Explode()
        {
            Vector3 explosionOrigin = transform.position;

            foreach (var part in _parts)
            {
                if (part != null)
                {
                    part.isKinematic = false;
                    Debug.Log("");
                    part.AddExplosionForce(explosionForce, explosionOrigin, explosionRadius, 0f, forceMode);
                }
            }
            
            DOVirtual.DelayedCall(7f, ()=> GameObject.Destroy(this.gameObject));
        }
    }
}