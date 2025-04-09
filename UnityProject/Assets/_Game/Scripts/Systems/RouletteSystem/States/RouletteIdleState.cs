using _Game.Core.Events;
using _Game.Core.Singletons;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteIdleState : BaseState
    {
        private readonly RouletteManager _manager;
        private readonly IEventBus _eventBus;
        private readonly IRouletteWheelService _wheelService;

        public RouletteIdleState(StateMachine stateMachine, RouletteManager manager, IEventBus eventBus, IRouletteWheelService wheelService) : base(stateMachine)
        {
            _manager = manager;
            _eventBus = eventBus;
            _wheelService = wheelService;
        }

        public override void Enter()
        {
            _wheelService.Reset();
        }

        public override void Tick(float deltaTime) { }
    }
}