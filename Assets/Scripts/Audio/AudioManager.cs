using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.PlayerOptionsData;
using FormStateMachine;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;
using Random = UnityEngine.Random;

namespace Audio
{
    public class AudioManager : ITickable
    {
        private readonly Sound[] _musicSounds;
        private readonly AudioClip[] _humanSounds;
        private readonly AudioClip[] _carSounds;
        private readonly AudioClip[] _helicopterSounds;
        private readonly AudioClip[] _boatSounds;
        private readonly AudioSource _musicSource;
        private readonly AudioSource _sfxSource;
        private List<Sound> _musicPlaylist;
        private AudioClip _previousMusicClip;
        private Sound _musicToPlay;
        private readonly IDataProvider<PersistentPlayerOptionsData> _dataProvider;
        private readonly PersistentPlayerOptionsData _persistentPlayerData;
        private IObjectPool<AudioSource> _audioSourcesPool;
        private Transform _audioSourceParent;
        private readonly Player _player;
        private IFormState _currentPlayerForm;

        private const float Interval = 1;
        private float _timer;

        public event Action<string> OnNewTrackPlay;

        [Inject]
        public AudioManager(PersistentPlayerOptionsData persistentPlayerData, IDataProvider<PersistentPlayerOptionsData> dataProvider, 
            Sound[] musicSounds, AudioClip[] humanSounds, AudioClip[] helicopterSounds, AudioClip[] carSounds, AudioClip[] boatSounds, Player player)
        {
            _dataProvider = dataProvider;
            _persistentPlayerData = persistentPlayerData;
            _player = player;

            _musicSource = new GameObject("musicSource", typeof(AudioSource)).GetComponent<AudioSource>();
            _musicSounds = musicSounds;
            _musicSource.volume = _persistentPlayerData.MusicVolume / 2;
            _musicSource.mute = _persistentPlayerData.MuteMusic;

            _sfxSource = new GameObject("sfxSource", typeof(AudioSource)).GetComponent<AudioSource>();
            _sfxSource.volume = _persistentPlayerData.SfxVolume / 2;
            _sfxSource.mute = _persistentPlayerData.MuteSfx;
            
            _humanSounds = humanSounds;
            _carSounds = carSounds;
            _helicopterSounds = helicopterSounds;
            _boatSounds = boatSounds;
            
            ResetPlaylist(_musicSounds);
            _timer = Interval;
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

        private void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        private void PlayRandomSound(AudioClip[] sounds, float playSpeed)
        {
            _sfxSource.clip = sounds[Random.Range(0, sounds.Length)];
            _sfxSource.pitch = playSpeed;
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
            
            PlayMusic(clipToPlay);

            _musicPlaylist.Remove(_musicToPlay);
            return _musicToPlay.name;
        }

        public void ChangeMusicVolume(float newValue)
        {
            _persistentPlayerData.MusicVolume = newValue;
            _musicSource.volume = newValue / 2;
        }

        public void ChangeSfxVolume(float newValue)
        {
            _persistentPlayerData.SfxVolume = newValue;
            _sfxSource.volume = newValue / 2;
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
            if (!_sfxSource.isPlaying || _currentPlayerForm != _player.GetCurrentState())
            {
                _currentPlayerForm = _player.GetCurrentState();
                PlayStateSfx(_currentPlayerForm);
                
            }
            
            if (_musicSource.isPlaying) return;

            _timer += Time.deltaTime;

            if (!(_timer >= Interval)) return;
            
            OnNewTrackPlay?.Invoke(PlayAllMusic());
            _timer = 0;
        }

        private void PlayStateSfx(IFormState currentForm)
        {
            switch (currentForm)
            {
                case FormStateMachine.States.HumanFormState:
                    PlayRandomSound(_humanSounds, 1.5f);
                    break;
                case FormStateMachine.States.CarFormState:
                    PlayRandomSound(_carSounds, 1);
                    break;
                case FormStateMachine.States.HelicopterFormState:
                    PlayRandomSound(_helicopterSounds, 1);
                    break;
                case FormStateMachine.States.BoatFormState:
                    PlayRandomSound(_boatSounds, 1);
                    break;
            }
        }
    }
}