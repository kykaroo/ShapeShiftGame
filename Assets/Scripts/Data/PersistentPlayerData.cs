namespace Data
{
    public class PersistentPlayerData : IPersistentPlayerData
    {
        public PlayerGameData.PlayerGameData PlayerGameData { get; set; }
        public PlayerOptionsData.PlayerOptionsData PlayerOptionsData { get; set; }
    }
}