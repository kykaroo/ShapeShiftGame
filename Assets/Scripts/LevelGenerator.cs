using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float tileZSize;
    [SerializeField] private TileInfo[] levelTilesList;
    [SerializeField] private TileInfo startTile;
    
    private LinkedList<TileInfo> _generatedTiles;
    private Vector3 _currentPosition;

    private void Awake()
    {
        _generatedTiles = new();
    }

    private void Start()
    {
        _generatedTiles.AddFirst(Instantiate(startTile, transform.position, Quaternion.identity));
    }

    private void Update()
    {
        if (_generatedTiles.Last.Value.startPosition < player.position.z)
        {
            GenerateTile();
        }

        while (_generatedTiles.First.Value.endPosition + _generatedTiles.First.Next?.Value.tileZSize < player.position.z)
        {
            var value = _generatedTiles.First.Value;
            _generatedTiles.RemoveFirst();
            Destroy(value.gameObject);
        }

    }

    void GenerateTile()
    {
        var nextTile = levelTilesList[Random.Range(0, levelTilesList.Length)];
        var tileInfo = Instantiate(nextTile, Vector3.zero, Quaternion.identity);
        var tileInfoStartPosition = _generatedTiles.Last.Value.endPosition;
        _generatedTiles.AddLast(tileInfo);
        tileInfo.startPosition = tileInfoStartPosition;
    }
}