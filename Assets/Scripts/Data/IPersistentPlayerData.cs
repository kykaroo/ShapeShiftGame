namespace Data
{
    public interface IPersistentPlayerData
    {
        PlayerGameData.PlayerGameData PlayerGameData { get; set; }
        PlayerOptionsData.PlayerOptionsData PlayerOptionsData { get; set; }
    }
}