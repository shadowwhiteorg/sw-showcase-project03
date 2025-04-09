using _Game.Core.Events;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Views;

namespace _Game.Systems.UISystem.Screens
{
    public class StartUIScreen : BaseUIScreen<StartUIModel, StartUIView>
    {
        protected override void RegisterEvents()
        {
            base.RegisterEvents();
            // Subscribe to view events
            _view.OnStartClicked += OnStartClicker;
            
            // Subscribe to EventBus Events
            
            _eventBus.Subscribe<GameInitializedEvent>(e=> _view.Show());
            _eventBus.Subscribe<GameStartedEvent>(e=> _view.Hide());
        }
        private void OnStartClicker() => _eventBus.Fire(new GameStartedEvent());

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _view.OnStartClicked -= OnStartClicker;
        }
    }
}