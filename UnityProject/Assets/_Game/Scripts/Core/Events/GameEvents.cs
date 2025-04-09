using System.Collections.Generic;
using _Game.Interfaces;
using _Game.Enums;
using _Game.Systems.BetSystem;
using _Game.Systems.ChipSystem;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Screens;
using UnityEngine;

namespace _Game.Core.Events
{
    public struct GameInitializedEvent : IGameEvent
    {
        public string Message { get; }
        public GameInitializedEvent(string message) => this.Message = message;
    }
    
    public struct GameStartedEvent : IGameEvent
    {
        public string Message { get; }
        public GameStartedEvent(string message) => Message = message;
    }
    
    public struct GameOverEvent : IGameEvent
    {
        public string Message { get; }
        public GameOverEvent(string message) => Message = message;
    }
    
    public struct GameEndedEvent : IGameEvent
    {
        public string Message { get; }
        public int FinalBalance;
        public bool IsWin => FinalBalance > 0;
        public GameEndedEvent(string message, int result) => (Message, FinalBalance) = (message, result);
    }
    
    public struct ToTableEvent : IGameEvent
    {
        public string Message { get; }
        public ToTableEvent(string message) => Message = message;
    }

    public struct ToBettingEvent : IGameEvent
    {
        public string Message { get; }
        public ToBettingEvent(string message) => Message = message;
    }
    
    public struct ToRouletteEvent : IGameEvent
    {
        public int TotalBet;
        public ToRouletteEvent(int totalBet) => TotalBet = totalBet;
    }

    public struct FinanceSystemInitialized : IGameEvent
    {
        public IFinancialService FinancialService;
        public FinanceSystemInitialized(IFinancialService financialService) => FinancialService = financialService;
    }

    public struct ChipSystemInitialized : IGameEvent
    {
        public ChipManager ChipManager;
        
        public ChipSystemInitialized(ChipManager chipManager) => ChipManager = chipManager;
    }
    
    public struct WinEvent : IGameEvent {}
    public struct LoseEvent : IGameEvent {}

}