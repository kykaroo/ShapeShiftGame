using System.Collections.Generic;
using PrefabInfo;
using UnityEngine;

public class Ground
{
    public LayerMask AllEnvironment { get; }

    public LayerMask WaterMask { get; }

    public LayerMask BalloonsMask { get; }

    public LayerMask StairsSlopeMask { get; }

    public LayerMask GroundMask { get; }

    public LinkedList<TileInfo> TileInfoList { get; } = new();
    
    public LinkedList<BackgroundInfo> BackgroundTileList { get; } = new();

    public Ground(LayerMask allEnvironment, LayerMask waterMask, LayerMask balloonsMask, LayerMask stairsSlopeMask, 
        LayerMask groundMask)
    {
        AllEnvironment = allEnvironment;
        WaterMask = waterMask;
        BalloonsMask = balloonsMask;
        StairsSlopeMask = stairsSlopeMask;
        GroundMask = groundMask;
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

    public LayerMask GetTerrainType(Bounds bounds)
    {
        Physics.Raycast(bounds.center + Vector3.forward * bounds.extents.z, bounds.center + Vector3.forward * bounds.extents.z + Vector3.down, out var hit, bounds.extents.x + 0.01f);
        return hit.collider.gameObject.layer;
    }
}