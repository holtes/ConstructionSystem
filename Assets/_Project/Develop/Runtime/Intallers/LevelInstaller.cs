using UnityEngine;
using Zenject;
using _Project.Develop.Runtime.Controllers.Player;
using _Project.Develop.Runtime.Models;
using _Project.Develop.Runtime.Signals;

namespace _Project.Develop.Runtime.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private PlayerController _player;
        [SerializeField] private Transform _sceneOrigin;
        public override void InstallBindings()
        {
            BindSignalBus();
            BindSignals();
            BindPlayer();
            BindPlayerConfig();
            BindSceneOrigin();
        }

        private void BindSignalBus() => SignalBusInstaller.Install(Container);

        private void BindSignals()
        {
            Container.DeclareSignal<OnPickUpSignal>();
            Container.DeclareSignal<OnDropSignal>();

            Container.DeclareSignal<OnConstructStartSignal>();
            Container.DeclareSignal<OnConstructTryEndSignal>();
            Container.DeclareSignal<OnConstructEndSignal>();
            Container.DeclareSignal<OnConstructDeclineSignal>();
        }

        private void BindPlayer() => Container.Bind<PlayerController>().FromInstance(_player);

        private void BindPlayerConfig() => Container.Bind<PlayerConfig>().FromInstance(_playerConfig);

        private void BindSceneOrigin() => Container.Bind<Transform>().FromInstance(_sceneOrigin);
    }
}