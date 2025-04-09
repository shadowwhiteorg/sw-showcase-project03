using System;
using System.Collections;
using _Game.Core.ScriptableObjects;
using _Game.Systems.RouletteSystem;
using UnityEngine;

namespace _Game.Systems.SimulationSystem
{
    using UnityEngine;
    using System.Collections;

    public class BallPolarPlayback : MonoBehaviour
    {

        private PolarTrajectoryData _trajectory;
        private float _playbackStartTime;
        private float _angleOffset;
        private Transform _wheelCenter;
        private WheelView _wheelView; // so we can get NumberSlotData references
        
        public string loadFilename = "defaultTrajectory.json";

        public void StartPlayback(int targetSlotNumber)
        {
            _trajectory = TrajectoryFileManager.Load(loadFilename);
            if (_trajectory == null || _trajectory.frames.Count == 0)
            {
                Debug.LogError("[BallPolarPlayback] No trajectory loaded or data is empty!");
                return;
            }

            _wheelView = GetComponent<BallView>().Wheel;
            _wheelCenter = _wheelView.WheelCenter;
            // 1) The final slot from the recorded run
            int recordedFinalSlot = _trajectory.recordedSlot;
            var finalSlotData = _wheelView.GetSlotData(recordedFinalSlot);
            var targetSlotData = _wheelView.GetSlotData(targetSlotNumber);
            Debug.Log("target " + targetSlotData.Number);
            Debug.Log("final " + finalSlotData.Number);

            if (!finalSlotData || !targetSlotData)
            {
                Debug.LogError(
                    $"[BallPolarPlayback] Could not find slot data for final={recordedFinalSlot} or target={targetSlotNumber}");
                return;
            }

            // 2) Compute angles
            float finalAngle = _wheelView.NumberSlotAngle(finalSlotData);
            float desiredAngle = _wheelView.NumberSlotAngle(targetSlotData);

            // 3) offset so finalAngle maps to desiredAngle
            _angleOffset = -Mathf.DeltaAngle(finalAngle, desiredAngle);
            // _angleOffset = Mathf.DeltaAngle(finalAngle, desiredAngle);
            Debug.Log(
                $"[BallPolarPlayback] StartPlayback: finalAngle={finalAngle:F1}, desiredAngle={desiredAngle:F1}, offset={_angleOffset:F1}");
           
            _playbackStartTime = Time.time;
            Debug.Log("angle offset "+_angleOffset);
            StopAllCoroutines();
            StartCoroutine(PlaybackRoutine());
        }

        private IEnumerator PlaybackRoutine()
        {
            int index = 0;

            while (index < _trajectory.frames.Count - 1)
            {
                float currentTime = Time.time - _playbackStartTime;

                while (index < _trajectory.frames.Count - 1 &&
                       _trajectory.frames[index + 1].time < currentTime)
                {
                    index++;
                }

                PolarFrame f0 = _trajectory.frames[index];
                PolarFrame f1 = _trajectory.frames[Mathf.Min(index + 1, _trajectory.frames.Count - 1)];

                float t = Mathf.InverseLerp(f0.time, f1.time, currentTime);

                float angle = Mathf.LerpAngle(f0.angleDeg, f1.angleDeg, t) + _angleOffset;
                float radius = Mathf.Lerp(f0.radius, f1.radius, t);
                float height = Mathf.Lerp(f0.height, f1.height, t);

                float rad = angle * Mathf.Deg2Rad;
                Vector3 pos = new(
                    Mathf.Cos(rad) * radius + _wheelCenter.position.x,
                    height,
                    Mathf.Sin(rad) * radius + _wheelCenter.position.z
                );
                transform.position = pos;

                yield return null;
            }

            Debug.Log("[BallPolarPlayback] Playback complete.");
        }
    }

}