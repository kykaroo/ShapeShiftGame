using System;
using Newtonsoft.Json;
using YG;
using Zenject;

namespace Data.PlayerGameData
{
    public class YgGameDataProvider : IDataProvider<PlayerGameData>
    {
        private PlayerGameData _playerGameData;
        private YandexGame _yandexGame;

        [Inject]
        public YgGameDataProvider(YandexGame yandexGame)
        {
            _yandexGame = yandexGame;
        }

        public void Save()
        {
            YandexGame.savesData.PlayerGameData = JsonConvert.SerializeObject(_playerGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            _yandexGame._SaveProgress();
        }

        public void DeleteSave()
        {
            YandexGame.savesData.PlayerGameData = null;
            _yandexGame._SaveProgress();
        }

        public PlayerGameData GetData()
        {
            _yandexGame._LoadProgress();
            
            if (_playerGameData != null)
            {
                return _playerGameData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _playerGameData = new();
                return _playerGameData;
            }
            
            _playerGameData = JsonConvert.DeserializeObject<PlayerGameData>(YandexGame.savesData.PlayerGameData); 
            return _playerGameData;
        }
        
        private bool IsDataAlreadyExists()
        {
            return !string.IsNullOrEmpty(YandexGame.savesData.PlayerGameData) && YandexGame.savesData.PlayerGameData != "null";
        }
    }
}