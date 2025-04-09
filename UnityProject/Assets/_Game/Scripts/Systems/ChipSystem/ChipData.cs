using System;
using UnityEngine;

namespace _Game.Systems.ChipSystem
{
    [CreateAssetMenu(menuName = "Roulette/Chip Data")]
    public class ChipData : ScriptableObject
    {
        public int Denomination;
        public Material Material;
        public GameObject Prefab;
        public Color DisplayColor;
    }
}