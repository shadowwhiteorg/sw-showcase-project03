using _Game.Core.Events;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.PayoutSystem
{
    /// <summary>
    /// PayoutIntegrator listens for the PayoutCalculatedEvent.
    /// It then credits the total winning payout to the user’s balance using the IFinancialService.
    /// </summary>
    public class PayoutIntegrator : MonoBehaviour
    {
        private IEventBus _eventBus;
        private IFinancialService _financialService;

        /// <summary>
        /// Initialize the integrator with the event bus and finance service.
        /// </summary>
        /// <param name="eventBus">The central event bus.</param>
        /// <param name="financialService">The financial service that manages the player's balance.</param>
        public void Construct(IEventBus eventBus, IFinancialService financialService)
        {
            _eventBus = eventBus;
            _financialService = financialService;
            _eventBus.Subscribe<PayoutCalculatedEvent>(OnPayoutCalculated);
        }

        /// <summary>
        /// Handles the PayoutCalculatedEvent by crediting the winnings.
        /// </summary>
        /// <param name="evt">The event containing the winning bets and total payout.</param>
        private void OnPayoutCalculated(PayoutCalculatedEvent evt)
        {
            // Convert the total payout to an integer if needed.
            int payoutToCredit = Mathf.RoundToInt(evt.TotalPayout);
            _financialService.CreditFunds(payoutToCredit);

            Debug.Log($"PayoutIntegrator: Credited {payoutToCredit} funds for winning number {evt.WinningNumber}.");
        }
    }
}