namespace _Game.Systems.RouletteSystem
{
    public interface IRouletteBallService
    {
        void AttachToWheel();     // Ball follows wheel (kinematic)
        void ReleaseToPhysics();  // Ball is released into physics
        int DetectSlot();         // Returns slot ID, -1 if still rolling
        void Reset();             // Resets to initial state
        void FixedTick(float deltaTime); // Handles orbit movement while kinematic
        public bool IsBallStopped(); // Returns true if ball is stopped
    }
}