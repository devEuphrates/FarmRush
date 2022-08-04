using System.Collections.Generic;
using UnityEngine;

namespace Euphrates
{
	public class SoundManager : Singleton<SoundManager>
	{
        [SerializeField]List<AudioSource> _sources;
        [SerializeField] List<ClipData> _clips;

        [Header("Randomization"), Space]
        [Range(0f, 9f)]
        [SerializeField] float _volumeMultiplierMin = 0.2f;
        [Range(-3f, 3f)]
        [SerializeField] float _pitchMultiplierMin = 0.2f;
        [Range(-3f, 3f)]
        [SerializeField] float _pitchMultiplierMax = 1.4f;

        protected override void Awake()
        {
            base.Awake();
        }

        public AudioSource GetAvailableSource()
        {
            foreach (var src in _sources)
            {
                if (!src.isPlaying)
                    return src;
            }

            return null;
        }

        public static void PlayClip(string name, int layer, float maxVolume = 1f, bool randomize = false)
        {
            AudioClip clip = null;
            foreach (var c in Instance._clips)
            {
                if (c.Name != name)
                    continue;

                clip = c.Clip;
                break;
            }

            if (clip == null)
                return;

            AudioSource source = Instance._sources[layer];
            if (source == null)
                return;

            float vol = randomize ? Random.Range(Instance._volumeMultiplierMin, Mathf.Clamp01(maxVolume)) : maxVolume;
            float pitch = randomize ? Random.Range(Instance._pitchMultiplierMin, Instance._pitchMultiplierMax) : source.pitch;

            source.clip = clip;
            source.volume = vol;
            source.pitch = pitch;

            source.PlayOneShot(source.clip);
        }
    }

    [System.Serializable]
	public struct ClipData
    {
		public string Name;
		public AudioClip Clip;
    }
}
