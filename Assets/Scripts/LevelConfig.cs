using UnityEngine;

[CreateAssetMenu(menuName = "Level/LevelTiles", fileName = "Level Tiles Config", order = 0)]
public class LevelConfig : ScriptableObject
{
    [SerializeField] private TileInfo[] levelTilesList;
    [SerializeField] private TileInfo startTile;

    public TileInfo[] LevelTilesList => levelTilesList;

    public TileInfo StartTile => startTile;
}