using UnityEngine;

public class TileInfo : MonoBehaviour
{
    [field:SerializeField] public float tileZSize { get; private set; }
    [field:SerializeField] public float minY { get; private set; }
    [field:SerializeField] public float maxY { get; private set; }
    
    public float startPosition
    {
        get => transform.position.z - tileZSize * 0.5f;
        set => transform.position = Vector3.forward * (value + 0.5f * tileZSize);
    }

    public float endPosition => transform.position.z + tileZSize * 0.5f;
}