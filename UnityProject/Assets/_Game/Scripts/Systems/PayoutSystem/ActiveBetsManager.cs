using System.Collections.Generic;
using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.BetSystem;
using UnityEngine;

namespace _Game.Systems.PayoutSystem
{
    public class ActiveBetsManager : MonoBehaviour
    {
        // Holds the active bets for the current round.
        private readonly List<Bet> _activeBets = new List<Bet>();
        public IReadOnlyList<Bet> ActiveBets => _activeBets.AsReadOnly();

        private IEventBus _eventBus;


        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<BetPlacedEvent>(OnBetPlaced);
            _eventBus.Subscribe<BetsClearedEvent>(OnBetsCleared);
        }


        private void OnBetPlaced(BetPlacedEvent evt)
        {
            if (evt.Bet != null)
            {
                _activeBets.Add(evt.Bet);
                var totalBets = 0;
                foreach (var bet in _activeBets)
                {
                    totalBets += bet.Amount;
                }
                _eventBus.Fire<TotalBetsCalculatedEvent>(new TotalBetsCalculatedEvent(totalBets));
            }
        }


        private void OnBetsCleared(BetsClearedEvent evt)
        {
            int count = _activeBets.Count;
            _activeBets.Clear();
            Debug.Log($"ActiveBetsManager: Cleared {count} bets.");
        }

        public List<Bet> GetActiveBets()
        {
            return new List<Bet>(_activeBets);
        }
    }
}