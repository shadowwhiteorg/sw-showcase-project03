using System;
using UnityEngine;

namespace _Game.Utils.Helpers
{
    public static class FinanceHelpers
    {
        public static string Timestamp { get; set; }

        // Optional: A helper property to convert this string back to DateTime when needed.
        public static DateTime DateTimeValue
        {

            get
            {
                DateTime dt;
                if (DateTime.TryParse(Timestamp, null, System.Globalization.DateTimeStyles.RoundtripKind, out dt))
                {
                    return dt;
                }

                return default(DateTime);
            }
        }
    }

}
