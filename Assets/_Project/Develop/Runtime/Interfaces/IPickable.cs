using UnityEngine;

namespace _Project.Develop.Runtime.Interfaces
{
    public interface IPickable
    {
        public void PickUp(Transform parent);
        public void Drop(Transform parent);
        public Transform GetTransform();
    }
}
