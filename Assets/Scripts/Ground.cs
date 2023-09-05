using System.Collections.Generic;
using UnityEngine;

public class Ground
{
    private LayerMask _allEnvironment;
    private LayerMask _waterMask;
    private LayerMask _balloonsMask;
    private LayerMask _stairsSlopeMask;
    private LayerMask _groundMask;

    public LayerMask AllEnvironment => _allEnvironment;

    public LayerMask WaterMask => _waterMask;

    public LayerMask BalloonsMask => _balloonsMask;

    public LayerMask StairsSlopeMask => _stairsSlopeMask;

    public LayerMask GroundMask => _groundMask;

    public Ground(LayerMask allEnvironment, LayerMask waterMask, LayerMask balloonsMask, LayerMask stairsSlopeMask, LayerMask groundMask)
    {
        _allEnvironment = allEnvironment;
        _waterMask = waterMask;
        _balloonsMask = balloonsMask;
        _stairsSlopeMask = stairsSlopeMask;
        _groundMask = groundMask;
    }

    public LinkedList<TileInfo> TileInfoList { get; } = new();

    public Vector3 GetMoveDirection(Vector3 origin)
    {
        return Physics.Raycast(origin, 
            Vector3.down, out var surfaceHit, 5f, _allEnvironment)
            ? Vector3.ProjectOnPlane(Vector3.forward, surfaceHit.normal).normalized
            : Vector3.forward;
    }

    public bool CanMoveTo(Bounds body, Vector3 from, Vector3 to, LayerMask mask)
    {
        return !Physics.BoxCast(body.center, body.extents, (to - from).normalized, Quaternion.identity, Vector3.Distance(from, to), mask);
    }
}