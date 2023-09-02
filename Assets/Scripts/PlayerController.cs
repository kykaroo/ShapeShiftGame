using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Button humanButton;
    [SerializeField] private Button carButton;
    [SerializeField] private Button helicopterButton;
    [SerializeField] private Button shipButton;
    public float gravityScale;

    [Header("Environment Settings")] 
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask stairsSlopeMask;
    [SerializeField] private LayerMask balloonsMask;
    [Header("Human Form Settings")]
    [SerializeField] private GameObject humanFormGameObject;
    public float humanFormSpeed;
    [Header("Car Form Settings")]
    [SerializeField] private GameObject carFormGameObject;
    public float carFormSpeed;
    [Header("Helicopter Form Settings")]
    [SerializeField] private GameObject helicopterFormGameObject;
    public float helicopterFormSpeed;
    public float helicopterFlyHeight;
    public float helicopterUpwardsSpeed;
    [Header("Ship Form Settings")]
    [SerializeField] private GameObject shipFormGameObject;
    public float shipFormSpeed;

    private BoxCollider _playerCollider;
    private GameObject _currentActiveForm;
    private Vector3 _refVel;
    private RaycastHit _helicopterGroundHit;
    private RaycastHit _surfaceHit;
    private Vector3 _moveDirection;

    private float _gravityScale;
    private const float Gravity = -9.81f;
    private float _currentGravity;

    public LayerMask AllEnvironment => allEnvironment;


    private void Start()
    {
        AssignButtons();
        _gravityScale = gravityScale;
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
        _gravityScale = 1f;
        _currentActiveForm = shipFormGameObject;
    }

    private void HelicopterForm()
    {
        _currentActiveForm.SetActive(false);
        helicopterFormGameObject.SetActive(true);
        _gravityScale = 0f;
        _currentActiveForm = helicopterFormGameObject;
    }

    private void CarForm()
    {
        _currentActiveForm.SetActive(false);
        carFormGameObject.SetActive(true);
        _gravityScale = 1f;
        _currentActiveForm = carFormGameObject;
    }

    private void HumanForm()
    {
        _currentActiveForm.SetActive(false);
        humanFormGameObject.SetActive(true);
        _gravityScale = 1f;
        _currentActiveForm = humanFormGameObject;
    }

    private void FixedUpdate()
    {
        if (_currentActiveForm == humanFormGameObject)
            HumanFormMovement();

        if (_currentActiveForm == carFormGameObject)
            CarFormMovement();
        
        if (_currentActiveForm == helicopterFormGameObject)
            HelicopterFormMovement();
        
        if (_currentActiveForm == shipFormGameObject)
            ShipFormMovement();

        _currentGravity = Gravity * _gravityScale;
        playerBody.AddForce(new(0, _currentGravity, 0), ForceMode.Force);
    }

    private void ShipFormMovement()
    {
        if (Physics.CheckBox(shipFormGameObject.transform.position,
                shipFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                stairsSlopeMask))
            _gravityScale = gravityScale * 0.01f;
        else
            _gravityScale = gravityScale;
        
        if (Physics.CheckBox(shipFormGameObject.transform.position,
                    shipFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity, 
                    allEnvironment - waterMask))
        {
            transform.Translate(GetMoveDirection() * (shipFormSpeed * 0.1f * Time.deltaTime));
        }
        else
        {
            transform.Translate(GetMoveDirection() * (shipFormSpeed * Time.deltaTime));
        }
    }

    private void HelicopterFormMovement()
    {
        HandleHelicopterHeight();

        if (Physics.CheckBox(helicopterFormGameObject.transform.position,
                helicopterFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                balloonsMask))
        {
            transform.Translate(Vector3.forward * (helicopterFormSpeed * 0.1f * Time.deltaTime));
            return;
        }
        
        transform.Translate(Vector3.forward * (helicopterFormSpeed * Time.deltaTime));
    }

    private void HandleHelicopterHeight()
    {
        Physics.Raycast(helicopterFormGameObject.transform.position, Vector3.down, out _helicopterGroundHit,
            helicopterFlyHeight * 10f, allEnvironment);
        if (_helicopterGroundHit.distance <= helicopterFlyHeight - 0.2f)
            transform.Translate(Vector3.up * (helicopterUpwardsSpeed * Time.deltaTime));
        if (_helicopterGroundHit.distance >= helicopterFlyHeight + 0.2f)
            transform.Translate(Vector3.down * (helicopterUpwardsSpeed * Time.deltaTime));
    }

    private void CarFormMovement()
    {
        if (Physics.CheckBox(carFormGameObject.transform.position,
                carFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                stairsSlopeMask))
        {
            transform.Translate(GetMoveDirection() * (carFormSpeed * 0.1f * Time.deltaTime));
            _gravityScale = gravityScale * 0.01f;
            return;   
        }

        _gravityScale = gravityScale;
        
        if (Physics.CheckBox(carFormGameObject.transform.position,
                carFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                waterMask))
        {
            transform.Translate(GetMoveDirection() * (carFormSpeed * 0.1f * Time.deltaTime));
            return;
        }
        
        transform.Translate(GetMoveDirection() * (carFormSpeed * Time.deltaTime));
    }

    private void HumanFormMovement()
    {
        if (Physics.CheckBox(humanFormGameObject.transform.position,
                humanFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                stairsSlopeMask))
        {
            transform.Translate(GetMoveDirection() * (humanFormSpeed * Time.deltaTime));
            _gravityScale = gravityScale * 0.01f;
            return;
        }
        
        _gravityScale = gravityScale;

        if (Physics.CheckBox(humanFormGameObject.transform.position,
                humanFormGameObject.transform.localScale * 0.5f + Vector3.one * 0.001f, Quaternion.identity,
                waterMask))
        {
            transform.Translate(GetMoveDirection() * (humanFormSpeed * 0.1f * Time.deltaTime));
            return;
        }
        
        transform.Translate(GetMoveDirection() * (humanFormSpeed * Time.deltaTime));
    }
    
     private Vector3 GetMoveDirection()
     {
         return Physics.Raycast(transform.position, Vector3.down, out _surfaceHit, 5f, allEnvironment)
             ? Vector3.ProjectOnPlane(Vector3.forward, _surfaceHit.normal).normalized
             : Vector3.forward;
     }
}
