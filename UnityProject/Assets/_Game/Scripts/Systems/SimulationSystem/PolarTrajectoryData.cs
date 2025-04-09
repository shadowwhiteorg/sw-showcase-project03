using System;
using System.Collections.Generic;

namespace _Game.Systems.SimulationSystem
{
    [Serializable]
    public class PolarTrajectoryData
    {
        public int recordedSlot = -1;
        public List<PolarFrame> frames = new();
    }
}