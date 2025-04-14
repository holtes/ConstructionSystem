using UnityEngine;
using System;
using UniRx;
using Zenject;
using _Project.Develop.Runtime.Signals;


namespace _Project.Develop.Runtime.Controllers.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform _pickPoint;
        [SerializeField] private PlayerCameraController _cameraController;

        [Inject] private SignalBus _signalBus;

        private IDisposable _controllStreamRC;
        private IDisposable _controllStreamLC;

        private void Awake()
        {
            _signalBus.Subscribe<OnPickUpSignal>(SecondIteractControlls);
            _signalBus.Subscribe<OnConstructStartSignal>(SecondIteractControlls);
            _signalBus.Subscribe<OnConstructEndSignal>(FirstInteractControlls);
            FirstInteractControlls();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnPickUpSignal>(SecondIteractControlls);
            _signalBus.Unsubscribe<OnConstructStartSignal>(SecondIteractControlls);
            _signalBus.Unsubscribe<OnConstructEndSignal>(FirstInteractControlls);
        }

        private void FirstInteractControlls()
        {
            _controllStreamRC?.Dispose();
            _controllStreamLC?.Dispose();
            _controllStreamRC = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => _cameraController.CheckIteractableItem());
        }

        private void SecondIteractControlls()
        {
            _controllStreamRC?.Dispose();
            _controllStreamLC?.Dispose();
            _controllStreamRC = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ => FinishItemInteract());
            _controllStreamLC = Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButtonDown(1))
                .Subscribe(_ => DeclineItemInteract());

        }

        private void FinishItemInteract() => _signalBus.Fire(new OnConstructTryEndSignal());

        private void DeclineItemInteract()
        {
            _signalBus.Fire(new OnConstructDeclineSignal());
            _signalBus.Fire(new OnDropSignal());
            FirstInteractControlls();
        }

        public Transform GetPickPoint() => _pickPoint;

        public PlayerCameraController GetCameraController() => _cameraController;
    }
}

