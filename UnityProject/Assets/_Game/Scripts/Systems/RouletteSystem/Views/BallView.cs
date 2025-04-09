using System.Collections;
using _Game.Core.Events;
using _Game.Enums;
using _Game.Interfaces;
using _Game.Systems.SimulationSystem;
using UnityEditor.Rendering;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallView : MonoBehaviour
    {
        private Rigidbody _rb;
        private Transform _wheelCenter;
        private bool _isFollowing = false;
        private float _orbitRadius;
        private float _orbitAngle;
        private Vector3 _initialPosition;
        private Collider _collider;
        private IEventBus _eventBus;
        private WheelView _wheel;
        private int _targetNumber;
        
        public BallMode mode = BallMode.PhysicsRecord;
        public BallPolarRecorder recorder;
        public BallPolarPlayback playback;
        public WheelView Wheel => _wheel;

        public void Construct(IEventBus eventBus, WheelView wheel)
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _rb.isKinematic = true;
            _initialPosition = transform.position;
            _eventBus = eventBus;
            _wheel = wheel;
            
            _eventBus.Subscribe<SpinWheelEvent>(e=>OnSpinWheel());
            _eventBus.Subscribe<SpinCompletedEvent>(e=>OnSpinCompleted(e.WinningNumber));
            _eventBus.Subscribe<SpinWheelEvent>(e=>OnSpinResultReady());
            _eventBus.Subscribe<TargetNumberSetEvent>(e=>OnTargetNumberSet(e.TargetNumber));

            ApplyMode();
        }

        public void SetupOrbit(Transform wheelCenter, float radius)
        {
            _wheelCenter = wheelCenter;
            _orbitRadius = radius;
            _orbitAngle = 0f;
            _isFollowing = true;
        }

        public void OrbitTick(float fixedDeltaTime, float wheelSpeed, Transform ballTarget)
        {
            if (!_isFollowing) return;

            // We move the ball around the wheel center
            float radiansPerSec = -wheelSpeed * Mathf.Deg2Rad;
            _orbitRadius = (ballTarget.position - _wheelCenter.position).magnitude;
            _orbitAngle += radiansPerSec * fixedDeltaTime;

            Vector3 offset = new Vector3(Mathf.Cos(_orbitAngle), 0, Mathf.Sin(_orbitAngle)) * _orbitRadius;
            // kinematic move
            _rb.MovePosition(_wheelCenter.position + offset);
        }

        public void ReleaseBall(float wheelSpeedDegPerSec)
        {
            _isFollowing = false;
            if(mode!= BallMode.PhysicsRecord) return;
            _rb.isKinematic = false;

            // Convert angular speed to radians per second
            float radiansPerSec = -wheelSpeedDegPerSec * Mathf.Deg2Rad;

            // Calculate tangential direction at current orbit angle
            Vector3 tangentDir = new Vector3(-Mathf.Sin(_orbitAngle), 0, Mathf.Cos(_orbitAngle)).normalized;

            // Tangential velocity = angular speed × radius
            Vector3 velocity = tangentDir * (_orbitRadius * radiansPerSec * 1.25f);

            // Optional: add vertical drop component
            velocity += Vector3.down * 1.0f;

            _rb.linearVelocity = velocity;

            Debug.Log($"[BallView] Released ball with velocity: {velocity}");
        }
        
        public float BallSpeed()
        {
            return _rb.linearVelocity.magnitude;
        }
        public float GetAngleRelativeToCenter()
        {
            Vector3 dir = transform.position - _wheelCenter.position;
            return Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        }

        public void ResetBall(Transform newParentPos)
        {
            _rb.isKinematic = true;
            transform.position = _initialPosition;

        }
        
        public void SetMode(BallMode newMode)
        {
            mode = newMode;
            ApplyMode();
        }
        
        private void ApplyMode()
        {
            switch (mode)
            {
                case BallMode.PhysicsRecord:
                    // Enable physics
                    _rb.isKinematic = false;
                    _collider.enabled = true;
                    if (recorder) recorder.enabled = false; // We'll enable on spin
                    if (playback) playback.enabled = false;
                    break;

                case BallMode.PolarPlayback:
                    // Disable physics
                    _rb.isKinematic = true;
                    _collider.enabled = false;
                    if (recorder) recorder.enabled = false;
                    if (playback) playback.enabled = false; // We'll start it on SpinResult
                    break;
            }
        }
        
        
        private void OnSpinWheel()
        {
            if (mode != BallMode.PhysicsRecord) return;

            // Start recording now
            if (recorder)
            {
                recorder.enabled = true;
                recorder.StartRecording();
            }
        }

        private void OnSpinCompleted(int winningNumber)
        {
            if (mode != BallMode.PhysicsRecord) return;

            // Stop recording and save, using e.WinningNumber
            if (recorder)
            {
                recorder.enabled = false;
                recorder.StopAndSave(winningNumber);
            }
        }

        private void OnTargetNumberSet(int targetNumber)
        {
            _targetNumber = targetNumber;
        }

        private void OnSpinResultReady()
        {
            if (mode != BallMode.PolarPlayback) return;

            // Start playback with the chosen rigged slot
            if (playback)
            {
                playback.enabled = true;
                playback.StartPlayback(_targetNumber);
            }
        }
    }
}