using System.Collections.Generic;
using UnityEngine;

public class Ground
{
    public LayerMask AllEnvironment { get; }

    public LayerMask WaterMask { get; }

    public LayerMask BalloonsMask { get; }

    public LayerMask StairsSlopeMask { get; }

    public LayerMask GroundMask { get; }

    public Vector3 TerrainStartPosition { get; }
    
    public float TerrainLenght { get; }
    
    public List<Terrain> TerrainPrefabs { get; }
    
    public LinkedList<TileInfo> TileInfoList { get; } = new();

    public Ground(LayerMask allEnvironment, LayerMask waterMask, LayerMask balloonsMask, LayerMask stairsSlopeMask, 
        LayerMask groundMask, Vector3 terrainStartPosition, float terrainLenght, List<Terrain> terrainPrefabs)
    {
        AllEnvironment = allEnvironment;
        WaterMask = waterMask;
        BalloonsMask = balloonsMask;
        StairsSlopeMask = stairsSlopeMask;
        GroundMask = groundMask;
        TerrainStartPosition = terrainStartPosition;
        TerrainLenght = terrainLenght;
        TerrainPrefabs = terrainPrefabs;
    }


    public Vector3 GetMoveDirection(Vector3 origin)
    {
        return Physics.Raycast(origin, 
            Vector3.down, out var surfaceHit, 5f, AllEnvironment)
            ? Vector3.ProjectOnPlane(Vector3.forward, surfaceHit.normal).normalized
            : Vector3.forward;
    }

    public bool SurfaceCollision(Bounds bounds, Quaternion playerRotation, LayerMask maskToCheck)
    {
        var surfaceCollision = Physics.CheckBox(bounds.center, bounds.extents * 1.01f, playerRotation, maskToCheck);
        return surfaceCollision;
    }

    

    public bool VerticalObstacleCheck(Bounds bounds, Vector3 transformForward, LayerMask maskToCheck)
    {
        return Physics.Raycast(bounds.center, transformForward, bounds.extents.x + 0.01f, maskToCheck);
    }
}