namespace _Game.Interfaces
{
    /// <summary>
    /// Abstraction for all financial operations.
    /// </summary>
    public interface IFinancialService
    {
        /// <summary>
        /// Attempts to deduct the specified amount.
        /// </summary>
        /// <param name="amount">The amount to deduct.</param>
        /// <returns>True if deduction succeeds; otherwise, false.</returns>
        bool DeductFunds(int amount);

        /// <summary>
        /// Credits the specified amount.
        /// </summary>
        /// <param name="amount">The amount to credit.</param>
        void CreditFunds(int amount);
        float GetBalance();
        void ResetBalance(int amount);
    }
}