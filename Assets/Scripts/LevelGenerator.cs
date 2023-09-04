using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float tileZSize;
    [SerializeField] private GameObject[] levelTilesList;
    [SerializeField] private GameObject startTile;
    
    private List<GameObject> _generatedTiles;
    private Vector3 _currentPosition;

    private void Awake()
    {
        _generatedTiles = new();
    }

    private void Start()
    {
        _generatedTiles.Add(Instantiate(startTile, transform.position, Quaternion.identity));
        transform.position += Vector3.forward * tileZSize;
    }

    private void Update()
    {
        if (transform.position.z - player.position.z < tileZSize * 2)
        {
            GenerateTile();
            transform.position += Vector3.forward * tileZSize;
        }

        foreach (var tile in _generatedTiles)
        {
            if (player.position.z - tile.transform.position.z > tileZSize)
            {
                _generatedTiles.Remove(tile);
                Destroy(tile);
                return;
            }
        }
    }

    void GenerateTile()
    {
        _generatedTiles.Add(Instantiate(levelTilesList[Random.Range(0, levelTilesList.Length)], transform.position, Quaternion.identity));
    }
}