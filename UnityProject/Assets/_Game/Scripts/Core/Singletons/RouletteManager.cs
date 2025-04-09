using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.RouletteSystem;

namespace _Game.Core.Singletons
{
    // RouletteManager now implements IUpdatableManager and includes a dedicated Reset state.
    public class RouletteManager : IUpdatableManager
    {
        private readonly IRouletteWheelService _wheelService;
        private readonly IRouletteBallService _ballService;
        private readonly IEventBus _eventBus;
        private readonly StateMachine _stateMachine;

        private readonly BaseState _idleState;
        private readonly BaseState _spinningState;
        private readonly BaseState _ballDroppedState;
        private readonly BaseState _resultState;
        private readonly BaseState _resetState;

        public RouletteManager(IRouletteWheelService wheelService, IRouletteBallService ballService, IEventBus eventBus)
        {
            _wheelService = wheelService;
            _ballService = ballService;
            _eventBus = eventBus;
            _stateMachine = new StateMachine();

            // Instantiate the states with appropriate dependencies.
            _idleState = new RouletteIdleState(_stateMachine, this, _eventBus, _wheelService);
            _spinningState = new RouletteSpinningState(_stateMachine, this, _wheelService, _ballService, _eventBus);
            _ballDroppedState = new RouletteBallDroppedState(_stateMachine, this, _ballService, _eventBus);
            _resultState = new RouletteResultState(_stateMachine, this, _ballService, _eventBus);
            _resetState = new RouletteResetState(_stateMachine, this, _wheelService, _ballService, _eventBus);

            // Start in the Idle state.
            _stateMachine.ChangeState(_idleState);

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            // When the ToRouletteEvent is fired (e.g., by a UI reset button), transition into Reset state.
            _eventBus.Subscribe<ToRouletteEvent>(_ => ResetRoulette());
            _eventBus.Subscribe<SpinWheelEvent>(_ => ChangeToSpinningState());
            _eventBus.Subscribe<ExitRouletteEvent>(_ => ChangeToIdleState());
        }

        private void ResetRoulette()
        {
            _stateMachine.ChangeState(_resetState);
        }

        // Helper method for the Reset state to return to Idle.
        public BaseState GetIdleState() => _idleState;

        public void Update(float deltaTime)
        {
            _stateMachine?.Tick(deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            _stateMachine?.FixedTick(fixedDeltaTime);
        }
        public void ChangeToIdleState() => _stateMachine.ChangeState(_idleState);
        public void ChangeToSpinningState() => _stateMachine.ChangeState(_spinningState);
        public void ChangeToBallDroppedState() => _stateMachine.ChangeState(_ballDroppedState);
        public void ChangeToResultState() => _stateMachine.ChangeState(_resultState);
    }
}
