using UnityEngine;

namespace TTM.AudioFrame
{
    /// <summary>
    /// “Ù∆µ…Ë÷√”¶”√
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioConfigApplier : MonoBehaviour
    {
        public AudioConfigurationSO config;

        private void OnValidate()
        {
            ConfigureAudioSource();
        }

        private void Start()
        {
            ConfigureAudioSource();
        }

        private void ConfigureAudioSource()
        {
            if (config != null)
            {
                AudioSource audioSource = GetComponent<AudioSource>();
                config.ApplyTo(audioSource);
            }
        }
    }
}