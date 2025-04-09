using System;

namespace _Game.Systems.SimulationSystem
{
    [Serializable]
    public class PolarFrame
    {
        public float time;
        public float angleDeg;     // θ in degrees
        public float radius;       // r
        public float height;       // y

        public PolarFrame(float time, float angleDeg, float radius, float height)
        {
            this.time = time;
            this.angleDeg = angleDeg;
            this.radius = radius;
            this.height = height;
        }
    }
}