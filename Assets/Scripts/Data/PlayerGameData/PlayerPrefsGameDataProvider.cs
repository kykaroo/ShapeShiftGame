using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class PlayerPrefsGameDataProvider : IDataProvider<PersistentPlayerGameData>
    {
        private const string FileName = "PlayerGameSave";

        private PersistentPlayerGameData _persistentPlayerGameData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_persistentPlayerGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        private bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }

        public PersistentPlayerGameData GetData()
        {
            if (_persistentPlayerGameData != null)
            {
                return _persistentPlayerGameData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _persistentPlayerGameData = new();
                return _persistentPlayerGameData;
            }
            
            _persistentPlayerGameData = JsonConvert.DeserializeObject<PersistentPlayerGameData>(PlayerPrefs.GetString(FileName)); 
            return _persistentPlayerGameData;
        }
    }
}