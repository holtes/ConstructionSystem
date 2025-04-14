using UnityEngine;
using UniRx;
using Zenject;
using _Project.Develop.Runtime.Models;

namespace _Project.Develop.Runtime.Controllers.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        private float _moveSpeed;

        [Inject]
        private void Construct(PlayerConfig playerConfig)
        {
            _moveSpeed = playerConfig.PlayerSpeed;
        }

        private void Start()
        {
            Observable.EveryFixedUpdate()
                .Subscribe(_ => Move())
                .AddTo(this);
        }

        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            var move = new Vector3(horizontal, 0f, vertical);

            if (move.magnitude > 1f)
                move = move.normalized;

            move = transform.TransformDirection(move);

            _characterController.Move(move * _moveSpeed * Time.deltaTime);
        }
    }
}

