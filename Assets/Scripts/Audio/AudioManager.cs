using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.PlayerOptionsData;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager
    {
        private readonly Sound[] _musicSounds;
        private readonly Sound[] _sfxSounds;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;
        private List<Sound> _musicPlaylist;
        private AudioClip _previousMusicClip;
        private Sound _musicToPlay;
        private readonly IDataProvider _dataProvider;
        private readonly IPersistentPlayerData _persistentPlayerData;
        private IObjectPool<AudioSource> _audioSourcesPool;
        private Transform _audioSourceParent;

        public float Interval = 1;
        public float Timer;

        public event Action<string> OnNewTrackPlay;

        public AudioManager(IPersistentPlayerData persistentPlayerData, Sound[] musicSounds, Sound[] sfxSounds, AudioSource musicSource, AudioSource sfxSource)
        {
            _dataProvider = new PlayerPrefsOptionsDataProvider(persistentPlayerData);
            _persistentPlayerData = persistentPlayerData;
            LoadData();
            
            _musicSounds = musicSounds;
            _sfxSounds = sfxSounds;
            _musicSource = musicSource;
            _sfxSource = sfxSource;
            _musicSource.volume = _persistentPlayerData.PlayerOptionsData.MusicVolume;
            _sfxSource.volume = _persistentPlayerData.PlayerOptionsData.SfxVolume;
            ResetPlaylist(_musicSounds);
            Timer = Interval;
        }

        private void LoadData()
        {
            if (_dataProvider.TryLoad() == false)
            {
                _persistentPlayerData.PlayerOptionsData = new();
            }
        }

        private void ResetPlaylist(IEnumerable<Sound> musicSounds)
        {
            _musicPlaylist = musicSounds.ToList();
        }

        public void PlayMusic(string name)
        {
            var s = Array.Find(_musicSounds, x => x.name == name);
            _musicSource.clip = s.clip;
            _musicSource.Play();
        }
        
        public void PlaySfx(string name)
        {
            var s = Array.Find(_sfxSounds, x => x.name == name);
            _sfxSource.clip = s.clip;
            _sfxSource.Play();
        }

        public string PlayAllMusic()
        {
            if (_musicPlaylist.Count == 0)
            {
                ResetPlaylist(_musicSounds);
            }

            AudioClip clipToPlay;

            do
            {
                _musicToPlay = _musicPlaylist[Random.Range(0, _musicPlaylist.Count)];
                clipToPlay = _musicToPlay.clip;
            } while (clipToPlay == _previousMusicClip);
            
            _previousMusicClip = clipToPlay;
            _musicSource.clip = clipToPlay;
            _musicSource.Play();

            _musicPlaylist.Remove(_musicToPlay);
            return _musicToPlay.name;
        }

        public void ChangeMusicVolume(float newValue)
        {
            _persistentPlayerData.PlayerOptionsData.MusicVolume = newValue;
            _musicSource.volume = newValue;
        }

        public void ChangeSfxVolume(float newValue)
        {
            _persistentPlayerData.PlayerOptionsData.SfxVolume = newValue;
            _sfxSource.volume = newValue;
        }

        public void MuteMusic()
        {
            if (_persistentPlayerData.PlayerOptionsData.MuteMusic)
            {
                _musicSource.mute = false;
                _persistentPlayerData.PlayerOptionsData.MuteMusic = false;
                return;
            }

            _musicSource.mute = true;
            _persistentPlayerData.PlayerOptionsData.MuteMusic = true;
        }

        public void MuteSfx()
        {
            if (_persistentPlayerData.PlayerOptionsData.MuteSfx)
            {
                _sfxSource.mute = false;
                _persistentPlayerData.PlayerOptionsData.MuteSfx = false;
                return;
            }

            _sfxSource.mute = true;
            _persistentPlayerData.PlayerOptionsData.MuteSfx = true;
        }

        public void SaveSettings()
        {
            _dataProvider.Save();
        }

        public void Update()
        {
            if (_musicSource.isPlaying) return;

            Timer += Time.deltaTime;

            if (!(Timer >= Interval)) return;
            
            OnNewTrackPlay?.Invoke(PlayAllMusic());
            Timer = 0;
        }
    }
}