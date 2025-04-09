using System.Collections.Generic;
using _Game.Interfaces;
using _Game.Systems.BetSystem;

namespace _Game.Core.Events
{
    public struct TransactionFailedEvent : IGameEvent
    {
        public string Reason;
        public TransactionFailedEvent(string reason) => Reason = reason;
    }
    
    public struct BalanceResetEvent : IGameEvent
    {
        public float NewBalance;
        public BalanceResetEvent(float newBalance) => NewBalance = newBalance;
    }

    public struct PayoutCalculatedEvent : IGameEvent
    {
        public List<Bet> WinningBets;
        public int TotalPayout;
        public int WinningNumber;
    
        public PayoutCalculatedEvent(List<Bet> winners, int payout, int winningNumber)
        {
            WinningBets = winners;
            TotalPayout = payout;
            WinningNumber = winningNumber;
        }
    }

    public struct SinglePayoutEvent : IGameEvent
    {
        public Bet WinningBet;
        public float Amount;
    
        public SinglePayoutEvent(Bet bet, float amount)
        {
            WinningBet = bet;
            Amount = amount;
        }
    }
    
    public struct BalanceChangedEvent : IGameEvent
    {
        public float NewBalance;
        public BalanceChangedEvent(float newBalance) => NewBalance = newBalance;
    }
}

