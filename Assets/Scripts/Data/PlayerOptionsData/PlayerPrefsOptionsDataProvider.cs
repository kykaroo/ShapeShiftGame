using Newtonsoft.Json;
using UnityEngine;

namespace Data.PlayerOptionsData
{
    public class PlayerPrefsOptionsDataProvider :  IDataProvider<PersistentPlayerOptionsData>
    {
        private const string FileName = "PlayerOptionsSave";

        private PersistentPlayerOptionsData _persistentPlayerOptionsData;

        public void Save()
        {
            PlayerPrefs.SetString(FileName, JsonConvert.SerializeObject(_persistentPlayerOptionsData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        public PersistentPlayerOptionsData GetData()
        {
            if (_persistentPlayerOptionsData != null)
            {
                return _persistentPlayerOptionsData;
            }
            
            if (IsDataAlreadyExists() == false)
            {
                _persistentPlayerOptionsData = new();
                return _persistentPlayerOptionsData;
            }
            
            _persistentPlayerOptionsData = JsonConvert.DeserializeObject<PersistentPlayerOptionsData>(PlayerPrefs.GetString(FileName));
            return _persistentPlayerOptionsData;
        }

        private bool IsDataAlreadyExists() => PlayerPrefs.HasKey(FileName);

        public void DeleteSave()
        {
            PlayerPrefs.DeleteKey(FileName);
        }
    }
}