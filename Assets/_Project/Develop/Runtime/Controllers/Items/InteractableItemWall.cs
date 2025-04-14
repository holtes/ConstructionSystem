using UnityEngine;
using _Project.Develop.Runtime.Interfaces;
using _Project.Develop.Runtime.Models;

namespace _Project.Develop.Runtime.Controllers.Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class InteractableItemWall : MonoBehaviour, IPickable, IContructable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [SerializeField] private ConstructObjectData data;

        public void Drop(Transform parent)
        {
            transform.parent = parent;
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }

        public void PickUp(Transform parent)
        {
            transform.parent = parent;
            _collider.enabled = false;
        }

        public Transform GetTransform() => transform;

        public void StartConstruct(Transform parent) => _rigidbody.isKinematic = true;

        public void EndConstruct(Transform parent)
        {
            transform.parent = parent;
            _collider.enabled = true;
        }

        public void DeclineConstruct(Transform parent) => _rigidbody.isKinematic = false;

        public ConstructObjectData GetConstructData() => data;
    }
}

