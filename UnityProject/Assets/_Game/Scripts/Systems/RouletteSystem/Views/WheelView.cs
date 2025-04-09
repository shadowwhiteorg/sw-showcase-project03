using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class WheelView : MonoBehaviour
    {
        // (Optional) If your wheel prefab has children for numbers/colliders
        [SerializeField] private Transform numberParent;
        [SerializeField] private Transform collidersParent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Transform ballTarget;
        [SerializeField] private Transform wheelCenter;
        [SerializeField] private Transform seperatorParent;
        private float _angularSpeed;
        private List<NumberSlotData> numberSlots = new List<NumberSlotData>();

        public Transform NumberParent => numberParent;
        public Transform CollidersParent => collidersParent;
        public Transform BallTarget => ballTarget;
        public List<NumberSlotData> NumberSlots => numberSlots;
        
        public Transform WheelCenter => wheelCenter;

        public void Construct()
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            numberSlots.AddRange(seperatorParent.GetComponentsInChildren<NumberSlotData>());
        }

        public void ApplyRotation(Quaternion rotation)
        {
            // Kinematic motion
            rb.MoveRotation(rotation);
        }

        public void SetAngularSpeed(float speed)
        {
            _angularSpeed = speed;
        }

        public float GetAngularSpeed()
        {
            return _angularSpeed;
        }
        
        public float GetLinearSpeed()
        {
            return rb.linearVelocity.magnitude;
        }

        public void ResetRotation()
        {
            rb.MoveRotation(Quaternion.identity);
            _angularSpeed = 0f;
        }
        
        public NumberSlotData GetSlotData(int number)
        {
            return NumberSlots.FirstOrDefault(ns => ns.Number == number);
        }
        
        public float NumberSlotAngle(NumberSlotData slot)
        {
            if (!slot) return -1f;

            Vector3 dir = slot.transform.position - wheelCenter.position;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            angle = (angle + 360f) % 360f;
            return angle;
        }
    }
}