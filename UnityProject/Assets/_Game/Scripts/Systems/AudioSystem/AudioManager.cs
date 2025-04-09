using _Game.Core.Events;
using _Game.Enums;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [Tooltip("Reference to the Sound Catalogue containing all audio clips.")] [SerializeField]
        private SoundCatalogue catalogue;

        [SerializeField] private AudioSource defaultAudioSource;
        private IEventBus _eventBus;
        private AudioSource _trackedAudioSource;

        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            SubscribeToEvents();
            if (defaultAudioSource == null)
            {
                defaultAudioSource = GetComponent<AudioSource>();
                defaultAudioSource.playOnAwake = false;
            }
        }

        public void PlaySound(SoundType soundKey, Vector3? position = null, float volumeMultiplier = 1f)
        {
            var soundData = catalogue.GetSound(soundKey);
            if (soundData == null || soundData.Clip == null)
                return;

            float finalVolume = soundData.DefaultVolume * volumeMultiplier;

            if (position.HasValue)
            {
                AudioSource.PlayClipAtPoint(soundData.Clip, position.Value, finalVolume);
            }
            else
            {
                defaultAudioSource.PlayOneShot(soundData.Clip, finalVolume);
            }
        }

        private AudioSource PlayTrackedSound(SoundType soundKey, Vector3? position = null, float volumeMultiplier = 1f)
        {
            var soundData = catalogue.GetSound(soundKey);
            if (soundData == null || soundData.Clip == null)
                return null;

            float finalVolume = soundData.DefaultVolume * volumeMultiplier;
            AudioSource source = null;

            if (position.HasValue)
            {
                // Create a new GameObject with an AudioSource for spatial sound
                GameObject go = new GameObject($"Audio_{soundKey}");
                go.transform.position = position.Value;
                source = go.AddComponent<AudioSource>();
                source.clip = soundData.Clip;
                source.volume = finalVolume;
                source.spatialBlend = 1f; // Fully 3D
                source.Play();

                Destroy(go, soundData.Clip.length);
            }
            else
            {
                GameObject go = new GameObject($"Audio_{soundKey}");
                go.transform.SetParent(transform);
                source = go.AddComponent<AudioSource>();
                source.clip = soundData.Clip;
                source.volume = finalVolume;
                source.playOnAwake = false;
                source.Play();
                _trackedAudioSource = source;

            }

            return source;
        }

        private void StopTrackedSound()
        {
            _trackedAudioSource?.Stop();
        }

        public void SubscribeToEvents()
        {
            _eventBus.Subscribe<GameStartedEvent>(e => PlaySound(SoundType.Click));
            _eventBus.Subscribe<ChipDragStartedEvent>(e => PlaySound(SoundType.PickStack));
            _eventBus.Subscribe<BetPlacedEvent>(e => PlaySound(SoundType.BetPlaced));
            _eventBus.Subscribe<SpinStartedEvent>(e => PlayTrackedSound(SoundType.Spin));
            _eventBus.Subscribe<BallDroppedEvent>(e =>
            {
                StopTrackedSound();
                PlaySound(SoundType.BallDrop);
            });
            _eventBus.Subscribe<ChipCreatedEvent>(e => PlaySound(SoundType.ChipDrop));
            _eventBus.Subscribe<WinEvent>(e => PlaySound(SoundType.Win));
            _eventBus.Subscribe<LoseEvent>(e => PlaySound(SoundType.Lose));
        }
    }
}  
    