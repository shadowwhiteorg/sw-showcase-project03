using _Game.Interfaces;
using _Game.Systems.ChipSystem;
using _Game.Systems.UISystem.Views;

namespace _Game.Systems.UISystem.Models
{
    public class BettingUIModel : BaseUIModel
    {
        public int CurrentBet { get; private set; } = 1;
        public int UnitBet { get; private set; } = 1;
        public int TotalBet { get; set; } = 0;

        public void IncreaseBet() => SetBet( UnitBet );
        public void DecreaseBet() => SetBet(- UnitBet );
        
        public IFinancialService FinancialService { get; private set; }
        public ChipManager ChipManager { get; private set; }

        private void SetBet(int amount)
        {
            TotalBet += amount;
            NotifyUpdate();
        }

        public void SetMultiplier(int multiplier)
        {
            UnitBet = multiplier;
            NotifyUpdate();
        }

        public void ResetBetValues(BettingUIView view)
        {
            TotalBet = 0;
            CurrentBet = 1;
            UnitBet = 1;
            view.ResetViewValues();
        }

        public void SetFinancialService(IFinancialService financialService)
        {
            FinancialService = financialService;
        }

        public void SetChipManager(ChipManager chipManager)
        {
            ChipManager = chipManager;
        }
    }
}