using _Game.Core.Events;
using _Game.Core.Singletons;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteResetState : BaseState
    {
        private readonly RouletteManager _manager;
        private readonly IRouletteWheelService _wheelService;
        private readonly IRouletteBallService _ballService;
        private readonly IEventBus _eventBus;

        public RouletteResetState(StateMachine stateMachine, RouletteManager manager, IRouletteWheelService wheelService, IRouletteBallService ballService, IEventBus eventBus)
            : base(stateMachine)
        {
            _manager = manager;
            _wheelService = wheelService;
            _ballService = ballService;
            _eventBus = eventBus;
        }

        public override void Enter()
        {
            Debug.Log("Entering RouletteResetState");
            // Reset the wheel and ball systems.
            _wheelService.Reset();
            _ballService.Reset();

            // Optionally, fire a ResetCompletedEvent for any listeners.
            _eventBus.Fire(new ResetCompletedEvent());

            // After reset, immediately switch to Idle state.
            StateMachine.ChangeState(_manager.GetIdleState());
        }

        public override void Tick(float deltaTime) { }
        public override void FixedTick(float fixedDeltaTime) { }

        public override void Exit()
        {
            Debug.Log("Exiting RouletteResetState");
        }
    }
}