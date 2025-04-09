using System.Collections;
using UnityEngine;

namespace _Game.Systems.SimulationSystem
{
    public class BallTrajectoryPlayback : MonoBehaviour
    {
        private Rigidbody rb;
        private TrajectoryData trajectoryData;
        private float playbackStartTime;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Call this method to begin playback.
        public void StartPlayback(TrajectoryData data)
        {
            trajectoryData = data;
            playbackStartTime = Time.time;
            rb.isKinematic = true; // Prevent physics from interfering
            StartCoroutine(PlaybackRoutine());
        }

        IEnumerator PlaybackRoutine()
        {
            int sampleIndex = 0;
            int totalSamples = trajectoryData.samples.Count;
            if (totalSamples < 2)
                yield break;

            while (sampleIndex < totalSamples - 1)
            {
                float playbackTime = Time.time - playbackStartTime;
                // Clamp playbackTime to simulation end.
                if (playbackTime > trajectoryData.samples[totalSamples - 1].time)
                {
                    transform.position = trajectoryData.samples[totalSamples - 1].position;
                    transform.rotation = trajectoryData.samples[totalSamples - 1].rotation;
                    break;
                }

                // Find the two samples we are between.
                while (sampleIndex < totalSamples - 1 && trajectoryData.samples[sampleIndex + 1].time < playbackTime)
                {
                    sampleIndex++;
                }
                TrajectorySample current = trajectoryData.samples[sampleIndex];
                TrajectorySample next = trajectoryData.samples[sampleIndex + 1];
                float t = Mathf.InverseLerp(current.time, next.time, playbackTime);

                // Interpolate position and rotation.
                transform.position = Vector3.Lerp(current.position, next.position, t);
                transform.rotation = Quaternion.Slerp(current.rotation, next.rotation, t);

                yield return null;
            }
        }
    }
}