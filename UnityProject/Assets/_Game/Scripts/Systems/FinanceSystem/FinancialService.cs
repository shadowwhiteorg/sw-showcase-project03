using _Game.Core.Events;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.FinanceSystem
{

    public class FinancialService : IFinancialService
    {
        private readonly FinancialDataManager _dataManager;
        private readonly IEventBus _eventBus;

        public FinancialService(FinancialDataManager dataManager, IEventBus eventBus)
        {
            _dataManager = dataManager;
            _eventBus = eventBus;
            Debug.Log("[FinancialService] Initialized.");
            SubscribeToEvents();
        }

        public bool DeductFunds(int amount)
        {
            if (_dataManager.Data.Balance < amount)
            {
                _eventBus.Fire(new TransactionFailedEvent("Insufficient funds"));
                return false;
            }

            _dataManager.UpdateBalance(-amount, "Deduction for bet");
            return true;
        }

        public void CreditFunds(int amount)
        {
            _dataManager.UpdateBalance(amount, "Credit from payout");
        }

        public float GetBalance()
        {
            return _dataManager.Data.Balance;
        }

        /// <summary>
        /// Resets the financial balance. This method resets the data manager and then fires a BalanceResetEvent
        /// so that consumers can reload the new, cleared data.
        /// </summary>
        public void ResetBalance(int amount)
        {
            // Perform reset in the data manager (this clears transactions and sets balance to default)
            _dataManager.Reset();

            // Optionally, adjust to a specific reset value if needed.
            if (!Mathf.Approximately(amount, _dataManager.Data.Balance))
            {
                _dataManager.Data.Balance = amount;
                _dataManager.Save();
            }

            // Notify other systems (e.g., UI) so they can refresh their data.
            _eventBus.Fire(new BalanceResetEvent(_dataManager.Data.Balance));
        }

        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<ChipDragStartedEvent>(e =>
            {
                int value = e.DraggedStack.GetTotalValue();
                DeductFunds(value);
            });
            _eventBus.Subscribe<ChipsCreditedEvent>(e =>
            {
                CreditFunds(e.Amount);
            });
            _eventBus.Subscribe<ChipsDeductedEvent>(e =>
            {
                DeductFunds(e.Amount);
            });
        }
    }
}