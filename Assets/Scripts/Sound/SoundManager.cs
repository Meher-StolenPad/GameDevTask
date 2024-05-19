using DaVanciInk.GenericPooler;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Davanci
{
    public enum ClipFx
    {
        CardFlipped,
        CardMatch,
        CardUnMatch,
        LevelCompleted
    }
    public class SoundManager : SingletonMB<SoundManager>
    {
        public static bool SoundActivated = true;
        public Pooler<SoundObjectRespawner> AudioSourcePool;
        public static Action<bool> OnSoundSateChangedCallback;

        private float VolumeChangeMultiplier = 0.15f;
        private float PitchChangeMultiplier = 0.1f;


        private void Start()
        {
            AudioSourcePool.Initialize(transform);
            GameManager.OnCardStartFlippingCallback += OnCardFlipped;
            GameManager.OnMoveCallback += OnTry;
            GameManager.OnLevelCompletedCallback += OnLevelCompleted;
            OnSoundSateChangedCallback += OnSoundStateChanged;
        }

        private void OnSoundStateChanged(bool state)
        {
            SoundActivated = state;
        }

        private void OnDisable()
        {
            GameManager.OnCardStartFlippingCallback -= OnCardFlipped;
            GameManager.OnMoveCallback -= OnTry;
            GameManager.OnLevelCompletedCallback -= OnLevelCompleted;
        }
        private void OnLevelCompleted(LevelCompletedData levelCompletedData)
        {
            PlaySoundFxClip(Database.GetAudioClip(ClipFx.LevelCompleted));
        }

        private void OnTry(int count, bool isMatch)
        {
            PlaySoundFxClip(Database.GetAudioClip(isMatch ? ClipFx.CardMatch : ClipFx.CardUnMatch));
        }

        private void OnCardFlipped()
        {
            PlaySoundFxClip(Database.GetAudioClip(ClipFx.CardFlipped));
        }

        public void PlaySoundFxClip(AudioClip clip)
        {
            if (!SoundActivated) return;

            float randomVolume = Random.Range(1 - VolumeChangeMultiplier, 1 + VolumeChangeMultiplier);
            float randomPitch = Random.Range(1 - PitchChangeMultiplier, 1 + VolumeChangeMultiplier);

            SoundObjectRespawner a = Instance.AudioSourcePool.UpdateFromPool(1, true, Vector3.zero).Single;

            a.Init(clip, randomVolume, randomPitch);
        }
        public void DespawnFromPool(SoundObjectRespawner audioSourceRespawner)
        {
            AudioSourcePool.DespawnObjectFromPool(audioSourceRespawner);
        }
    }
}