using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Multiplatform.Podcasts
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioClipController : MonoBehaviour
    {
        [HideInInspector]
        public UnityEvent<string> onEnable;
        [HideInInspector]
        public UnityEvent<float> onUpdate;

        AudioSource source;
        AudioClip clip;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (source.isPlaying)
            {
                onUpdate?.Invoke(source.time / clip.length);
            }
        }
        public void SetClip(AudioClip _clip)
        {
            clip = _clip;
            source.clip = clip;

            onEnable?.Invoke(_clip.name);
        }

        public void Play()
        {
            source.Play();
        }
        public void Pause()
        {
            source.Pause();
        }
        public void Stop()
        {
            source.Stop();
        }

        public void SetTime(float _time)
        {
            source.Pause();
            source.time = _time;
            Debug.Log(source.time + ", " + _time);
            source.UnPause();
        }
    }
}