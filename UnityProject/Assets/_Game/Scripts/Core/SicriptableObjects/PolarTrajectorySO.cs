using _Game.Systems.SimulationSystem;
using UnityEngine;

namespace _Game.Core.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Roulette/Polar Trajectory")]
    public class PolarTrajectorySO : ScriptableObject
    {
        public TextAsset jsonFile;

        public PolarTrajectoryData Load()
        {
            return JsonUtility.FromJson<PolarTrajectoryData>(jsonFile.text);
        }
    }
}