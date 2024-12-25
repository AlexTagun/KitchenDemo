using System;
using Quantum;
using UnityEngine;

namespace KitchenDemo.Gameplay
{
    public class CameraView : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _anchor;

        private Transform _transform;

        public Camera Camera => _camera;

        public Vector3 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }
        
        public Vector3 AnchorPosition
        {
            get => _anchor.position;
            set => _anchor.position = value;
        }

        private void Awake()
        {
            _transform = transform;
        }
    }
}