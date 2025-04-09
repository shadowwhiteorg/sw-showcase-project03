using System.Collections.Generic;
using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.BetSystem;
using UnityEngine;

namespace _Game.Systems.PayoutSystem
{
    /// <summary>
    /// BetPayoutCalculator listens for the SpinCompletedEvent, retrieves all active bets from ActiveBetsManager,
    /// calculates the total payout for winning bets, and fires a PayoutCalculatedEvent.
    /// This system re-uses your existing Bet type.
    /// </summary>
    public class BetPayoutCalculator : MonoBehaviour
    {
        private IEventBus _eventBus;
        private ActiveBetsManager _activeBetsManager;

       
        public void Construct(IEventBus eventBus, ActiveBetsManager activeBetsManager)
        {
            _eventBus = eventBus;
            _activeBetsManager = activeBetsManager;
            _eventBus.Subscribe<SpinCompletedEvent>(OnSpinCompleted);
        }

        /// <summary>
        /// Handles SpinCompletedEvent: calculates which bets win and aggregates the total payout.
        /// </summary>
        /// <param name="evt">SpinCompletedEvent containing the winning number.</param>
        private void OnSpinCompleted(SpinCompletedEvent evt)
        {
            int winningNumber = evt.WinningNumber;
            List<Bet> activeBets = _activeBetsManager.GetActiveBets();
            List<Bet> winningBets = new List<Bet>();
            int totalPayout = 0;

            foreach (Bet bet in activeBets)
            {
                // Determine if this bet wins using your existing logic.
                if (bet.IsWinningBet(winningNumber))
                {
                    winningBets.Add(bet);
                    totalPayout += bet.CalculatePayout();
                }
            }
            if(totalPayout >0) _eventBus.Fire(new WinEvent());
            else _eventBus.Fire(new LoseEvent());
            PayoutCalculatedEvent payoutEvent = new PayoutCalculatedEvent(winningBets, totalPayout, winningNumber);
            _eventBus.Fire(payoutEvent);

        }
    }
}