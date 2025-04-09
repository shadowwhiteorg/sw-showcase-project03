using System;
using System.Collections.Generic;
using System.IO;
using _Game.Systems.RouletteSystem;
using UnityEngine;

namespace _Game.Systems.SimulationSystem
{
    public class BallPolarRecorder : MonoBehaviour
    {
        [Header("References")]
        private Transform _wheelCenter;
        [Header("Recording Settings")]
        public string saveFilename = "defaultTrajectory.json";
        public float recordInterval = 0.02f;

        private bool _isRecording;
        private float _elapsed;
        private float _nextRecordTime;
        private readonly List<PolarFrame> frames = new();
        
        void FixedUpdate()
        {
            if (!_isRecording) return;

            _elapsed += Time.fixedDeltaTime;

            if (_elapsed >= _nextRecordTime)
            {
                _nextRecordTime += recordInterval;

                Vector3 pos = transform.position;
                Vector3 flatOffset = new(pos.x - _wheelCenter.position.x, 0, pos.z - _wheelCenter.position.z);
                float angle = Mathf.Atan2(flatOffset.z, flatOffset.x) * Mathf.Rad2Deg;
                angle = (angle + 360f) % 360f;

                float radius = flatOffset.magnitude;
                float height = pos.y;

                frames.Add(new PolarFrame(_elapsed, angle, radius, height));
            }
        }

        public void StartRecording()
        {
            frames.Clear();
            _elapsed = 0f;
            _nextRecordTime = 0f;
            _isRecording = true;
            Debug.Log("[BallPolarRecorder] Started recording polar frames...");
            _wheelCenter = GetComponent<BallView>().Wheel.WheelCenter;
        }

        public void StopAndSave(int finalSlotNumber)
        {
            _isRecording = false;

            var data = new PolarTrajectoryData
            {
                recordedSlot = finalSlotNumber,
                frames = new List<PolarFrame>(frames)
            };

            TrajectoryFileManager.Save(saveFilename, data);
            Debug.Log($"[BallPolarRecorder] Stopped. Saved with finalSlot={finalSlotNumber} to {saveFilename}");
        }
    }
}