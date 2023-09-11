﻿using System.Collections.Generic;
using PrefabInfo;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelGenerator
    {
    
        private readonly Transform _player;
        private readonly LevelConfig _levelConfig;
        private readonly Ground _ground;

        private LinkedList<TileInfo> TileList => _ground.TileInfoList;
    
        private LinkedList<BackgroundInfo> BackgroundList => _ground.BackgroundTileList;

        public VictoryTrigger VictoryTrigger;

        public LevelGenerator(LevelConfig levelConfig, Ground ground)
        {
            _levelConfig = levelConfig;
            _ground = ground;
        }

        public void GenerateLevel()
        {
            TileList.AddFirst(Object.Instantiate(_levelConfig.StartTile, Vector3.zero, Quaternion.identity));
            BackgroundList.AddFirst(Object.Instantiate(
                _levelConfig.BackgroundTileList[Random.Range(0, _levelConfig.BackgroundTileList.Length)],
                _levelConfig.BackgroundStartPosition, Quaternion.identity));

            var tilesToGenerate = _levelConfig.NumberOfTiles;
            while (tilesToGenerate > 0)
            {
                GenerateTile();
                tilesToGenerate--;
            }

            GenerateFinishTile();

            while (TileList.Last.Value.EndPosition > BackgroundList.Last.Value.EndPosition)
            {
                GenerateBackground();
            }
        }

        private void GenerateFinishTile()
        {
            var tileInfoStartPosition = TileList.Last.Value.EndPosition;
            TileList.AddLast(Object.Instantiate(_levelConfig.FinishTile, Vector3.zero, Quaternion.identity));
            TileList.Last.Value.StartPosition = tileInfoStartPosition;
            VictoryTrigger = TileList.Last.Value.FinishTileVictoryTrigger;
        }
    
        private void GenerateTile()
        {
            var nextTile = _levelConfig.LevelTilesList[Random.Range(0, _levelConfig.LevelTilesList.Length)];
            var tileInfo = Object.Instantiate(nextTile, Vector3.zero, Quaternion.identity);
            var tileInfoStartPosition = TileList.Last.Value.EndPosition;
            TileList.AddLast(tileInfo);
            tileInfo.StartPosition = tileInfoStartPosition;
        }

        public void ClearLevel()
        {
            while (TileList.First != null)
            {
                DestroyFirstElementInTilesList();
            }
        
            while (BackgroundList.First != null)
            {
                DestroyFirstElementInBackgroundList();
            }
        }

        private void DestroyFirstElementInBackgroundList()
        {
            var value = BackgroundList.First.Value;
            BackgroundList.RemoveFirst();
            Object.Destroy(value.gameObject);
        }

        private void DestroyFirstElementInTilesList()
        {
            var value = TileList.First.Value;
            TileList.RemoveFirst();
            Object.Destroy(value.gameObject);
        }
    
        private void GenerateBackground()
        {
            var nextTile = _levelConfig.BackgroundTileList[Random.Range(0, _levelConfig.BackgroundTileList.Length)];
            var backgroundInfo = Object.Instantiate(nextTile, Vector3.zero, Quaternion.identity);
            var tileInfoStartPosition = BackgroundList.Last.Value.EndPosition;
            BackgroundList.AddLast(backgroundInfo);
            backgroundInfo.StartPosition = tileInfoStartPosition;
        }
    }
}