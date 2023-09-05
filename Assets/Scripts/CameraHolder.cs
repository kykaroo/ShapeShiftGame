using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Update()
    {
        transform.localEulerAngles = -playerTransform.eulerAngles;
    }
}