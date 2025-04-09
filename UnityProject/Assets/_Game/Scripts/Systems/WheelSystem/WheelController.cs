using UnityEngine;

namespace _Game.Systems.WheelSystem
{
    public class WheelController : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [SerializeField] private float _maxSpinSpeed = 720f; // Degrees/sec
        [SerializeField] private float _decelerationRate = 90f;
        [SerializeField] private AnimationCurve _speedCurve;

        private float _currentSpeed;
        private bool _isSpinning;

        void Update()
        {
            if (!_isSpinning) return;
        
            _currentSpeed = Mathf.MoveTowards(
                _currentSpeed, 
                0f, 
                _decelerationRate * Time.deltaTime
            );
        
            float easedSpeed = _currentSpeed * _speedCurve.Evaluate(_currentSpeed / _maxSpinSpeed);
            transform.Rotate(Vector3.up, easedSpeed * Time.deltaTime);
        }

        public void StartSpin(float initialSpeed)
        {
            _currentSpeed = initialSpeed;
            _isSpinning = true;
        }
    }
}