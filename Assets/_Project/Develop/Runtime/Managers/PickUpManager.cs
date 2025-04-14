using DG.Tweening;
using UnityEngine;
using Zenject;
using _Project.Develop.Runtime.Controllers.Player;
using _Project.Develop.Runtime.Interfaces;
using _Project.Develop.Runtime.Models;
using _Project.Develop.Runtime.Signals;

namespace _Project.Develop.Runtime.Managers
{
    public class PickUpManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private PlayerController _player;
        private float _magnetDuration;

        private IPickable _pickableItem;
        private Tween _pickUpTween;

        [Inject]
        private void Construct(SignalBus signalBus, PlayerController player, PlayerConfig playerConfig)
        {
            _signalBus = signalBus;
            _player = player;
            _magnetDuration = playerConfig.MagnetDuration;
        }

        private void Awake()
        {
            _signalBus.Subscribe<OnPickUpSignal>(PickUpItem);
            _signalBus.Subscribe<OnDropSignal>(DropItem);
            _signalBus.Subscribe<OnConstructEndSignal>(KillPickUpMovement);
        }
        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPickUpSignal>(PickUpItem);
            _signalBus.Unsubscribe<OnDropSignal>(DropItem);
            _signalBus.Unsubscribe<OnConstructEndSignal>(KillPickUpMovement);
        }

        private void PickUpItem(OnPickUpSignal pickItemSignal)
        {
            _pickableItem = pickItemSignal.PickableItem;
            _pickableItem.PickUp(_player.transform);
            MoveItemToTarget();
        }

        private void MoveItemToTarget()
        {
            var itemTransform = _pickableItem.GetTransform();
            var target = _player.GetPickPoint();

            var startPos = itemTransform.position;
            var startRot = itemTransform.rotation;

            _pickUpTween = DOVirtual.Float(0, 1, _magnetDuration, t =>
            {
                itemTransform.position = Vector3.Lerp(startPos, target.position, t);
                itemTransform.rotation = Quaternion.Slerp(startRot, target.rotation, t);
            });
        }

        private void KillPickUpMovement() => _pickUpTween?.Kill();

        private void DropItem() => _pickableItem.Drop(_player.transform.parent);
    }
}
