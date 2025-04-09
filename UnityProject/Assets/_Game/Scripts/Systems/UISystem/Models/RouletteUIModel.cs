namespace _Game.Systems.UISystem.Models
{
    public class RouletteUIModel : BaseUIModel
    {
        public int TotalBet { get; private set; } = 0;
        public int TotalPayout{ get; private set; } = 0;
        public int WinningNumber { get; private set; } = 0;
        
        public void SetTotalBet(int amount)
        {
            TotalBet = amount;
            NotifyUpdate();
        }
        public void SetTotalPayout(int amount)
        {
            TotalPayout = amount;
            NotifyUpdate();
        }
        public void SetWinningNumber(int number)
        {
            WinningNumber = number;
            NotifyUpdate();
        }
        
    }
}