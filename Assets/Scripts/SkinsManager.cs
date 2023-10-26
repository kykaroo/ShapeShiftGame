using Data.PlayerGameData;
using Shop;
using Zenject;

public class SkinsManager
{
    public SkinSelector Selector { get; private set; }

    public SkinUnlocker Unlocker { get; private set; }

    public OpenSkinsChecker OpenChecker { get; private set; }

    public SelectedSkinChecker SelectedChecker { get; private set; }

    [Inject]
    private SkinsManager(PlayerGameData playerGameData)
    {
        OpenChecker = new(playerGameData);
        SelectedChecker = new(playerGameData);
        Selector = new(playerGameData);
        Unlocker = new(playerGameData);
    }
}