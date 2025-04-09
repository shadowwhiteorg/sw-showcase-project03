using UnityEngine;

namespace _Game.Core.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Roulette/New Roulette Config")]
    public class RouletteConfigSO : ScriptableObject
    {
        [Header("Wheel Motion")]
        public float spinDuration = 5f;               // total time for the wheel curve
        public float maxAngularSpeed = 720f;          // peak speed factor
        public AnimationCurve spinCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        // example: x=0..1, y can go negative->positive->zero for fancy wind-up

        [Header("Ball Logic")]
        public float dropThresholdSpeed = 30f;        // if wheel speed < this, ball is released

        [Header("Slot Setup")]
        public int slotCount = 37;                    // default 0..36
        public float orbitRadius = 1.5f;              // approximate radius
    }

}