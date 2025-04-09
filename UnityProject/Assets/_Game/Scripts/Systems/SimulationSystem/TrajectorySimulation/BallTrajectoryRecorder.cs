using UnityEngine;

namespace _Game.Systems.SimulationSystem
{
    public class BallTrajectoryRecorder : MonoBehaviour
    {
        public float recordInterval = 0.02f; // (Optional) Use a fixed interval (assumed same as fixedDeltaTime)
        public TrajectoryData trajectoryData = new TrajectoryData();
        private float elapsedTime = 0f;

        void FixedUpdate()
        {
            elapsedTime += Time.fixedDeltaTime;
            trajectoryData.samples.Add(new TrajectorySample(
                elapsedTime,
                transform.position,
                transform.rotation
            ));
        }

        public TrajectoryData GetTrajectoryData()
        {
            return trajectoryData;
        }
    }
}