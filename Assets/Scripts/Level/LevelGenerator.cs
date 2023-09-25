using System.Collections.Generic;
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
        private float _levelStartZ;
        private float _levelEndZ;

        private LinkedList<TileInfo> TileList => _ground.TileInfoList;
    
        private LinkedList<BackgroundInfo> BackgroundList => _ground.BackgroundTileList;

        public VictoryTrigger VictoryTrigger;

        public float LevelStartZ => _levelStartZ;

        public float LevelEndZ => _levelEndZ;

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
            GetLevelInfo();

            while (TileList.Last.Value.End.z > BackgroundList.Last.Value.EndPosition.z)
            {
                GenerateBackground();
            }
        }

        private void GetLevelInfo()
        {
            _levelEndZ = TileList.Last.Value.FinishTileVictoryTrigger.transform.position.z;
            _levelStartZ = 0;
        }

        private void GenerateFinishTile()
        {
            var tileInfoStartPosition = TileList.Last.Value.End;
            TileList.AddLast(Object.Instantiate(_levelConfig.FinishTile, Vector3.zero, Quaternion.identity));
            TileList.Last.Value.ConnectStartToCurrentEnd = tileInfoStartPosition;
            VictoryTrigger = TileList.Last.Value.FinishTileVictoryTrigger;
        }
    
        private void GenerateTile()
        {
            var nextTile = _levelConfig.LevelTilesList[Random.Range(0, _levelConfig.LevelTilesList.Length)];
            var tileInfo = Object.Instantiate(nextTile, Vector3.zero, Quaternion.identity);
            var tileInfoStartPosition = TileList.Last.Value.End;
            
            if (nextTile.waterEndRamp != null && TileList.Last.Value.waterEndRamp != null)
            {
                Object.Destroy(TileList.Last.Value.waterEndRamp);
            }
            
            TileList.AddLast(tileInfo);
            tileInfo.ConnectStartToCurrentEnd = tileInfoStartPosition;
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