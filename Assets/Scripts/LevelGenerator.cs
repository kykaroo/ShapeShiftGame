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
    }

    public void Update()
    {
        if (TileList.Last.Value.StartPosition - TileList.Last.Value.TileZSize * TilesAhead < _player.position.z)
        {
            GenerateTile();
        }

        if (_player.position.z > _currentTerrain.Last.Value.transform.position.z + 350f)
        {
            var position = _currentTerrain.Last.Value.transform.position;
            Vector3 terrainPos = new(position.x, position.y, position.z + 500);
            _currentTerrain.AddLast(Object.Instantiate(TerrainList[0], terrainPos, Quaternion.identity));
        }

        while (TileList.First.Value.EndPosition + TileList.First.Next?.Value.TileZSize < _player.position.z)
        {
            DestroyFirstElementInTilesList();
        }

        while (_currentTerrain.First.Value.transform.position.z + 600f < _player.position.z)
        {
            DestroyFirstElementInTerrainList();
        }
    }

    private void DestroyFirstElementInTerrainList()
    {
        var value = _currentTerrain.First.Value;
        _currentTerrain.RemoveFirst();
        Object.Destroy(value.gameObject);
    }

    private void DestroyFirstElementInTilesList()
    {
        var value = TileList.First.Value;
        TileList.RemoveFirst();
        Object.Destroy(value.gameObject);
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
}