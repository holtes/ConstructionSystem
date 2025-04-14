using UnityEngine;
using System;
using Zenject;
using UniRx;
using _Project.Develop.Runtime.Controllers.Player;
using _Project.Develop.Runtime.Controllers.Items;
using _Project.Develop.Runtime.Interfaces;
using _Project.Develop.Runtime.Models;
using _Project.Develop.Runtime.Signals;

namespace _Project.Develop.Runtime.Managers
{
    public class ConstructManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private PlayerCameraController _cameraController;
        private Transform _sceneOrigin;
        private float _rotationStep;

        private ReactiveProperty<bool> _isHitDetected = new(false);
        private ItemFantom _itemFantom;
        private IContructable _contructableItem;
        private IDisposable _constructControlStream;
        private IDisposable _isHitDetectedPropertyStream;


        [Inject]
        private void Construct(SignalBus signalBus, PlayerController player, Transform sceneOrigin, PlayerConfig playerConfig)
        {
            _signalBus = signalBus;
            _cameraController = player.GetCameraController();
            _sceneOrigin = sceneOrigin;
            _rotationStep = playerConfig.RotationStep;
        }

        private void Awake()
        {
            _signalBus.Subscribe<OnConstructStartSignal>(ConstructItem);
            _signalBus.Subscribe<OnConstructTryEndSignal>(CheckConstructPlace);
            _signalBus.Subscribe<OnConstructDeclineSignal>(DeclineConstruct);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnConstructStartSignal>(ConstructItem);
            _signalBus.Unsubscribe<OnConstructTryEndSignal>(CheckConstructPlace);
            _signalBus.Unsubscribe<OnConstructDeclineSignal>(DeclineConstruct);
        }

        private void ConstructItem(OnConstructStartSignal contructItemSignal)
        {
            _contructableItem = contructItemSignal.ContructableItem;
            _contructableItem.StartConstruct();

            var constructableItemData = _contructableItem.GetConstructData();
            SpawnFantom(constructableItemData.ItemFantom);

            _constructControlStream = Observable.EveryUpdate()
                .Subscribe(_ => TransformFantom(constructableItemData.LayerMask));
            _isHitDetectedPropertyStream = _isHitDetected
                .DistinctUntilChanged()
                .Subscribe(hit => _itemFantom.SetAvaliableState(hit));
        }

        private void TransformFantom(LayerMask layerMask)
        {
            MoveFantom(layerMask);
            RotateFantom();
        }

        private void MoveFantom(LayerMask layerMask)
        {
            Ray ray;
            var raycastHit = _cameraController.Raycast(out ray, layerMask);
            _isHitDetected.Value = raycastHit.collider != null;

            if (_isHitDetected.Value)
            {
                var targetPosition = raycastHit.point;

                var heightOffset = _itemFantom.GetCollider().bounds.extents.y;
                targetPosition += raycastHit.normal * (heightOffset + 0.01f);

                _itemFantom.transform.position = targetPosition;

                var forward = Vector3.ProjectOnPlane(_itemFantom.transform.forward, raycastHit.normal).normalized;
                if (forward == Vector3.zero) forward = Vector3.forward;
                _itemFantom.transform.rotation = Quaternion.LookRotation(forward, raycastHit.normal);
            }
            else
            {
                var endPoint = ray.origin + ray.direction * _cameraController.GetRaycastLen();
                _itemFantom.transform.position = endPoint;
            }
        }

        private void RotateFantom()
        {
            var scroll = Input.mouseScrollDelta.y;

            if (Mathf.Abs(scroll) > 0.01f)
            {
                var rotationAmount = _rotationStep * Mathf.Sign(scroll);
                _itemFantom.transform.Rotate(Vector3.up, rotationAmount, Space.World);
            }
        }

        private void SpawnFantom(ItemFantom itemFantom)
        {
            _itemFantom = Instantiate(itemFantom, _sceneOrigin);
            var itemTransform = _contructableItem.GetTransform();
            _itemFantom.transform.position = itemTransform.position;
            _itemFantom.transform.rotation = itemTransform.rotation;
        }

        private void CheckConstructPlace()
        {
            if (_itemFantom.GetState()) FinishConstruct();
        }

        private void FinishConstruct()
        {
            _signalBus.Fire(new OnConstructEndSignal());

            _constructControlStream.Dispose();
            _isHitDetectedPropertyStream.Dispose();
            var constructableObjTransform = _contructableItem.GetTransform();
            constructableObjTransform.position = _itemFantom.transform.position;
            constructableObjTransform.rotation = _itemFantom.transform.rotation;

            Destroy(_itemFantom.gameObject);
            _contructableItem.EndConstruct(_sceneOrigin);
            _contructableItem = null;
        }

        private void DeclineConstruct()
        {
            _constructControlStream.Dispose();
            _isHitDetectedPropertyStream.Dispose();

            Destroy(_itemFantom.gameObject);
            _contructableItem.DeclineConstruct();
            _contructableItem = null;
        }
    }
}
