using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    [Serializable]
    public class StrAudioClipCollectionDictionary : SerializableDictionary<string, AudioClip[]>
    {
    }

    [Serializable]
    public class ConcreteClip
    {
        public AudioClip clip;
        
        [Range(0, 1)]
        public float volume = 1;
    }
    [Serializable]
    public class NamedAudioCollection
    {
        public string name;
        public ConcreteClip[] clips;
    }
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager: MonoBehaviour
    {
        [SerializeField]
        private NamedAudioCollection[] clips;

        private AudioSource _audioSource;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayAudio(string name)
        {
            foreach (var clipCollection in clips)
            {
                if (clipCollection.name == name)
                {
                    int index = Random.Range(0, clipCollection.clips.Length);
                    var clip = clipCollection.clips[index];
                    _audioSource.PlayOneShot(clip.clip, clip.volume);
                }
            }
        }
    }
}