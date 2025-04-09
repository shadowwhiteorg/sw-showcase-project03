using System;
using System.Collections.Generic;

namespace _Game.Systems.SimulationSystem
{
    [Serializable]
    public class TrajectoryData
    {
        public List<TrajectorySample> samples = new List<TrajectorySample>();
        public int winningNumber = -1;
    }
}