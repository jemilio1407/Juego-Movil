using System;
using System.Collections.Generic;
using Dino.Utility.Audio;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using AudioType = Dino.Utility.Audio.AudioType;

namespace Dino.Utility.Audio
{
    /// <summary>
    /// Last update 27/04/2025 Dino
    /// A class that allows you to manage audio.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        public static AudioManager Instance { get; private set; }
        #endregion
        
        #region serialized fields

        
        
        [Header("Audio Manager Data")] 
        [SerializeField]
        private AudioManagerData _audioManagerData;

        
        [Header("Audio Test")]
        public string _soundNameTest;

        #endregion

        #region private fields

        private GameObject _soundsContainer;
        private readonly Dictionary<string, AudioData> _audioDataLookup = new Dictionary<string, AudioData>();
        private readonly Dictionary<AudioClip, AudioSource> _audioSourceLookup = new Dictionary<AudioClip, AudioSource>();
        private bool _isInitialized;

        #endregion

        public AudioManagerData AudioManagerData
        {
            get => _audioManagerData;
            set => _audioManagerData = value;
        }


        #region unity methods

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        #endregion

        #region private methods

        private void Initialize()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            _soundsContainer = new GameObject("SoundsContainer");
            _soundsContainer.transform.SetParent(transform);

            _audioDataLookup.Clear();
            _audioSourceLookup.Clear();

            if (_audioManagerData != null && _audioManagerData.audioData != null)
            {
                for (int i = 0; i < _audioManagerData.audioData.Count; i++)
                {
                    AudioData data = _audioManagerData.audioData[i];
                    if (data != null && !string.IsNullOrEmpty(data.name) && !_audioDataLookup.ContainsKey(data.name))
                    {
                        _audioDataLookup.Add(data.name, data);
                    }
                }
            }
        }

        private AudioData GetAudioData(string soundName)
        {
            if (!_isInitialized) Initialize();

            if (string.IsNullOrEmpty(soundName)) return null;
            if (_audioDataLookup.TryGetValue(soundName, out AudioData cached)) return cached;

            if (_audioManagerData != null && _audioManagerData.audioData != null)
            {
                for (int i = 0; i < _audioManagerData.audioData.Count; i++)
                {
                    AudioData data = _audioManagerData.audioData[i];
                    if (data != null && data.name == soundName)
                    {
                        _audioDataLookup[soundName] = data;
                        return data;
                    }
                }
            }

            return null;
        }

        private void SetAudioInMixerGroup(AudioSource audioSource, AudioType audioType)
        {
            switch (audioType)
            {
                case AudioType.Music:
                    break;
                case AudioType.SFX:
                    audioSource.outputAudioMixerGroup = _audioManagerData.SfxAudioMixerGroup;
                    break;
                case AudioType.Ambience:
                    break;
            }
        }
        
        #endregion


        #region public methdos

        public void PlaySound(string soundName)
        {
            AudioData audioData = GetAudioData(soundName);
            if (audioData == null || audioData.clip == null)
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }

            if (!_audioSourceLookup.TryGetValue(audioData.clip, out AudioSource audioSource) || audioSource == null)
            {
                GameObject go = new GameObject("AS : " + audioData.name);
                audioSource = go.AddComponent<AudioSource>();
                audioSource.transform.SetParent(_soundsContainer.transform);
                audioSource.clip = audioData.clip;
                audioSource.loop = audioData.loop;
                audioSource.volume = audioData.volume;
                audioSource.pitch = audioData.pitch;
                audioSource.Play();
                SetAudioInMixerGroup(audioSource, audioData.audioType);
                _audioSourceLookup[audioData.clip] = audioSource;
            }
            else
            {
                audioSource.Play();
            }
        }

        public void StopSound(string soundName)
        {
            AudioData audioData = GetAudioData(soundName);
            if (audioData == null || audioData.clip == null)
            {
                Debug.LogWarning("Sound: " + soundName + " not found!");
                return;
            }

            if (_audioSourceLookup.TryGetValue(audioData.clip, out AudioSource audioSource) && audioSource != null)
            {
                audioSource.Stop();
            }
        }
        
        public void UpdateAudioMixerGroupVolume(float value, AudioType audioType)
        {
            float volume = value;

            switch (audioType)
            {
                case AudioType.Music:
                    _audioManagerData.MusicAudioMixerGroup.audioMixer.SetFloat(_audioManagerData.MusicVolumeParameterName, Mathf.Lerp(-80f, 20f, volume));
                    break;
                case AudioType.SFX:
                    _audioManagerData.SfxAudioMixerGroup.audioMixer.SetFloat(_audioManagerData.SfxVolumeParameterName, Mathf.Lerp(-80f, 20f, volume));
                    break;
                case AudioType.Ambience:
                    _audioManagerData.AmbienceAudioMixerGroup.audioMixer.SetFloat(_audioManagerData.AmbienceVolumeParameterName, Mathf.Lerp(-80f, 20f, volume));
                    break;
                
            }
        }

        #endregion
        
        [Button]
        void TestSound()
        {
            PlaySound(_soundNameTest);
            Debug.Log("Playing sound: " + _soundNameTest);
            
        }
    }
}

namespace Dino.Utility.Audio
{
    public enum AudioType
    {
        Music,
        SFX,
        Ambience
    }
}