using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class LocalGameDataProvider : IDataProvider<PersistentGameData>
    {
        private const string FileName = "PlayerSave";
        private const string SaveFileExtension = ".json";

        private PersistentGameData _persistentGameData;

        private static string SavePath => Application.persistentDataPath;
        private static string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
            
        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_persistentGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
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
            
            _persistentGameData = JsonConvert.DeserializeObject<PersistentGameData>(File.ReadAllText(FullPath));
            return _persistentGameData;
        }

        private static bool IsDataAlreadyExists() => File.Exists(FullPath);

        public void DeleteSave()
        {
            File.Delete(FullPath);
        }
    }
}