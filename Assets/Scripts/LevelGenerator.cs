using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator
{
    private readonly Transform _player;
    private readonly LevelConfig _levelConfig;
    private readonly Ground _ground; 
    
    private LinkedList<TileInfo> TileList => _ground.TileInfoList;

    public LevelGenerator(Transform player, LevelConfig levelConfig, Ground ground)
    {
        _player = player;
        _levelConfig = levelConfig;
        _ground = ground;
    }

    public void Start()
    {
        TileList.AddFirst(Object.Instantiate(_levelConfig.StartTile, Vector3.zero, Quaternion.identity));
    }

    public void Update()
    {
        if (TileList.Last.Value.startPosition < _player.position.z)
        {
            GenerateTile();
        }

        while (TileList.First.Value.endPosition + TileList.First.Next?.Value.tileZSize < _player.position.z)
        {
            DestroyFirstElement();
        }
    }

    private void DestroyFirstElement()
    {
        var value = TileList.First.Value;
        TileList.RemoveFirst();
        Object.Destroy(value.gameObject);
    }

    private void GenerateTile()
    {
        var nextTile = _levelConfig.LevelTilesList[Random.Range(0, _levelConfig.LevelTilesList.Length)];
        var tileInfo = Object.Instantiate(nextTile, Vector3.zero, Quaternion.identity);
        var tileInfoStartPosition = TileList.Last.Value.endPosition;
        TileList.AddLast(tileInfo);
        tileInfo.startPosition = tileInfoStartPosition;
    }

    public void Stop()
    {
        while (TileList.First != null)
        {
            DestroyFirstElement();
        }
    }
}