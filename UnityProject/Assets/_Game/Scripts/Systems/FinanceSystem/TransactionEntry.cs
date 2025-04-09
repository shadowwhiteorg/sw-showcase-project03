using System;

namespace _Game.Systems.FinanceSystem
{
    [Serializable]
    public class TransactionEntry
    {
        public string Timestamp;
        public string Description;
        public float Amount; // positive for credits, negative for deductions

        // Optional: Helper property to parse the timestamp back to a DateTime.
        public DateTime DateTimeValue
        {
            get
            {
                DateTime dt;
                return DateTime.TryParse(Timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out dt)
                    ? dt
                    : default(DateTime);
            }
        }
    }
}