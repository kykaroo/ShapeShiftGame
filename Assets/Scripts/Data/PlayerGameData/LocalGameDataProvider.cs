using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerGameData
{
    public class LocalGameDataProvider : IDataProvider<PlayerGameData>
    {
        private const string FileName = "PlayerSave";
        private const string SaveFileExtension = ".json";

        private PlayerGameData _playerGameData;

        private static string SavePath => Application.persistentDataPath;
        private static string FullPath => Path.Combine(SavePath, $"{FileName}{SaveFileExtension}");
            
        public void Save()
        {
            File.WriteAllText(FullPath, JsonConvert.SerializeObject(_playerGameData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }
        
        public PlayerGameData GetData()
        {
            if (_playerGameData != null)
            {
                return _playerGameData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _playerGameData = new();
                return _playerGameData;
            }
            
            _playerGameData = JsonConvert.DeserializeObject<PlayerGameData>(File.ReadAllText(FullPath));
            return _playerGameData;
        }

        private static bool IsDataAlreadyExists() => File.Exists(FullPath);

        public void DeleteSave()
        {
            File.Delete(FullPath);
        }
    }
}