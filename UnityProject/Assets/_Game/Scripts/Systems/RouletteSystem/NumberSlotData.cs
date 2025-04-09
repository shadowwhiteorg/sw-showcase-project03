using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    [ExecuteInEditMode]
    public class NumberSlotData : MonoBehaviour
    {
        public int Number;         // The actual number of the slot
        public int Index;          // Position/order index
        public Vector3 Position;   // Local or world position (optional for debugging)
    }
}