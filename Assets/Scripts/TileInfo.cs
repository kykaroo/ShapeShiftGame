using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [field:SerializeField] public float TileZSize { get; private set; }
    [SerializeField] private VictoryTrigger finishTileVictoryTrigger;

    public VictoryTrigger FinishTileVictoryTrigger => finishTileVictoryTrigger;

    public float StartPosition
    {
        get => transform.position.z - TileZSize * 0.5f;
        set => transform.position = Vector3.forward * (value + 0.5f * TileZSize);
    }

    public float EndPosition => transform.position.z + TileZSize * 0.5f;
}