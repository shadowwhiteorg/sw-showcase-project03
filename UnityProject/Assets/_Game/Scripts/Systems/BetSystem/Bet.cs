using System;
using _Game.Core.Constants;
using _Game.Enums;
using UnityEngine;

namespace _Game.Systems.BetSystem
{
    [System.Serializable]
    public class Bet
    {
        public BetType Type { get; }
        public int Amount { get; }
        public Vector2Int GridPosition { get; }
        public int[] CoveredNumbers { get; }

        public Bet(BetType type, int amount, Vector2Int gridPos, int[] numbers)
        {
            Type = type;
            Amount = amount;
            GridPosition = gridPos;
            CoveredNumbers = numbers;
        }

        public bool IsWinningBet(int winningNumber)
        {
            return Array.Exists(CoveredNumbers, n => n == winningNumber);
        }

        public int CalculatePayout()
        {
            return Amount * GetPayoutMultiplier();
        }

        private int GetPayoutMultiplier() => Type switch
        {
            BetType.Straight => PayoutConstants.StraightPayout,
            BetType.Split => PayoutConstants.SplitPayout,
            BetType.Street => PayoutConstants.StreetPayout,
            BetType.Dozen or BetType.Column => PayoutConstants.DozenPayout,
            _ => 1 // Red/Black, Even/Odd etc.
        };
    }
}