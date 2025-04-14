using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace _Project.Develop.Runtime.Controllers.Items
{
    [RequireComponent(typeof(MeshRenderer), typeof(Collider))]
    public class ItemFantom : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Color _colideColor = Color.white;
        [SerializeField] private Color _correctColor = Color.white;

        private Material _mat;
        private bool _isOverlaped;
        private bool _isAvailable;

        private void Awake() => _mat = GetComponent<MeshRenderer>().material;

        private void Start()
        {
            this.OnTriggerEnterAsObservable()
                .Subscribe(other => SetOverlapState(true))
                .AddTo(this);

            this.OnTriggerExitAsObservable()
                .Subscribe(other => SetOverlapState(false))
                .AddTo(this);
        }

        private void SetOverlapState(bool state)
        {
            _isOverlaped = state;
            ChangeFantomColor();
        }
        public void SetAvaliableState(bool state)
        {
            _isAvailable = state;
            ChangeFantomColor();
        }

        public bool GetState() => !_isOverlaped && _isAvailable;

        public void ChangeFantomColor() => _mat.color = !_isOverlaped && _isAvailable ? _correctColor : _colideColor;

        public Collider GetCollider() => _collider;
    }
}
