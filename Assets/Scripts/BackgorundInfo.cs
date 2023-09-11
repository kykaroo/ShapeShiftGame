using UnityEngine;

public class BackgroundInfo : MonoBehaviour
{
    [field: SerializeField] public float BackgroundZSize { get; private set; }

    public float StartPosition
    {
        get => transform.position.z;
        set => transform.position = Vector3.forward * (value + BackgroundZSize);
    }

    public float EndPosition => transform.position.z + BackgroundZSize;
}