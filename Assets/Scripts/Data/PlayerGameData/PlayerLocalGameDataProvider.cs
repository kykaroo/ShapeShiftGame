using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class PlayerLocalGameDataProvider : IDataProvider<PersistentPlayerGameData>
    {
        private const string FileName = "PlayerSave";
        private const string SaveFileExtension = ".json";

        private PersistentPlayerGameData _persistentPlayerGameData;

        private string SavePath => Application.persistentDataPath;
        private string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
            
        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_persistentPlayerGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
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
            
            _persistentPlayerGameData = JsonConvert.DeserializeObject<PersistentPlayerGameData>(File.ReadAllText(FullPath));
            return _persistentPlayerGameData;
        }

        private bool IsDataAlreadyExists() => File.Exists(FullPath);

        public void DeleteSave()
        {
            File.Delete(FullPath);
        }
    }
}