namespace _Game.Systems.UISystem.Models
{
    public class GameOverModel : BaseUIModel
    {
        public string ResultMessage { get; private set; }
        public int FinalBalance { get; private set; }

        public void SetResult(bool isWin, int balance)
        {
            ResultMessage = isWin ? "Victory!" : "Game Over";
            FinalBalance = balance;
            NotifyUpdate();
        }
    }
}