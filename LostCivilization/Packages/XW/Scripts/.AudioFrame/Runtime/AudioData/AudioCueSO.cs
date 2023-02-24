
using System;
using UnityEngine;
namespace TTM.AudioFrame
{
    /// <summary>
    /// 音频信号 一个声音多个clip
    /// </summary>
    public class AudioCueSO : ScriptableObject
    {
        public bool looping = false;
        [SerializeField] private AudioClipsGroup[] _audioClipGroups = default;

        public AudioClip[] GetClips()
        {
            int numberOfClips = _audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numberOfClips];

            for (int i = 0; i < numberOfClips; i++)
            {
                resultingClips[i] = _audioClipGroups[i].GetNextClip();
            }

            return resultingClips;
        }
    }

    /// <summary>
    /// 多个AudioClip集合
    /// </summary>
    [Serializable]
    public class AudioClipsGroup
    {
        public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] audioClips;

        private int _nextClipToPlay = -1;
        private int _lastClipPlayed = -1;

        /// <summary>
        /// Chooses the next clip in the sequence, either following the order or randomly.
        /// </summary>
        /// <returns>A reference to an AudioClip</returns>
        public AudioClip GetNextClip()
        {
            // Fast out if there is only one clip to play
            if (audioClips.Length == 1)
                return audioClips[0];

            if (_nextClipToPlay == -1)
            {
                // Index needs to be initialised: 0 if Sequential, random if otherwise
                _nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
            }
            else
            {
                // Select next clip index based on the appropriate SequenceMode
                switch (sequenceMode)
                {
                    case SequenceMode.Random:
                        _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        break;

                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        } while (_nextClipToPlay == _lastClipPlayed);
                        break;

                    case SequenceMode.Sequential:
                        _nextClipToPlay = (int)Mathf.Repeat(++_nextClipToPlay, audioClips.Length);
                        break;
                }
            }

            _lastClipPlayed = _nextClipToPlay;

            return audioClips[_nextClipToPlay];
        }

        public enum SequenceMode
        {
            Random, // 随机
            RandomNoImmediateRepeat, // 随机（连续两个不重复）
            Sequential, // 顺序
        }
    }
}