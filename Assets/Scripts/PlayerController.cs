using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Button humanButton;
    [SerializeField] private Button carButton;
    [SerializeField] private Button helicopterButton;
    [SerializeField] private Button shipButton;

    [Header("Environment Settings")] 
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask stairsMask;
    [SerializeField] private LayerMask stairsSlopeMask;
    [Header("Human Form Settings")]
    [SerializeField] private GameObject humanFormGameObject;
    [SerializeField] private CapsuleCollider humanCollider;
    public float humanFormSpeed;
    [Header("Car Form Settings")]
    [SerializeField] private GameObject carFormGameObject;
    [SerializeField] private BoxCollider carCollider;
    public float carFormSpeed;
    [Header("Helicopter Form Settings")]
    [SerializeField] private GameObject helicopterFormGameObject;
    [SerializeField] private BoxCollider helicopterCollider;
    public float helicopterFormSpeed;
    [SerializeField] private float helicopterFlyHeight;
    public float helicopterUpwardsSpeed;
    [Header("Ship Form Settings")]
    [SerializeField] private GameObject shipFormGameObject;
    [SerializeField] private BoxCollider shipCollider;
    public float shipFormSpeed;

    private BoxCollider _playerCollider;
    private GameObject _currentActiveForm;
    private Vector3 _refVel;
    private RaycastHit _helicopterGroundHit;
    private RaycastHit _surfaceHit;
    private Vector3 _moveDirection;


    private void Start()
    {
        AssignButtons();
        _currentActiveForm = humanFormGameObject;
        HumanForm();
    }

    private void AssignButtons()
    {
        humanButton.onClick.AddListener(HumanForm);
        carButton.onClick.AddListener(CarForm);
        helicopterButton.onClick.AddListener(HelicopterForm);
        shipButton.onClick.AddListener(ShipForm);
    }

    private void ShipForm()
    {
        _currentActiveForm.SetActive(false);
        shipFormGameObject.SetActive(true);
        playerBody.useGravity = true;
        _currentActiveForm = shipFormGameObject;
    }

    private void HelicopterForm()
    {
        _currentActiveForm.SetActive(false);
        helicopterFormGameObject.SetActive(true);
        playerBody.useGravity = false;
        _currentActiveForm = helicopterFormGameObject;
    }

    private void CarForm()
    {
        _currentActiveForm.SetActive(false);
        carFormGameObject.SetActive(true);
        playerBody.useGravity = true;
        _currentActiveForm = carFormGameObject;
    }

    private void HumanForm()
    {
        _currentActiveForm.SetActive(false);
        humanFormGameObject.SetActive(true);
        playerBody.useGravity = true;
        _currentActiveForm = humanFormGameObject;
    }

    private void Update()
    {
        if (_currentActiveForm == humanFormGameObject)
            HumanFormMovement();

        if (_currentActiveForm == carFormGameObject)
            CarFormMovement();
        
        if (_currentActiveForm == helicopterFormGameObject)
            HelicopterFormMovement();
        
        if (_currentActiveForm == shipFormGameObject)
            ShipFormMovement();
    }

    private void ShipFormMovement()
    {
        transform.Translate(Vector3.forward * (shipFormSpeed * Time.deltaTime));
    }

    private void HelicopterFormMovement()
    {
        Physics.Raycast(gameObject.transform.position, Vector3.down, out _helicopterGroundHit, helicopterFlyHeight + 0.1f, allEnvironment);
        if (_helicopterGroundHit.distance <= helicopterFlyHeight)
            transform.Translate(Vector3.up * (helicopterUpwardsSpeed * Time.deltaTime));
        
        transform.Translate(Vector3.forward * (helicopterFormSpeed * Time.deltaTime));
    }

    private void CarFormMovement()
    {
        transform.Translate(Vector3.forward * (carFormSpeed * Time.deltaTime));
    }

    private void HumanFormMovement()
    {
        playerBody.useGravity = !Physics.Raycast(transform.position, Vector3.down, 5f, stairsSlopeMask);
        transform.Translate(GetMoveDirection() * (humanFormSpeed * Time.deltaTime));
    }
    
     private Vector3 GetMoveDirection()
     {
         return Physics.Raycast(transform.position, Vector3.down, out _surfaceHit, 5f, allEnvironment)
             ? Vector3.ProjectOnPlane(Vector3.forward, _surfaceHit.normal).normalized
             : Vector3.forward;
     }
}
