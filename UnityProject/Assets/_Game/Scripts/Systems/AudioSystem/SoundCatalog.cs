using System.Collections.Generic;
using _Game.Enums;
using UnityEngine;

namespace _Game.Systems.AudioSystem
{
    /// <summary>
    /// Stores a list of sounds that can be looked up by Key. This allows the AudioManager to remain free of direct data.
    /// </summary>
    [CreateAssetMenu(menuName = "Roulette/Sound Catalogue", fileName = "SoundCatalogue")]
    public class SoundCatalogue : ScriptableObject
    {
        [Tooltip("List of sound data entries used by the AudioManager.")]
        public List<SoundData> sounds = new List<SoundData>();

        private Dictionary<SoundType, SoundData> _soundDictionary;

        private void OnEnable()
        {
            // Build a dictionary for quick lookup by Key.  
            _soundDictionary = new Dictionary<SoundType, SoundData>();
            foreach (var sound in sounds)
            {
                if (!_soundDictionary.ContainsKey(sound.Key))
                {
                    _soundDictionary.Add(sound.Key, sound);
                }
                else
                {
                    Debug.LogWarning($"Duplicate sound Key found in catalogue: {sound.Key}");
                }
            }
        }

        // Retrieves sound data based on its Key.  
        public SoundData GetSound(SoundType key)
        {
            if (_soundDictionary != null && _soundDictionary.TryGetValue(key, out SoundData soundData))
                return soundData;

            Debug.LogWarning($"Sound with Key '{key}' not found in the catalogue.");
            return null;
        }
    }
}