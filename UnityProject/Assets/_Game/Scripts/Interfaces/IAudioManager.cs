using _Game.Enums;

namespace _Game.Interfaces
{
    public interface IAudioManager
    {
        // Plays a sound identified by its Key. Optionally, the sound can be positioned in 3D space.
        void PlaySound(SoundType soundKey, UnityEngine.Vector3? position = null, float volumeMultiplier = 1f);
    }
}