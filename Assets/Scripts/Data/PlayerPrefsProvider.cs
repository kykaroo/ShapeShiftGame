using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class PlayerPrefsProvider : IDataProvider
    {
        private const string FileName = "PlayerSave";

        private readonly IPersistentData _persistentData;

        public PlayerPrefsProvider(IPersistentData persistentData) => _persistentData = persistentData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_persistentData.PlayerData, Formatting.Indented, new JsonSerializerSettings
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
            
            _persistentData.PlayerData = JsonConvert.DeserializeObject<PlayerData>(PlayerPrefs.GetString(FileName));
            return true;
        }

        private bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }
    }
}