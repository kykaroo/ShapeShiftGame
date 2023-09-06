using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelTiles", fileName = "Level Tiles Config", order = 0)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private TileInfo[] levelTilesList;
    [SerializeField] private TileInfo startTile;
    [SerializeField] private TileInfo finishTile;
    [SerializeField] private int numberOfTiles;

    public TileInfo[] LevelTilesList => levelTilesList;

    public TileInfo StartTile => startTile;

    public TileInfo FinishTile => finishTile;

    public int NumberOfTiles => numberOfTiles;
}