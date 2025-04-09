using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.FinanceSystem;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;

namespace _Game.Systems.UISystem.Screens
{
    public class FinanceUIScreen : BaseUIScreen<FinanceUIModel, FinanceUIView>
    {
        private readonly FinancialDataManager _dataManager;

        protected override void RegisterEvents()
        {
            // Subscribe to local view event (e.g., a refresh button)
            _view.OnOpenButtonClicked += _model.RefreshData;

            // Subscribe to EventBus events
            _eventBus.Subscribe<BalanceResetEvent>(e => _model.RefreshData());

            // Optionally, subscribe to other events if you wish:
            _eventBus.Subscribe<ChipsCreditedEvent>(e => _model.RefreshData());
            _eventBus.Subscribe<ChipsDeductedEvent>(e => _model.RefreshData());

            // Other UI state events
            _eventBus.Subscribe<GameInitializedEvent>(e => _view.Hide());
            _eventBus.Subscribe<GameStartedEvent>(e => _view.Show());
        }
    }
}