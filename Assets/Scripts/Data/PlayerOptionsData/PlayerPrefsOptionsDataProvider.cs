using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerOptionsData
{
    public class PlayerPrefsOptionsDataProvider :  IDataProvider<PlayerOptionsData>
    {
        private const string FileName = "PlayerOptionsSave";

        private PlayerOptionsData _playerOptionsData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_playerOptionsData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public PlayerOptionsData GetData()
        {
            if (_playerOptionsData != null)
            {
                return _playerOptionsData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _playerOptionsData = new();
                return _playerOptionsData;
            }
            
            _playerOptionsData = JsonConvert.DeserializeObject<PlayerOptionsData>(PlayerPrefs.GetString(FileName));
            return _playerOptionsData;
        }

        private static bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }
    }
}