namespace _Game.Systems.UISystem.Models
{
    public class TableUIModel : BaseUIModel
    {
        public int TotalBets { get; set; } = 1;
        public int TotalPayouts { get; set; } = 1;
        
        public void SetTotalBets(int amount)
        {
            TotalBets = amount;
            NotifyUpdate();
        }
    }
}