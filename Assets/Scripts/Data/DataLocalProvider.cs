using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    public class DataLocalProvider : IDataProvider
    {
        private const string FileName = "PlayerSave";
        private const string SaveFileExtension = ".json";

        private IPersistentData _persistentData;

        public DataLocalProvider(IPersistentData persistentData) => _persistentData = persistentData;

        private string SavePath => Application.persistentDataPath;
        private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
            
        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_persistentData.PlayerData, Formatting.Indented, new JsonSerializerSettings
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

            _persistentData.PlayerData = JsonConvert.DeserializeObject<PlayerData>(File.ReadAllText(FullPath));
            return true;
        }

        private bool IsDataAlreadyExists() => File.Exists(FullPath);

        public void DeleteSave()
        {
            File.Delete(FullPath);
        }
    }
}