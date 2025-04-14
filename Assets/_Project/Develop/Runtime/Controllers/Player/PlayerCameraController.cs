using UnityEngine;
using UniRx;
using Zenject;
using _Project.Develop.Runtime.Models;
using _Project.Develop.Runtime.Interfaces;
using _Project.Develop.Runtime.Signals;

namespace _Project.Develop.Runtime.Controllers.Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform _playerCamera;
        
        private float _mouseSensitivity;
        private float _raycastLen;
        private SignalBus _signalBus;

        private float xRotation = 0f;

        [Inject]
        private void Construct(SignalBus signalBus, PlayerConfig playerConfig)
        {
            _signalBus = signalBus;
            _mouseSensitivity = playerConfig.MouseSensitivity;
            _raycastLen = playerConfig.RaycastLen;
        }

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            Observable.EveryUpdate()
                .Subscribe(_ => Look())
                .AddTo(this);
        }

        private void Look()
        {
            var mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            _playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        public RaycastHit Raycast(LayerMask? includeMask = null)
        {
            var ray = new Ray(_playerCamera.position, _playerCamera.forward);
            RaycastHit hit;
            var finalMask = includeMask ?? Physics.DefaultRaycastLayers;

            Physics.Raycast(ray, out hit, _raycastLen, finalMask);
            Debug.DrawRay(_playerCamera.position, _playerCamera.forward * _raycastLen, Color.red, 1f);
            return hit;
        }

        public RaycastHit Raycast(out Ray ray, LayerMask? includeMask = null)
        {
            ray = new Ray(_playerCamera.position, _playerCamera.forward);
            RaycastHit hit;
            var finalMask = includeMask ?? Physics.DefaultRaycastLayers;

            Physics.Raycast(ray, out hit, _raycastLen, finalMask);
            Debug.DrawRay(ray.origin, ray.direction * _raycastLen, Color.red, 1f);

            return hit;
        }

        public void CheckIteractableItem()
        {
            var hit = Raycast();
            if (hit.collider != null)
            {
                IPickable pickable;
                IContructable contructable;
                if (hit.collider.TryGetComponent(out pickable)) _signalBus.Fire(new OnPickUpSignal(pickable));
                if (hit.collider.TryGetComponent(out contructable)) _signalBus.Fire(new OnConstructStartSignal(contructable));
            }
        }

        public float GetRaycastLen() => _raycastLen;
    }
}
