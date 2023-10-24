using Newtonsoft.Json;
using YG;
using Zenject;

namespace Data.PlayerOptionsData
{
    
    public class YgOptionsDataProvider : IDataProvider<PlayerOptionsData>
    {
        private PlayerOptionsData _playerOptionsData;
        private YandexGame _yandexGame;

        [Inject]
        public YgOptionsDataProvider(YandexGame yandexGame)
        {
            _yandexGame = yandexGame;
        }

        public void Save()
        {
            YandexGame.savesData.PlayerOptionsData = JsonConvert.SerializeObject(_playerOptionsData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            
            _yandexGame._SaveProgress();
        }

        public void DeleteSave()
        {
            YandexGame.savesData.PlayerOptionsData = null;
            _yandexGame._SaveProgress();
        }

        public PlayerOptionsData GetData()
        {
            _yandexGame._LoadProgress();
            
            if (_playerOptionsData != null)
            {
                return _playerOptionsData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _playerOptionsData = new();
                return _playerOptionsData;
            }

            _playerOptionsData = JsonConvert.DeserializeObject<PlayerOptionsData>(YandexGame.savesData.PlayerOptionsData); 
            return _playerOptionsData;
        }

        private bool IsDataAlreadyExists()
        {
            return !string.IsNullOrEmpty(YandexGame.savesData.PlayerOptionsData) && YandexGame.savesData.PlayerGameData != "null";
        }
    }
}