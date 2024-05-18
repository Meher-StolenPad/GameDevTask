using Davanci;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Davanci
{
    public class SoundObjectRespawner : MonoBehaviour
    {
        [SerializeField] private AudioSource AudioSource;
        private float ClipLength;

        public void Init(AudioClip clip, float randomVolume, float randomPitch)
        {
            AudioSource.clip = clip;
            AudioSource.volume = randomVolume;
            AudioSource.pitch = randomPitch;
            AudioSource.Play();
            StartCoroutine(Despawn());
        }
        private IEnumerator Despawn()
        {
            ClipLength = AudioSource.clip.length;
            yield return new WaitForSeconds(ClipLength);
            SoundManager.Instance.DespawnFromPool(this);
        }
    }
}

