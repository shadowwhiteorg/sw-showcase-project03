using System;
using _Game.Systems.ChipSystem;
using UnityEngine;

namespace _Game.Core.Singletons
{
    public class InputManager : MonoBehaviour
    {
        private ChipDragger _chipDragger;

        public void Initialize(ChipDragger chipDragger)
        {
            _chipDragger = chipDragger;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                
            }
        }
    }
}