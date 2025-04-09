using System;
using UnityEngine;

namespace _Game.Systems.SimulationSystem
{
    [Serializable]
    public class TrajectorySample
    {
        public float time;             // Time (in seconds) since simulation start
        public Vector3 position;       // Ball position at that time
        public Quaternion rotation;    // Ball rotation at that time

        public TrajectorySample(float t, Vector3 pos, Quaternion rot)
        {
            time = t;
            position = pos;
            rotation = rot;
        }
    }
}