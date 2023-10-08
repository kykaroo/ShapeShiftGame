using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.PlayerOptionsData;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : ITickable
    {
        private readonly Sound[] _musicSounds;
        private readonly Sound[] _sfxSounds;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;
        private List<Sound> _musicPlaylist;
        private AudioClip _previousMusicClip;
        private Sound _musicToPlay;
        private readonly IDataProvider<PersistentPlayerOptionsData> _dataProvider;
        private readonly PersistentPlayerOptionsData _persistentPlayerData;
        private IObjectPool<AudioSource> _audioSourcesPool;
        private Transform _audioSourceParent;

        public float Interval = 1;
        public float Timer;

        public event Action<string> OnNewTrackPlay;

        [Inject]
        public AudioManager(PersistentPlayerOptionsData persistentPlayerData,
            IDataProvider<PersistentPlayerOptionsData> dataProvider, Sound[] musicSounds, Sound[] sfxSounds)
        {
            _dataProvider = dataProvider;
            _persistentPlayerData = persistentPlayerData;

            _musicSource = new GameObject("musicSource", typeof(AudioSource)).GetComponent<AudioSource>();
            _musicSounds = musicSounds;
            _musicSource.volume = _persistentPlayerData.MusicVolume;
            _musicSource.mute = _persistentPlayerData.MuteMusic;

            _sfxSource = new GameObject("sfxSource", typeof(AudioSource)).GetComponent<AudioSource>();
            _sfxSounds = sfxSounds;
            _sfxSource.volume = _persistentPlayerData.SfxVolume;
            _sfxSource.mute = _persistentPlayerData.MuteSfx;
            
            ResetPlaylist(_musicSounds);
            Timer = Interval;
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
            _persistentPlayerData.MusicVolume = newValue;
            _musicSource.volume = newValue;
        }

        public void ChangeSfxVolume(float newValue)
        {
            _persistentPlayerData.SfxVolume = newValue;
            _sfxSource.volume = newValue;
        }

        public void ToggleMusic()
        {
            if (_persistentPlayerData.MuteMusic)
            {
                _musicSource.mute = false;
                _persistentPlayerData.MuteMusic = false;
                return;
            }

            _musicSource.mute = true;
            _persistentPlayerData.MuteMusic = true;
        }

        public void ToggleSfx()
        {
            if (_persistentPlayerData.MuteSfx)
            {
                _sfxSource.mute = false;
                _persistentPlayerData.MuteSfx = false;
                return;
            }

            _sfxSource.mute = true;
            _persistentPlayerData.MuteSfx = true;
        }

        public void SaveSettings()
        {
            _dataProvider.Save();
        }

        public void Tick()
        {
            if (_musicSource.isPlaying) return;

            Timer += Time.deltaTime;

            if (!(Timer >= Interval)) return;
            
            OnNewTrackPlay?.Invoke(PlayAllMusic());
            Timer = 0;
        }
    }
}