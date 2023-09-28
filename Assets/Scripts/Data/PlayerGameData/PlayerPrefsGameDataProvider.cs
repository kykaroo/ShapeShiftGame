using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class PlayerPrefsGameDataProvider : IDataProvider
    {
        private const string FileName = "PlayerGameSave";

        private readonly IPersistentPlayerData _persistentPlayerData;

        public PlayerPrefsGameDataProvider(IPersistentPlayerData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_persistentPlayerData.PlayerGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public bool TryLoad()
        {
            if (IsDataAlreadyExists() == false)
            {
                return false;
            }
            
            _persistentPlayerData.PlayerGameData = JsonConvert.DeserializeObject<PlayerGameData>(PlayerPrefs.GetString(FileName));
            return true;
        }

        private bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }
    }
}