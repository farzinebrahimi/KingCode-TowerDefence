using System.Collections;
using UnityEngine;

namespace Managers
{
    public class AudioManager  : MonoBehaviour
    {
        [Header("Music Settings")]
        [SerializeField] private AudioClip gameplayMusic;
        [SerializeField] private AudioSource musicSource;
        
        [Header("Volume Settings")]
        [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;
        
        [Header("Fade Settings")]
        [SerializeField] private bool useFadeIn = true;
        [SerializeField] private float fadeInDuration = 2f;
        
        private void Awake()
        {
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
            }
            
            SetupAudioSource();
        }
         private void Start()
        {
            PlayMusic();
        }

        private void SetupAudioSource()
        {
            musicSource.clip = gameplayMusic;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = useFadeIn ? 0f : musicVolume;
        }

        private void PlayMusic()
        {
            if (gameplayMusic == null)
                return;

            musicSource.Play();

            if (useFadeIn)
            {
                StartCoroutine(FadeInMusic());
            }
        }

        
        //-------> Sound Settings Sections ^^  <-------
        private IEnumerator FadeInMusic()
        {
            float elapsed = 0f;
            
            while (elapsed < fadeInDuration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(0f, musicVolume, elapsed / fadeInDuration);
                yield return null;
            }
            
            musicSource.volume = musicVolume;
        }

        public void StopMusic(bool useFadeOut = true, float fadeOutDuration = 1f)
        {
            if (useFadeOut)
            {
                StartCoroutine(FadeOutMusic(fadeOutDuration));
            }
            else
            {
                musicSource.Stop();
            }
        }

        private IEnumerator FadeOutMusic(float duration)
        {
            float startVolume = musicSource.volume;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
                yield return null;
            }
            
            musicSource.volume = 0f;
            musicSource.Stop();
        }

        public void SetVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            musicSource.volume = musicVolume;
        }

        public void PauseMusic()
        {
            musicSource.Pause();
        }

        public void ResumeMusic()
        {
            musicSource.UnPause();
        }
    }
}