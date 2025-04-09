using _Game.Core.ScriptableObjects;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteContext
    {
        public WheelView WheelView { get; }
        public BallView BallView { get; }
        public RouletteConfigSO Config { get; }

        public RouletteContext(WheelView wheelView, BallView ballView, RouletteConfigSO config)
        {
            WheelView = wheelView;
            BallView = ballView;
            Config = config;
        }

        public Transform WheelCenter => WheelView.WheelCenter;
        public float DropThreshold => Config.dropThresholdSpeed;
        public int SlotCount => Config.slotCount;
        public float OrbitRadius => Config.orbitRadius;
    }
}

