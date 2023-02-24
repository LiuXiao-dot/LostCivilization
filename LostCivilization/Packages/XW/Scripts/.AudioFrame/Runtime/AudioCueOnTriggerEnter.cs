using UnityEngine;

namespace TTM.AudioFrame
{
    /// <summary>
    /// 音频触发
    /// </summary>
    [RequireComponent(typeof(AudioCue))]
    public class AudioCueOnTriggerEnter : MonoBehaviour
    {
        [SerializeField] private string _tagToDetect = "Player";
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(_tagToDetect))
                GetComponent<AudioCue>().PlayAudioCue();
        }
    }
}
