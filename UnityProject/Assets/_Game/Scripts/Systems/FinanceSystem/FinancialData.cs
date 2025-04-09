using System;
using System.Collections.Generic;

namespace _Game.Systems.FinanceSystem
{
    [Serializable]
    public class FinancialData
    {
        public float Balance;
        public List<TransactionEntry> Transactions = new List<TransactionEntry>();
    }
}