using _Game.Interfaces;
using _Game.Systems.BetSystem;
using _Game.Systems.ChipSystem;
using UnityEngine;

namespace _Game.Core.Events
{
    public struct ChipDragStartedEvent : IGameEvent
    {
        public ChipStack DraggedStack;
        public ChipDragStartedEvent(ChipStack stack)
        {
            DraggedStack = stack;
        }
    }
    public struct ChipDragEndedEvent : IGameEvent
    {
        public BetArea BetArea;
        public int BetAmount;
        public ChipDragEndedEvent(BetArea betArea, int betAmount)
        {
            BetArea = betArea;
            BetAmount = betAmount;
        }
    }
    
    public struct ChipDragEvent : IGameEvent
    {
        public BetArea ClosestArea;
        public ChipStack DraggedStack;

        public ChipDragEvent(BetArea closestArea, ChipStack stack)
        {
            ClosestArea = closestArea;
            DraggedStack = stack;
        }
    }

    
    public struct ChipsDeductedEvent : IGameEvent
    {
        public int Amount;
        public ChipsDeductedEvent(int amount) => Amount = amount;
    }

    public struct ChipsCreditedEvent : IGameEvent
    {
        public int Amount;
        public ChipsCreditedEvent(int amount) => Amount = amount;
    }
}