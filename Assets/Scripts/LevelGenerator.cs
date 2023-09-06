using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator
{
    private readonly Transform _player;
    private readonly LevelConfig _levelConfig;
    private readonly Ground _ground;
    private const int TilesAhead = 1;

    private LinkedList<TileInfo> TileList => _ground.TileInfoList;
    
    private List<Terrain> TerrainList => _ground.TerrainPrefabs;

    private LinkedList<Terrain> _currentTerrain;

    public LevelGenerator(Transform player, LevelConfig levelConfig, Ground ground)
    {
        _player = player;
        _levelConfig = levelConfig;
        _ground = ground;
    }

    public void Start()
    {
        _currentTerrain = new();
        TileList.AddFirst(Object.Instantiate(_levelConfig.StartTile, Vector3.zero, Quaternion.identity));
        _currentTerrain.AddFirst(Object.Instantiate(TerrainList[0], _ground.TerrainStartPosition, Quaternion.identity));
        
        var tilesToGenerate = _levelConfig.NumberOfTiles;
        while (tilesToGenerate > 0)
        {
            GenerateTile();
            tilesToGenerate--;
        }

        GenerateFinishTile();
    }

    private void GenerateFinishTile()
    {
        var tileInfoStartPosition = TileList.Last.Value.EndPosition;
        TileList.AddLast(Object.Instantiate(_levelConfig.FinishTile, Vector3.zero, Quaternion.identity));
        TileList.Last.Value.StartPosition = tileInfoStartPosition;
    }
    
    private void GenerateTile()
    {
        var nextTile = _levelConfig.LevelTilesList[Random.Range(0, _levelConfig.LevelTilesList.Length)];
        var tileInfo = Object.Instantiate(nextTile, Vector3.zero, Quaternion.identity);
        var tileInfoStartPosition = TileList.Last.Value.EndPosition;
        TileList.AddLast(tileInfo);
        tileInfo.StartPosition = tileInfoStartPosition;
    }

    public void Stop()
    {
        while (TileList.First != null)
        {
            DestroyFirstElementInTilesList();
        }
    }
    
    private void DestroyFirstElementInTilesList()
    {
        var value = TileList.First.Value;
        TileList.RemoveFirst();
        Object.Destroy(value.gameObject);
    }
}