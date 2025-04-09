using _Game.Enums;
using _Game.Interfaces;
using _Game.Systems.BetSystem;
using UnityEngine;

namespace _Game.Core.Events
{
    
    public struct BetAmountChangedEvent : IGameEvent 
    {
        public int CurrentBet;
        public int Multiplier;
        public int TotalBet;
    }
    
    public struct MultiplierSetEvent : IGameEvent
    {
        public int Multiplier;
        public MultiplierSetEvent(int multiplier) => Multiplier = multiplier;
    }
    
    public struct IncreaseBetEvent : IGameEvent
    {
        public int Amount;
        public IncreaseBetEvent(int amount) => Amount = amount;
    }
    
    public struct DecreaseBetEvent : IGameEvent
    {
        public int Amount;
        public DecreaseBetEvent(int amount) => Amount = amount;
    }
    
    public struct IncreaseMultiplierEvent : IGameEvent
    {
        public int Amount;
        public IncreaseMultiplierEvent(int amount) => Amount = amount;
    }
    
    public struct DecreaseMultiplierEvent : IGameEvent
    {
        public int Amount;
        public DecreaseMultiplierEvent(int amount) => Amount = amount;
    }
    public struct BetPlacedEvent : IGameEvent
    {
        public Bet Bet;
        public Vector3 WorldPosition; // ChipVisual drop location
        public GameObject ChipVisual;

        public BetPlacedEvent(Bet bet, Vector3 worldPos, GameObject chipVisual = null)
        {
            Bet = bet;
            WorldPosition = worldPos;
            ChipVisual = chipVisual;
        }
    }

    public struct TotalBetsCalculatedEvent : IGameEvent
    {
        public int TotalBet;
        public TotalBetsCalculatedEvent(int totalBet) => TotalBet = totalBet;
    }

    public struct BetFailedEvent : IGameEvent
    {
        public string Reason;
        public BetType AttemptedType;
    
        public BetFailedEvent(string reason, BetType type)
        {
            Reason = reason;
            AttemptedType = type;
        }
    }
    public struct BetsClearedEvent : IGameEvent 
    {
        public int ClearedBetCount;
    
        public BetsClearedEvent(int count) => ClearedBetCount = count;
    }

    public struct BetRemovedEvent : IGameEvent
    {
        public Bet Bet;
        public float RefundAmount;
    
        public BetRemovedEvent(Bet bet, float refund)
        {
            Bet = bet;
            RefundAmount = refund;
        }
    }
    
    public struct AmbiguousBetEvent : IGameEvent
    {
        public Vector3 WorldPosition;
        public BetType[] PossibleTypes;
    
        public AmbiguousBetEvent(Vector3 pos, BetType[] types)
        {
            WorldPosition = pos;
            PossibleTypes = types;
        }
    }

    public struct MaxBetExceededEvent : IGameEvent
    {
        public float AttemptedAmount;
        public float CurrentTotal;
    
        public MaxBetExceededEvent(float attempted, float current)
        {
            AttemptedAmount = attempted;
            CurrentTotal = current;
        }
    }
    
    public struct BetAreaHoverEvent : IGameEvent
    {
        public BetArea Area;
        public Vector3 HoverPosition;
        
        public BetAreaHoverEvent(BetArea area, Vector3 hoverPos)
        {
            Area = area;
            HoverPosition = hoverPos;
        }
    }
    
    public struct ChipCreatedEvent : IGameEvent{}
}