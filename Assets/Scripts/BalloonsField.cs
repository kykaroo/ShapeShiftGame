using UnityEngine;

public class BalloonsField : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private RaycastHit _terrainHit;

    private void Start()
    {
        SetBalloonFieldHeight();
    }

    private void SetBalloonFieldHeight()
    {
        var position = transform.position;

        Physics.Raycast(position, Vector3.down, out _terrainHit,
            playerController.helicopterFlyHeight * 10f, playerController.AllEnvironment);
        if (_terrainHit.distance <= playerController.helicopterFlyHeight)
            transform.position = new(position.x, position.y + (playerController.helicopterFlyHeight - _terrainHit.distance),
                position.z);
        else
            transform.position = new(position.x, position.y - _terrainHit.distance, position.z);
    }
}
