using _Game.Core.Events;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;
using Unity.XR.OpenVR;

namespace _Game.Systems.UISystem.Screens
{
    public class RouletteUIScreen : BaseUIScreen<RouletteUIModel, RouletteUIView>
    {
        protected override void RegisterEvents()
        {
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.Show());
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.EnableDisableRollButton(true));
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.EnableDisableToBettingButton(false));
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.ShowHideWinningNumber(false));
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.ShowHideTotalPayout(false));
            _eventBus.Subscribe<ToRouletteEvent>(e => _view.SetRandomTargetNumber());
            _eventBus.Subscribe<GameInitializedEvent>(e => _view.Hide());
            _eventBus.Subscribe<ToTableEvent>(e => _view.Hide());
            _eventBus.Subscribe<ToBettingEvent>(e => _view.Hide());
            _eventBus.Subscribe<SpinCompletedEvent>(e => OnSpinCompleted(e.WinningNumber));
            _eventBus.Subscribe<SpinCompletedEvent>(e => _view.EnableDisableToBettingButton(true));
            _eventBus.Subscribe<PayoutCalculatedEvent>(e => OnPayoutCalculated(e.TotalPayout));
            _eventBus.Subscribe<TotalBetsCalculatedEvent>(e => OnTotalBetCalculated(e.TotalBet));
            _eventBus.Subscribe<PayoutCalculatedEvent>(e => _view.ShowHideWinningNumber(true));
            _eventBus.Subscribe<PayoutCalculatedEvent>(e => _view.ShowHideTotalPayout(true));
            _eventBus.Subscribe<SpinWheelEvent>(e => _view.EnableDisableRollButton(false));
            
            _view.OnRollButtonClicked += OnRollButtonClicked;
            _view.OnToTableButtonClicked += OnToTableClicked;
            _view.OnToBettingButtonClicked += OnToBettingClicked;
            
            _view.OnTargetNumberChanged += OnTargetNumberChanged;
            
            
            _model.OnUpdated += OnModelUpdated;
        }

        private void OnTargetNumberChanged(int targetNumber)
        {
            _eventBus.Fire(new TargetNumberSetEvent(targetNumber));
        }

        protected override void OnDestroy()
        {
            _view.OnRollButtonClicked -= OnRollButtonClicked;
            _view.OnToTableButtonClicked -= OnToTableClicked;
            _view.OnToBettingButtonClicked -= OnToBettingClicked;
            _model.OnUpdated -= OnModelUpdated;
            
            base.OnDestroy();
        }
        
        private void OnModelUpdated()
        {
            // _eventBus.Fire(new BetAmountChangedEvent
            // {
            //     CurrentBet = _model.CurrentBet,
            //     Multiplier = _model.UnitBet,
            //     TotalBet = _model.TotalBet
            // });
        }

        private void OnToTableClicked() => _eventBus.Fire(new ToTableEvent());
        private void OnToBettingClicked() => _eventBus.Fire(new ToBettingEvent());
        private void OnRollButtonClicked() => _eventBus.Fire(new SpinWheelEvent());
        
        
        private void OnSpinCompleted(int winningNumber)
        {
            _model.SetWinningNumber(winningNumber);
        }

        private void OnPayoutCalculated(int payout)
        {
            _model.SetTotalPayout(payout);
        }

        private void OnTotalBetCalculated(int totalBet)
        {
            _model.SetTotalBet(totalBet);
        }
        
    }
}