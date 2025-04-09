using _Game.Core.Events;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;
using UnityEngine;

namespace _Game.Systems.UISystem.Screens
{
    public class BettingUIScreen : BaseUIScreen<BettingUIModel, BettingUIView>
    {
        protected override void RegisterEvents()
        {
            // Subscribe to view events
            _view.OnIncreaseBet += OnIncreaseBet;
            _view.OnDecreaseBet += OnDecreaseBet;
            _view.OnIncreaseBet += _model.IncreaseBet;
            _view.OnDecreaseBet += _model.DecreaseBet;
            _view.OnToTableClicked += OnToTableClicked;
            _view.OnMultiplierButtonClicked += _model.SetMultiplier;
            
            // Subscribe to model changes
            _model.OnUpdated += OnModelUpdated;
            
            // Subscribe to EventBus events
            _eventBus.Subscribe<GameInitializedEvent>(e => _view.Hide());
            _eventBus.Subscribe<GameInitializedEvent>(e => _view.HideTutorialHand());
            _eventBus.Subscribe<GameStartedEvent>(e => _view.Show());
            _eventBus.Subscribe<ToBettingEvent>(e => _view.Show());
            _eventBus.Subscribe<ToBettingEvent>(e => _model.ResetBetValues(_view));
            _eventBus.Subscribe<ChipDragStartedEvent>(e => _view.Hide());
            _eventBus.Subscribe<ChipDragStartedEvent>(e => _view.HideTutorialHand());
            _eventBus.Subscribe<FinanceSystemInitialized>(e=> _model.SetFinancialService(e.FinancialService));
            _eventBus.Subscribe<ChipSystemInitialized>(e=> _model.SetChipManager(e.ChipManager));
            _eventBus.Subscribe<MultiplierSetEvent>(e => OnMultiplierButtonClicked(e.Multiplier));
            
        }

        private void OnIncreaseBet()
        {
            _eventBus.Fire(new IncreaseBetEvent(_model.UnitBet));
            _view.ShowTutorialHand();
        }

        private void OnDecreaseBet() => _eventBus.Fire(new DecreaseBetEvent(_model.UnitBet));
        private void OnToTableClicked()
        {
            _eventBus.Fire(new ToTableEvent());
        }
        private void OnMultiplierButtonClicked(int multiplier)
        {
            _eventBus.Fire(new MultiplierSetEvent(multiplier));
            _view.SetPlusButton(_model.UnitBet);
            _view.SetMinusButton(_model.UnitBet);
            _view.SetMultiplierButtons();
        }

        private void OnModelUpdated()
        {
            _eventBus.Fire(new BetAmountChangedEvent {
                Multiplier = _model.UnitBet,
                TotalBet = _model.TotalBet
            });
        }
        protected override void OnDestroy()
        {
            _view.OnIncreaseBet -= OnIncreaseBet;
            _view.OnDecreaseBet -= OnDecreaseBet;
            _view.OnToTableClicked -= OnToTableClicked;
        
            _model.OnUpdated -= OnModelUpdated;
            base.OnDestroy();
        }
    }
}