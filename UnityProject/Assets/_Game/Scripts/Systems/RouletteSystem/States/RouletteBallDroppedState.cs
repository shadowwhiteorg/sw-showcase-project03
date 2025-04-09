using _Game.Core.Events;
using _Game.Core.Singletons;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteBallDroppedState : BaseState
    {
        private readonly RouletteManager _manager;
        private readonly IRouletteBallService _ballService;
        private readonly IEventBus _eventBus;

        public RouletteBallDroppedState(StateMachine stateMachine, RouletteManager manager, IRouletteBallService ballService, IEventBus eventBus) : base(stateMachine)
        {
            _manager = manager;
            _ballService = ballService;
            _eventBus = eventBus;
        }

        public override void Enter()
        {
            // Debug log to indicate the ball has dropped.
        }

        public override void Tick(float deltaTime)
        {
            
            if (_ballService.IsBallStopped())
            {
                int slot = _ballService.DetectSlot();
                _manager.ChangeToResultState();
            }
        }
    }
}