using System.Collections.Generic;
using _Game.Systems.FinanceSystem;

namespace _Game.Systems.UISystem.Models
{
    public class FinanceUIModel : BaseUIModel
    {
        private readonly FinancialDataManager _dataManager;

        // Constructor
        public float CurrentBalance { get; private set; }
        public List<TransactionEntry> Transactions { get; private set; }

        public FinanceUIModel(FinancialDataManager dataManager)
        {
            _dataManager = dataManager;
            RefreshData();
        }

        public void RefreshData()
        {
            // Reload persisted data
            _dataManager.Load();
            CurrentBalance = _dataManager.Data.Balance;
            Transactions = new List<TransactionEntry>(_dataManager.Data.Transactions);
            NotifyUpdate();
        }
    }
}
