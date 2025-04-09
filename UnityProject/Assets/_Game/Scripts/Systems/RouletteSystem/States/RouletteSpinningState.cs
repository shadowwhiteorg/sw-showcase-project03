using _Game.Core.Events;
using _Game.Core.Singletons;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteSpinningState : BaseState
    {
        private readonly RouletteManager _manager;
        private readonly IRouletteWheelService _wheelService;
        private readonly IRouletteBallService _ballService;
        private readonly IEventBus _eventBus;

        public RouletteSpinningState(StateMachine stateMachine, RouletteManager manager, IRouletteWheelService wheelService, IRouletteBallService ballService, IEventBus eventBus) : base(stateMachine)
        {
            _manager = manager;
            _wheelService = wheelService;
            _ballService = ballService;
            _eventBus = eventBus;
        }

        public override void Enter()
        {
            _ballService.AttachToWheel();
            _eventBus.Fire(new SpinStartedEvent());
            _wheelService.StartSpin();
            
        }

        public override void Tick(float deltaTime)
        {
            // _wheelService.FixedTick(deltaTime);
            // _ballService.FixedTick(deltaTime);
            //
            // if (_wheelService.WheelStopped())
            //     _manager.ChangeToBallDroppedState();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            base.FixedTick(fixedDeltaTime);
            _wheelService.FixedTick(fixedDeltaTime);
            _ballService.FixedTick(fixedDeltaTime);

            if (_wheelService.WheelStopped())
                _manager.ChangeToBallDroppedState();
        }
    }


}