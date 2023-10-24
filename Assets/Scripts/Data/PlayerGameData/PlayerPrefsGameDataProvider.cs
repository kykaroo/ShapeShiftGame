using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class PlayerPrefsGameDataProvider : IDataProvider<PlayerGameData>
    {
        private const string FileName = "PlayerGameSave";

        private PlayerGameData _playerGameData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_playerGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        private static bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }

        public PlayerGameData GetData()
        {
            if (_playerGameData != null)
            {
                return _playerGameData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _playerGameData = new();
                return _playerGameData;
            }
            
            _playerGameData = JsonConvert.DeserializeObject<PlayerGameData>(PlayerPrefs.GetString(FileName)); 
            return _playerGameData;
        }
    }
}