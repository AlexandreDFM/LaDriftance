using Types;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        public static AudioManager Instance;

        public AudioMixer audioMixer;
        public AudioMixerGroup musicGroup;
        public AudioMixerGroup sfxGroup;

        public Sound[] musicSounds;
        public AudioSource musicSource;

        public Sound[] sfxSounds;
        public AudioSource sfxSource;

        private void Awake()
        {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            } else {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            musicSource.outputAudioMixerGroup = musicGroup;
            sfxSource.outputAudioMixerGroup = sfxGroup;

            musicSource.loop = true;

            string sceneName = SceneManager.GetActiveScene().name;

            if (musicSource != null && System.Array.Exists(musicSounds, sound => sound.name == sceneName)) {
                PlayMusicFromHandler(sceneName, musicSource);
            } else if (musicSource != null && System.Array.Exists(musicSounds, sound => sound.name == "MainTheme")) {
                PlayMusicFromHandler("MainTheme", musicSource);
            } else {
                Debug.LogWarning("No music source found!");
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            string sceneName = scene.name;

            if (musicSource != null && System.Array.Exists(musicSounds, sound => sound.name == sceneName)) {
                PlayMusicFromHandler(sceneName, musicSource);
            } else if (musicSource != null && System.Array.Exists(musicSounds, sound => sound.name == "MainTheme")) {
                PlayMusicFromHandler("MainTheme", musicSource);
            } else {
                Debug.LogWarning("No matching music found for scene: " + sceneName);
            }
        }

        public void PlaySoundFXFromHandler(string sfxName, AudioSource audioSource)
        {
            Sound sound = System.Array.Find(sfxSounds, sound => sound.name == sfxName);
            if (sound == null) {
                Debug.LogWarning("Sound: " + sfxName + " not found!");
                return;
            }

            audioSource.clip = sound.clip;
            audioSource.Play();
        }

        public void PlayMusicFromHandler(string musicName, AudioSource audioSource)
        {
            Sound sound = System.Array.Find(musicSounds, sound => sound.name == musicName);
            if (sound == null) {
                Debug.LogWarning("Sound: " + musicName + " not found!");
                return;
            }

            if (audioSource.clip == sound.clip && audioSource.isPlaying)
                return;

            audioSource.clip = sound.clip;
            audioSource.Play();
        }

        public void SetSoundFXVolume(float volume)
        {
            if (volume < -39) volume = -80;
            audioMixer.SetFloat("SoundFXVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            if (volume < -39) volume = -80;
            audioMixer.SetFloat("MusicVolume", volume);
        }

        public void SetMasterVolume(float volume)
        {
            if (volume < -39) volume = -80;
            audioMixer.SetFloat("MasterVolume", volume);
        }
    }
}
