namespace _Game.Systems.RouletteSystem
{
    public interface IRouletteOutcomeService
    {
        void SetTargetNumber(int number);    // Optional rigging
        int GetTargetNumber();
        int PredictSlot(float wheelRotation); // Optional pre-simulation for deterministic mode
    }
}