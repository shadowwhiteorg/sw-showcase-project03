using _Game.Enums;
using UnityEngine;

namespace _Game.Interfaces
{
    public interface IBettingService
    {
        void PlaceBet(BetType type, float amount, Vector3 worldPosition);
        void ClearBets();
        void CalculatePayouts(int winningNumber);
    }
}