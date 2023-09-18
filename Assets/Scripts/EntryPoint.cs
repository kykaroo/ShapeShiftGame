using System.Collections.Generic;
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
    [SerializeField] private int aiNumber;
    [Header("Scene objects")]
    [SerializeField] private FormFactory formFactory;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject victoryCamera;
    [Header("Ui")]
    [SerializeField] private FormChangeUi formChangeUi;
    [SerializeField] private StartUi startUi;
    [SerializeField] private VictoryUi victoryUi;
    [SerializeField] private Shop.Shop shopUi;
    [SerializeField] private LevelConfig levelConfig;
    [Header("Ground masks")]
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask balloonsMask;
    [SerializeField] private LayerMask stairsSlopeMask;
    [SerializeField] private LayerMask groundMask;
    [Header("Change form effect")]
    [SerializeField] private ParticleSystem[] poofParticleSystems;
    [SerializeField] private float gravityMultiplier;
    [Header("Player and AI objects")]
    [SerializeField] private Rigidbody[] playersBody;
    [SerializeField] private GameObject[] playersStartPositions;
    [Header("Ai difficulty")]
    [SerializeField] private AiDifficulty[] aiDifficulty;
    [SerializeField] private EnemyAi[] enemyAis;

    private List<FormStateMachine.FormStateMachine> _formStateMachine;
    private List<HumanForm> _humanForm;
    private List<CarForm> _carForm;
    private List<HelicopterForm> _helicopterForm;
    private List<BoatForm> _boatForm;
    private Ground _ground;
    private LevelGenerator _levelGenerator;
    private const float Gravity = 9.81f;
    private float _gravityForce;


    private void Start()
    {
        InitializeLists();
        PrepareGameObjects();
        
        _ground = new(allEnvironment, waterMask, balloonsMask, stairsSlopeMask, groundMask);
        _levelGenerator = new(levelConfig, _ground);
        _gravityForce = Gravity * gravityMultiplier;
        
        CreateForms();
        
        for (var i = 0; i < aiNumber + 1; i++)
        {
            CreatePlayer(i);
        }
        
        AddButtonListeners();
        
        for (var i = 0; i < aiNumber; i++)
        {
            enemyAis[i].Initialize(aiDifficulty[i], _formStateMachine[i + 1]);
        }
        
        RestartLevel();
    }

    private void PrepareGameObjects()
    {
        victoryCamera.SetActive(false);
        playerCamera.SetActive(true);
        formChangeUi.gameObject.SetActive(false); 
        startUi.gameObject.SetActive(false); 
        victoryUi.gameObject.SetActive(false); 
        shopUi.gameObject.SetActive(false);
    }

    private void InitializeLists()
    {
        _formStateMachine = new();
        _humanForm = new();
        _carForm = new();
        _helicopterForm = new();
        _boatForm = new();
    }

    private void CreatePlayer(int playerId)
    {
        _formStateMachine.Add( new(new()
        {
            { typeof(HumanFormState), new HumanFormState(_humanForm[playerId], _ground, playersBody[playerId], poofParticleSystems[playerId], _gravityForce) },
            { typeof(CarFormState), new CarFormState(_carForm[playerId], _ground, playersBody[playerId], poofParticleSystems[playerId], _gravityForce) },
            { typeof(HelicopterFormState), new HelicopterFormState(_helicopterForm[playerId], _ground, playersBody[playerId], poofParticleSystems[playerId], _gravityForce) },
            { typeof(BoatFormState), new BoatFormState(_boatForm[playerId], _ground, playersBody[playerId], poofParticleSystems[playerId], _gravityForce) },
            { typeof(NoneFormState), new NoneFormState(playersBody[playerId]) }
        }));
    }

    private void AddButtonListeners()
    {
        startUi.OnStartButtonClick += OnStartGame;
        startUi.OnShopButtonClick += OpenShop;

        formChangeUi.OnHumanFormButtonClick += () => _formStateMachine[0].SetState<HumanFormState>();
        formChangeUi.OnCarFormButtonClick += () => _formStateMachine[0].SetState<CarFormState>();
        formChangeUi.OnHelicopterFormButtonClick += () => _formStateMachine[0].SetState<HelicopterFormState>();
        formChangeUi.OnBoatFormButtonClick += () => _formStateMachine[0].SetState<BoatFormState>();
        
        victoryUi.OnPlayAgainButtonClick += RestartLevel;

        shopUi.OnBackButtonClick += CloseShop;
    }

    private void OpenShop()
    {
        startUi.gameObject.SetActive(false);
        shopUi.gameObject.SetActive(true);
    }

    private void CloseShop()
    {
        shopUi.gameObject.SetActive(false);
        startUi.gameObject.SetActive(true);
    }

    private void CreateForms()
    {
        for (var i = 0; i < aiNumber + 1; i++)
        {
            _humanForm.Add(formFactory.CreateForm<HumanForm>(i));
            _carForm.Add(formFactory.CreateForm<CarForm>(i));
            _helicopterForm.Add(formFactory.CreateForm<HelicopterForm>(i));
            _boatForm.Add(formFactory.CreateForm<BoatForm>(i));
        }
    }

    private void OnStartGame()
    {
        startUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(false);
        formChangeUi.gameObject.SetActive(true);

        foreach (var formStateMachine in _formStateMachine)
        {
            formStateMachine.SetState<HumanFormState>();
        }
    }

    private void RestartLevel()
    {
        _levelGenerator.ClearLevel();
        _levelGenerator.GenerateLevel();
        _levelGenerator.VictoryTrigger.OnLevelComplete += LevelComplete;
        foreach (var formStateMachine in _formStateMachine)
        {
            formStateMachine.SetState<NoneFormState>();
        }

        for (var i = 0; i < aiNumber + 1; i++)
        {
            playersBody[i].position = playersStartPositions[i].transform.position;
        }

        formChangeUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(false);
        victoryCamera.SetActive(false);
        startUi.gameObject.SetActive(true);
        playerCamera.SetActive(true);
    }

    private void LevelComplete()
    {
        victoryCamera.transform.position = playerCamera.transform.position;
        playerCamera.SetActive(false);
        victoryCamera.SetActive(true);
        formChangeUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(true);
    }
}
