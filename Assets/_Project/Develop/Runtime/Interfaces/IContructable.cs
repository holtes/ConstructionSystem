using UnityEngine;
using _Project.Develop.Runtime.Models;

namespace _Project.Develop.Runtime.Interfaces
{
    public interface IContructable
    {
        public void StartConstruct(Transform parent = null);
        public void EndConstruct(Transform parent = null);
        public void DeclineConstruct(Transform parent = null);
        public ConstructObjectData GetConstructData();
        public Transform GetTransform();
    }
}
