using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class PlayerLocalGameDataProvider : IDataProvider
    {
        private const string FileName = "PlayerSave";
        private const string SaveFileExtension = ".json";

        private readonly IPersistentPlayerData _persistentPlayerData;

        public PlayerLocalGameDataProvider(IPersistentPlayerData persistentPlayerData) => _persistentPlayerData = persistentPlayerData;

        private string SavePath => Application.persistentDataPath;
        private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
            
        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_persistentPlayerData.PlayerGameData, Formatting.Indented, new JsonSerializerSettings
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

            _persistentPlayerData.PlayerGameData = JsonConvert.DeserializeObject<PlayerGameData>(File.ReadAllText(FullPath));
            return true;
        }

        private bool IsDataAlreadyExists() => File.Exists(FullPath);

        public void DeleteSave()
        {
            File.Delete(FullPath);
        }
    }
}