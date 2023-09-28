using System;
using Newtonsoft.Json;

namespace Data.PlayerOptionsData
{
    public class PlayerOptionsData
    {
        private float _musicVolume;
        private float _sfxVolume;

        public PlayerOptionsData()
        {
            _musicVolume = 0.5f;
            _sfxVolume = 0.5f;
            MuteMusic = false;
            MuteSfx = false;
        }
        
        [JsonConstructor]
        public PlayerOptionsData(float musicVolume, float sfxVolume, bool isMusicMute, bool isSfxMute)
        {
            _musicVolume = musicVolume;
            _sfxVolume = sfxVolume;
            MuteMusic = isMusicMute;
            MuteSfx = isSfxMute;
        }
        
        public bool MuteMusic { get; set; }

        public bool MuteSfx { get; set; }

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                if (value is < 0 or > 1)
                {
                    throw new ArgumentException();
                }

                _musicVolume = value;
            }
        }
        
        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                if (value is < 0 or > 1)
                {
                    throw new ArgumentException();
                }

                _sfxVolume = value;
            }
        }
    }
}