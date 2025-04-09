using UnityEngine;

namespace _Game.Utils.Helpers
{
    public static class RouletteUtility
    {
        // Normalize an angle to be within [0, 360) degrees.
        public static float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0)
                angle += 360f;
            return angle;
        }

        // Calculates the shortest angular distance between two angles.
        public static float ShortestAngle(float from, float to)
        {
            float diff = NormalizeAngle(to - from);
            if (diff > 180f)
                diff -= 360f;
            return diff;
        }


        // Maps wheel angle to slot index based on total slot count.
        public static int AngleToSlot(float angle, int totalSlots)
        {
            float normalized = NormalizeAngle(angle);
            float slotSize = 360f / totalSlots;
            return Mathf.FloorToInt(normalized / slotSize);
        }


        // Returns the center angle of a given slot.
        public static float SlotToAngle(int slotIndex, int totalSlots)
        {
            float slotSize = 360f / totalSlots;
            return slotIndex * slotSize + slotSize / 2f;
        }

        // Debug helper to draw a circle in the Scene view.
        public static void DrawCircle(Vector3 center, float radius, int segments = 36)
        {
            Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;
            for (int i = 1; i <= segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.PI * 2f;
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
                Debug.DrawLine(prevPoint, newPoint, Color.yellow);
                prevPoint = newPoint;
            }
        }
        
    }
}