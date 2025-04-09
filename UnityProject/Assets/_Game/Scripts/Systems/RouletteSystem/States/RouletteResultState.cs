using _Game.Core.Events;
using _Game.Core.Singletons;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteResultState : BaseState
    {
        private readonly RouletteManager _manager;
        private readonly IRouletteBallService _ballService;
        private readonly IEventBus _eventBus;

        public RouletteResultState(StateMachine stateMachine, RouletteManager manager, IRouletteBallService ballService, IEventBus eventBus) : base(stateMachine)
        {
            _manager = manager;
            _ballService = ballService;
            _eventBus = eventBus;
        }

        public override void Enter()
        {
            int winningNumber = _ballService.DetectSlot();
            _eventBus.Fire(new SpinCompletedEvent { WinningNumber = winningNumber });
        }
    }
}