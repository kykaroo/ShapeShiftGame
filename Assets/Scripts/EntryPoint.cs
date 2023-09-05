using System;
using FormStateMachine;
using FormStateMachine.Forms;
using FormStateMachine.States;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private FormFactory formFactory;
    [SerializeField] private Transform player;
    [SerializeField] private GlobalVariables globalVariables;
    [SerializeField] private FormChangeUi formChangeUi;
    [SerializeField] private StartUi startUi;
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float slopeGravityMultiplier;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask balloonsMask;
    [SerializeField] private LayerMask stairsSlopeMask;
    [SerializeField] private LayerMask groundMask;

    private FormStateMachine.FormStateMachine _formStateMachine;
    private HumanForm _humanForm;
    private CarForm _carForm;
    private HelicopterForm _helicopterForm;
    private BoatForm _boatForm;
    private Ground _ground;
    private LevelGenerator _levelGenerator;


    private void Start()
    {
        _ground = new(allEnvironment, waterMask, balloonsMask, stairsSlopeMask, groundMask);
        _levelGenerator = new(player, levelConfig, _ground);
        _levelGenerator.Start();
        CreateForms();
        SetupStates();
        AddButtonListeners();
        formChangeUi.gameObject.SetActive(false);
        startUi.gameObject.SetActive(true);
    }

    private void AddButtonListeners()
    {
        startUi.StartButton.onClick.AddListener(StartGame);
        formChangeUi.HumanFormButton.onClick.AddListener(() => _formStateMachine.SetState<HumanFormState>());
        formChangeUi.CarFormButton.onClick.AddListener(() => _formStateMachine.SetState<CarFormState>());
        formChangeUi.HelicopterFormButton.onClick.AddListener(() => _formStateMachine.SetState<HelicopterFormState>());
        formChangeUi.BoatFormButton.onClick.AddListener(() => _formStateMachine.SetState<BoatFormState>());
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
            { typeof(HumanFormState), new HumanFormState(_humanForm, globalVariables, _ground) },
            { typeof(CarFormState), new CarFormState(_carForm, globalVariables, _ground) },
            { typeof(HelicopterFormState), new HelicopterFormState(_helicopterForm, globalVariables, _ground) },
            { typeof(BoatFormState), new BoatFormState(_boatForm, globalVariables, _ground) }
        });
    }

    void StartGame()
    {
        startUi.gameObject.SetActive(false);
        formChangeUi.gameObject.SetActive(true);
        
        _formStateMachine.SetState<HumanFormState>();
    }

    private void Update()
    {
        _levelGenerator.Update();
    }
}
