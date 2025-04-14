using UnityEngine;
using _Project.Develop.Runtime.Controllers.Items;

namespace _Project.Develop.Runtime.Models
{
    [CreateAssetMenu(fileName = "New ConstructObjectData", menuName = "Construct Data", order = 51)]
    public class ConstructObjectData : ScriptableObject
    {
        [Header("Префаб призрака")]
        [SerializeField] private ItemFantom _itemFantom;

        [Header("Тег объекта для привязки")]
        [SerializeField] private LayerMask _layerMask;

        public ItemFantom ItemFantom => _itemFantom;
        public LayerMask LayerMask => _layerMask;
    }
}
