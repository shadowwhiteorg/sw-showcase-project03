using System.Linq;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteWheelService : IRouletteWheelService
    {
        private readonly RouletteContext _context;
        private float _spinTime;
        private float _currentAngle;
        private bool _isSpinning;
        private bool _canCheckDropCondition;

        public RouletteWheelService(RouletteContext context)
        {
            _context = context;
        }

        public void StartSpin()
        {
            _spinTime = 0f;
            _currentAngle = 0f;
            _isSpinning = true;
            _canCheckDropCondition = false;
        }
        
        public void FixedTick(float deltaTime)
        {
            if (!_isSpinning)
                return;

            _spinTime += deltaTime;
            if (_spinTime > _context.Config.spinDuration)
                _spinTime = _context.Config.spinDuration;

            float t = _spinTime / _context.Config.spinDuration;
            float curveValue = _context.Config.spinCurve.Evaluate(t);
            float wheelSpeed = _context.Config.maxAngularSpeed * curveValue;

            _currentAngle += wheelSpeed * deltaTime;
            _currentAngle %= 360f;

            _context.WheelView.ApplyRotation(Quaternion.Euler(0f, _currentAngle, 0f));
            _context.WheelView.SetAngularSpeed(wheelSpeed);

            if (!_canCheckDropCondition && wheelSpeed > _context.Config.dropThresholdSpeed)
            {
                _canCheckDropCondition = true;
            }
        }

        public void ApplyDeceleration(){}
        
        public bool WheelStopped() => _context.WheelView.GetAngularSpeed() <= 0f;
        public bool IsBelowThreshold() =>
            _context.WheelView.GetAngularSpeed() < _context.Config.dropThresholdSpeed && _canCheckDropCondition;
        public float GetCurrentAngle() => _currentAngle;

        // Reset method to reinitialize the wheel.
        public void Reset()
        {
            _isSpinning = false;
            _spinTime = 0f;
            _currentAngle = 0f;
            _canCheckDropCondition = false;
            _context.WheelView.ResetRotation();
        }
        
        public int DetectSlot()
        {
            // 1) Compute relative angle of the ball
            float ballAngle = NormalizeAngle(GetAngleRelativeToCenter());
        
            // 2) Possibly subtract the wheel’s angle if your system needs that:
            float wheelAngle = NormalizeAngle(_context.WheelView.GetAngularSpeed() /* or wheel rotation angle */);
            float relAngle = NormalizeAngle(ballAngle - wheelAngle);

            // 3) Find the slot whose angle is closest
            var wheelView = _context.WheelView;
            var bestSlot = wheelView.NumberSlots
                .Select(slot => new { 
                    Slot = slot, 
                    Dist = Mathf.Abs(relAngle - wheelView.NumberSlotAngle(slot)) 
                })
                .OrderBy(x => x.Dist)
                .FirstOrDefault();

            return bestSlot?.Slot.Number ?? -1;
        }

        private float GetAngleRelativeToCenter()
        {
            // e.g. transform or ball reference
            Vector3 dir = _context.BallView.transform.position - _context.WheelView.WheelCenter.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            return angle;
        }

        private float NormalizeAngle(float angleDeg)
        {
            angleDeg = angleDeg % 360f;
            if (angleDeg < 0) angleDeg += 360f;
            return angleDeg;
        }

    }
}