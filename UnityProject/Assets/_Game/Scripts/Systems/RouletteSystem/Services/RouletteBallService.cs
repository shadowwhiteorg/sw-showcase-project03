using System.Linq;
using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Utils.Helpers;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteBallService : IRouletteBallService
    {
        private readonly RouletteContext _ctx;
        private bool _isOrbiting;
        private IRouletteWheelService _wheelService;
        private IEventBus _eventBus;

        public RouletteBallService(RouletteContext ctx, IRouletteWheelService wheelService, IEventBus eventBus)
        {
            _eventBus = eventBus;
            _ctx = ctx;
            _wheelService = wheelService;
        }

        public void AttachToWheel()
        {
            Debug.Log("Attaching ball to wheel");
            _isOrbiting = true;
            _ctx.BallView.ResetBall(_ctx.WheelView.BallTarget);
            _ctx.BallView.SetupOrbit(_ctx.WheelCenter, _ctx.Config.orbitRadius);
        }

        public void FixedTick(float deltaTime)
        {
            if (_isOrbiting)
            {
                float wheelSpeed = _ctx.WheelView.GetAngularSpeed();
                _ctx.BallView.OrbitTick(deltaTime, wheelSpeed, _ctx.WheelView.BallTarget);
                if (_wheelService.IsBelowThreshold())
                    ReleaseToPhysics();
            }
        }

        public void ReleaseToPhysics()
        {
            _eventBus.Fire(new BallDroppedEvent());
            _ctx.BallView.ReleaseBall(_ctx.WheelView.GetAngularSpeed());
            _isOrbiting = false;
        }

        public int DetectSlot()
        {
            float ballAngle = RouletteUtility.NormalizeAngle(_ctx.BallView.GetAngleRelativeToCenter());
            float wheelAngle = RouletteUtility.NormalizeAngle(_ctx.WheelCenter.localEulerAngles.y);
            float relAngle = RouletteUtility.NormalizeAngle(ballAngle - wheelAngle);

            var closestSlot = _ctx.WheelView.NumberSlots
                .Select(slot => new { Slot = slot, Distance = Mathf.Abs(relAngle - NumberSlotAngle(slot)) })
                .OrderBy(x => x.Distance)
                .FirstOrDefault();

            return closestSlot?.Slot.Number ?? -2;
        }

        private Vector3 _lastPosition;
        private float _stuckTime;

        public bool IsBallStopped()
        {
            Vector3 currentPosition = _ctx.BallView.transform.position;
            if (Mathf.Abs(Vector3.Distance(currentPosition, _lastPosition)) < 0.01f)
            {
                _stuckTime += Time.deltaTime;
                if (_stuckTime >= 2f)
                {
                    return true;
                }
            }
            else
            {
                _stuckTime = 0f;
            }

            _lastPosition = currentPosition;
            return false;
        }
        
       
        

        private float NumberSlotAngle(NumberSlotData numberSlot)
        {
            Vector3 dir = numberSlot.transform.position - _ctx.WheelCenter.position;
            float slotAngle = RouletteUtility.NormalizeAngle(Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg);
            float wheelAngle = RouletteUtility.NormalizeAngle(_ctx.WheelCenter.localEulerAngles.y);
            return RouletteUtility.NormalizeAngle(slotAngle - wheelAngle);
        }

        // Reset the ball to its initial state (attached to the wheel).
        public void Reset()
        {
            _isOrbiting = false;
            _ctx.BallView.ResetBall(_ctx.WheelView.BallTarget);
        }
    }
}