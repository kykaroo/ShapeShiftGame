using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerOptionsData
{
    public class PlayerPrefsOptionsDataProvider : IDataProvider
    {
        private const string FileName = "PlayerOptionsSave";

        private readonly IPersistentPlayerData _persistentPlayerData;
        
        public PlayerPrefsOptionsDataProvider(IPersistentPlayerData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;
        
        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_persistentPlayerData.PlayerOptionsData, Formatting.Indented, new JsonSerializerSettings
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
            
            _persistentPlayerData.PlayerOptionsData = JsonConvert.DeserializeObject<PlayerOptionsData>(PlayerPrefs.GetString(FileName));
            return true;
        }

        private bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }
    }
}