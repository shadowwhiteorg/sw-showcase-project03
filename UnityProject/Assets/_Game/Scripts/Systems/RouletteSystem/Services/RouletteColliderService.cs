using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteColliderService : IRouletteColliderService
    {
        public void GenerateColliders()
        {
            // Use NumberSlot data to generate colliders in scene
            Debug.Log("Colliders generated.");
        }
    }
}