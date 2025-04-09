using UnityEngine;

namespace _Game.Systems.ChipSystem
{
    public class Chip : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        private int _denomination;
        private ChipStack _parentStack;
        
        public int Denomination => _denomination;

        public void Initialize(ChipData data)
        {
            _denomination = data.Denomination;
            _renderer.material = data.Material;
        }

        public void SetStack(ChipStack stack) => _parentStack = stack;
    }
}