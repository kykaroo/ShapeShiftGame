using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float slopeGravityMultiplier;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask balloonsMask;
    [SerializeField] private LayerMask stairsSlopeMask;

    private const float Gravity = 9.81f;

    private float _gravityForce;
    private float _slopeGravityForce;

    public Rigidbody PlayerBody => playerBody;

    public float GravityForce => _gravityForce;

    public float SlopeGravityForce => _slopeGravityForce;

    public LayerMask AllEnvironment => allEnvironment;

    public LayerMask WaterMask => waterMask;

    public LayerMask BalloonsMask => balloonsMask;

    public LayerMask StairsSlopeMask => stairsSlopeMask;


    private void Awake()
    {
        _gravityForce = Gravity * gravityMultiplier;
        _slopeGravityForce = _gravityForce * slopeGravityMultiplier;
    }
}