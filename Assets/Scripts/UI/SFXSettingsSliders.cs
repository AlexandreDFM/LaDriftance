using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SoundFXSettingsSliders : MonoBehaviour
    {
        public Slider masterSlider;
        public TextMeshProUGUI masterText;
        public Slider musicSlider;
        public TextMeshProUGUI musicText;
        public Slider sfxSlider;
        public TextMeshProUGUI sfxText;

        private void Start()
        {
            // Initialiser les valeurs actuelles (optionnel)
            float volume;
            if (!AudioManager.Instance) return;

            if (AudioManager.Instance.audioMixer.GetFloat("MasterVolume", out volume))
            {
                masterSlider.value = volume;
                masterText.text = Mathf.RoundToInt(volume + 100) + " %";
            }

            if (AudioManager.Instance.audioMixer.GetFloat("MusicVolume", out volume))
            {
                musicSlider.value = volume;
                musicText.text = Mathf.RoundToInt(volume + 100) + " %";
            }

            if (AudioManager.Instance.audioMixer.GetFloat("SoundFXVolume", out volume))
            {
                sfxSlider.value = volume;
                sfxText.text = Mathf.RoundToInt(volume + 100) + " %";
            }

            // Ajouter les listeners
            masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMasterVolume);
            masterSlider.onValueChanged.AddListener(value => masterText.text = Mathf.RoundToInt(value + 100) + " %");

            musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
            musicSlider.onValueChanged.AddListener(value => musicText.text = Mathf.RoundToInt(value + 100) + " %");

            sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSoundFXVolume);
            sfxSlider.onValueChanged.AddListener(value => sfxText.text = Mathf.RoundToInt(value + 100) + " %");
        }
    }
}