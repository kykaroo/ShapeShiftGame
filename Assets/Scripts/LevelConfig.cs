using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelTiles", fileName = "Level Tiles Config", order = 0)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private TileInfo[] levelTilesList;
    [SerializeField] private TileInfo startTile;
    [SerializeField] private TileInfo finishTile;
    [SerializeField] private int numberOfTiles;
    [SerializeField] private BackgroundInfo[] backgroundTileList;
    [SerializeField] private Vector3 backgroundStartPosition;

    public TileInfo[] LevelTilesList => levelTilesList;

    public TileInfo StartTile => startTile;

    public TileInfo FinishTile => finishTile;

    public int NumberOfTiles => numberOfTiles;
    
    public BackgroundInfo[] BackgroundTileList => backgroundTileList;

    public Vector3 BackgroundStartPosition => backgroundStartPosition;
}