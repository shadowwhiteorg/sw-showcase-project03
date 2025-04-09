using _Game.Enums;
using UnityEngine;

namespace _Game.Systems.AudioSystem
{
    [System.Serializable]
    public class SoundData
    {
        public SoundType Key;
        public AudioClip Clip;
        public float DefaultVolume = 1f;
    }
}