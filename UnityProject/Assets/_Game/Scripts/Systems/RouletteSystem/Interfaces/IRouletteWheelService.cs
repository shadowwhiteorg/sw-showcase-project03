namespace _Game.Systems.RouletteSystem
{
    public interface IRouletteWheelService
    {
        void StartSpin();
        void FixedTick(float fixedDeltaTime);
        void ApplyDeceleration();
        bool WheelStopped();
        float GetCurrentAngle();
        void Reset();
        public bool IsBelowThreshold();
    }
}