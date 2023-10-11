using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class PlayerPrefsGameDataProvider : IDataProvider<PersistentGameData>
    {
        private const string FileName = "PlayerGameSave";

        private PersistentGameData _persistentGameData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_persistentGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        private static bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }

        public PersistentGameData GetData()
        {
            if (_persistentGameData != null)
            {
                return _persistentGameData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _persistentGameData = new();
                return _persistentGameData;
            }
            
            _persistentGameData = JsonConvert.DeserializeObject<PersistentGameData>(PlayerPrefs.GetString(FileName)); 
            return _persistentGameData;
        }
    }
}