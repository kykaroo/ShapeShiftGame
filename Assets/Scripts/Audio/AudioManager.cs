using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
            
        public AudioManager(Sound[] musicSounds, Sound[] sfxSounds, AudioSource musicSource, AudioSource sfxSource)
        {
            _musicSounds = musicSounds;
            _sfxSounds = sfxSounds;
            _musicSource = musicSource;
            _sfxSource = sfxSource;
            ResetPlaylist(_musicSounds);
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

        public bool IsMusicPlaying()
        {
            return _musicSource.isPlaying;
        }
    }
}