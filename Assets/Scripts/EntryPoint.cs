using FormStateMachine;
using FormStateMachine.Forms;
using FormStateMachine.States;
using Level;
using ScriptableObjects;
using Ui;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Header("Scene objects")]
    [SerializeField] private FormFactory formFactory;
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private GameObject startPosition;
    [SerializeField] private GlobalVariables globalVariables;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject victoryCamera;
    [Header("Ui")]
    [SerializeField] private FormChangeUi formChangeUi;
    [SerializeField] private StartUi startUi;
    [SerializeField] private VictoryUi victoryUi;
    [SerializeField] private LevelConfig levelConfig;
    [Header("Ground masks")]
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask balloonsMask;
    [SerializeField] private LayerMask stairsSlopeMask;
    [SerializeField] private LayerMask groundMask;
    [Header("Change form effect")]
    [SerializeField] private ParticleSystem poofParticleSystem;

    private FormStateMachine.FormStateMachine _formStateMachine;
    private HumanForm _humanForm;
    private CarForm _carForm;
    private HelicopterForm _helicopterForm;
    private BoatForm _boatForm;
    private Ground _ground;
    private LevelGenerator _levelGenerator;


    private void Start()
    {
        victoryCamera.SetActive(false);
        playerCamera.SetActive(true);
        _ground = new(allEnvironment, waterMask, balloonsMask, stairsSlopeMask, groundMask);
        _levelGenerator = new(levelConfig, _ground);
        CreateForms();
        SetupStates();
        AddButtonListeners();
        RestartLevel();
    }

    private void AddButtonListeners()
    {
        startUi.StartButton.onClick.AddListener(StartGame);
        formChangeUi.HumanFormButton.onClick.AddListener(() => _formStateMachine.SetState<HumanFormState>());
        formChangeUi.CarFormButton.onClick.AddListener(() => _formStateMachine.SetState<CarFormState>());
        formChangeUi.HelicopterFormButton.onClick.AddListener(() => _formStateMachine.SetState<HelicopterFormState>());
        formChangeUi.BoatFormButton.onClick.AddListener(() => _formStateMachine.SetState<BoatFormState>());
        victoryUi.PlayAgainButton.onClick.AddListener(RestartLevel);
    }

    private void CreateForms()
    {
        _humanForm = formFactory.CreateForm<HumanForm>();
        _carForm = formFactory.CreateForm<CarForm>();
        _helicopterForm = formFactory.CreateForm<HelicopterForm>();
        _boatForm = formFactory.CreateForm<BoatForm>();
    }

    private void SetupStates()
    {
        _formStateMachine = new(new()
        {
            { typeof(HumanFormState), new HumanFormState(_humanForm, globalVariables, _ground, playerBody, poofParticleSystem) },
            { typeof(CarFormState), new CarFormState(_carForm, globalVariables, _ground, playerBody, poofParticleSystem) },
            { typeof(HelicopterFormState), new HelicopterFormState(_helicopterForm, globalVariables, _ground, playerBody, poofParticleSystem) },
            { typeof(BoatFormState), new BoatFormState(_boatForm, globalVariables, _ground, playerBody, poofParticleSystem) },
            { typeof(NoneFormState), new NoneFormState(playerBody) }
        });
    }

    void StartGame()
    {
        startUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(false);
        formChangeUi.gameObject.SetActive(true);
        
        _formStateMachine.SetState<HumanFormState>();
    }

    void RestartLevel()
    {
        _levelGenerator.ClearLevel();
        _levelGenerator.GenerateLevel();
        _levelGenerator.VictoryTrigger.OnLevelComplete += LevelComplete;
        _formStateMachine.SetState<NoneFormState>();
        playerBody.position = startPosition.transform.position;

        formChangeUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(false);
        victoryCamera.SetActive(false);
        startUi.gameObject.SetActive(true);
        playerCamera.SetActive(true);
    }

    void LevelComplete()
    {
        victoryCamera.transform.position = playerCamera.transform.position;
        playerCamera.SetActive(false);
        victoryCamera.SetActive(true);
        formChangeUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(true);
    }
}
