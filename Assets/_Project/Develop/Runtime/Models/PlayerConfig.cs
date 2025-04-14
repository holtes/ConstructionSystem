using UnityEngine;

namespace _Project.Develop.Runtime.Models
{
    [CreateAssetMenu(fileName = "New PlayerConfig", menuName = "PlayerConfig", order = 51)]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Чувствительность мыши")]
        [SerializeField] private float _mouseSensitivity = 100f;

        [Header("Скорость ходьбы")]
        [SerializeField] private float _playerSpeed = 5f;

        [Header("Дальность взаимодействия с объектами")]
        [SerializeField] private float _raycastLen = 100f;

        [Header("Шаг вращения объекта в режиме строительства")]
        [SerializeField] private float _rotationStep = 45f;

        [Header("Длительность притягивания объекта к игроку при подборе")]
        [SerializeField] private float _magnetDuration = 1f;

        public float MouseSensitivity => _mouseSensitivity;
        public float PlayerSpeed => _playerSpeed;
        public float RaycastLen => _raycastLen;
        public float RotationStep => _rotationStep;
        public float MagnetDuration => _magnetDuration;
    }
}
