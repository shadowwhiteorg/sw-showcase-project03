using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteOutcomeService : IRouletteOutcomeService
    {
        private int _targetNumber = -1;

        public void SetTargetNumber(int number)
        {
            _targetNumber = number;
            Debug.Log("Target number set to: " + _targetNumber);
        }

        public int GetTargetNumber()
        {
            return _targetNumber;
        }

        public int PredictSlot(float wheelRotation)
        {
            // Placeholder prediction logic
            Debug.Log("Predicting slot based on wheel rotation.");
            return -1;
        }
    }
}