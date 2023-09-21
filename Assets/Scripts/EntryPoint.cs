using System.Collections.Generic;
using Data;
using FormsFactories;
using FormStateMachine.Forms;
using FormStateMachine.States;
using Level;
using ScriptableObjects;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;
using Ui;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [Header("Scene objects")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject victoryCamera;
    [SerializeField] private ShopBootstrap shopBootstrap;
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
    [SerializeField] private ParticleSystem playerPoofParticleSystem;
    [SerializeField] private ParticleSystem[] aiPoofParticleSystems;
    [SerializeField] private float gravityMultiplier;
    [Header("Player and AI objects")]
    [SerializeField] private Rigidbody playerBody;
    [SerializeField] private Rigidbody[] aiBodies;
    [SerializeField] private Transform playerStartPosition;
    [SerializeField] private Transform[] aiStartPositions;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform[] aiTransforms;
    [Header("Ai")]
    [SerializeField] private int aiNumber;
    [SerializeField] private AiDifficulty[] aiDifficulty;
    [SerializeField] private EnemyAi[] enemyAis;
    [Header("Factories")]
    [SerializeField] private HumanFormFactory humanFormFactory;
    [SerializeField] private CarFormFactory carFormFactory;
    [SerializeField] private HelicopterFormFactory helicopterFormFactory;
    [SerializeField] private BoatFormFactory boatFormFactory;
    
    private IPersistentData _persistentPlayerData;

    private FormStateMachine.FormStateMachine _playerFormStateMachine;
    private HumanForm _playerHumanForm;
    private CarForm _playerCarForm;
    private HelicopterForm _playerHelicopterForm;
    private BoatForm _playerBoatForm;
    
    private List<FormStateMachine.FormStateMachine> _formStateMachine;
    private List<HumanForm> _aiHumanFormList;
    private List<CarForm> _aiCarFormsList;
    private List<HelicopterForm> _aiHelicopterFormsList;
    private List<BoatForm> _aiBoatFormsList;
    
    private Ground _ground;
    private LevelGenerator _levelGenerator;
    private const float Gravity = 9.81f;
    private float _gravityForce;


    private void Start()
    {
        _persistentPlayerData = shopBootstrap.PersistentPlayerData;
        SpawnPlayerForms();
        InitializeLists();
        PrepareGameObjects();
        
        
        _ground = new(allEnvironment, waterMask, balloonsMask, stairsSlopeMask, groundMask);
        _levelGenerator = new(levelConfig, _ground);
        _gravityForce = Gravity * gravityMultiplier;
        
        CreatePlayerForms();
        CreateForms();
        AddButtonListeners();
        
        for (var i = 0; i < aiNumber; i++)
        {
            CreateAiForms(i);
            enemyAis[i].Initialize(aiDifficulty[i], _formStateMachine[i]);
        }
        
        RestartLevel();
    }

    private void SpawnPlayerForms()
    {
        var spawnPointPosition = playerStartPosition.position;
        _playerHumanForm = humanFormFactory.Get(_persistentPlayerData.PlayerData.SelectedHumanFormSkin, spawnPointPosition, playerTransform);
        _playerCarForm = carFormFactory.Get(_persistentPlayerData.PlayerData.SelectedCarFormSkin, spawnPointPosition, playerTransform);
        _playerHelicopterForm = helicopterFormFactory.Get(_persistentPlayerData.PlayerData.SelectedHelicopterFormSkin, spawnPointPosition, playerTransform);
        _playerBoatForm = boatFormFactory.Get(_persistentPlayerData.PlayerData.SelectedBoatFormSkin, spawnPointPosition, playerTransform);
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
        _aiHumanFormList = new();
        _aiCarFormsList = new();
        _aiHelicopterFormsList = new();
        _aiBoatFormsList = new();
    }
    
    private void CreatePlayerForms()
    {
        _playerFormStateMachine = new(new()
        {
            {
                typeof(HumanFormState),
                new HumanFormState(_playerHumanForm, _ground, playerBody, playerPoofParticleSystem, _gravityForce)
            },
            {
                typeof(CarFormState),
                new CarFormState(_playerCarForm, _ground, playerBody, playerPoofParticleSystem, _gravityForce)
            },
            {
                typeof(HelicopterFormState),
                new HelicopterFormState(_playerHelicopterForm, _ground, playerBody, playerPoofParticleSystem,
                    _gravityForce)
            },
            {
                typeof(BoatFormState),
                new BoatFormState(_playerBoatForm, _ground, playerBody, playerPoofParticleSystem, _gravityForce)
            },
            { typeof(NoneFormState), new NoneFormState(playerBody) }
        });
    }

    private void CreateAiForms(int playerId)
    {
        _formStateMachine.Add(new(new()
        {
            {
                typeof(HumanFormState),
                new HumanFormState(_aiHumanFormList[playerId], _ground, aiBodies[playerId],
                    aiPoofParticleSystems[playerId], _gravityForce)
            },
            {
                typeof(CarFormState),
                new CarFormState(_aiCarFormsList[playerId], _ground, aiBodies[playerId],
                    aiPoofParticleSystems[playerId], _gravityForce)
            },
            {
                typeof(HelicopterFormState),
                new HelicopterFormState(_aiHelicopterFormsList[playerId], _ground, aiBodies[playerId],
                    aiPoofParticleSystems[playerId], _gravityForce)
            },
            {
                typeof(BoatFormState),
                new BoatFormState(_aiBoatFormsList[playerId], _ground, aiBodies[playerId],
                    aiPoofParticleSystems[playerId], _gravityForce)
            },
            { typeof(NoneFormState), new NoneFormState(aiBodies[playerId]) }
        }));
    }

    private void AddButtonListeners()
    {
        startUi.OnStartButtonClick += OnStartGame;
        startUi.OnShopButtonClick += OpenShop;

        formChangeUi.OnHumanFormButtonClick += () => _playerFormStateMachine.SetState<HumanFormState>();
        formChangeUi.OnCarFormButtonClick += () => _playerFormStateMachine.SetState<CarFormState>();
        formChangeUi.OnHelicopterFormButtonClick += () => _playerFormStateMachine.SetState<HelicopterFormState>();
        formChangeUi.OnBoatFormButtonClick += () => _playerFormStateMachine.SetState<BoatFormState>();
        
        victoryUi.OnPlayAgainButtonClick += RestartLevel;

        shopUi.OnBackButtonClick += CloseShop;
    }

    private void OpenShop()
    {
        startUi.gameObject.SetActive(false);
        shopUi.gameObject.SetActive(true);
    }

    private void ClearPlayerForms()
    {
        Destroy(_playerHumanForm.gameObject);
        Destroy(_playerCarForm.gameObject);
        Destroy(_playerHelicopterForm.gameObject);
        Destroy(_playerBoatForm.gameObject);
    }

    private void CloseShop()
    {
        shopUi.gameObject.SetActive(false);
        startUi.gameObject.SetActive(true);
        
        ClearPlayerForms();
        SpawnPlayerForms();
        CreatePlayerForms();
    }

    private void CreateForms()
    {
        for (var i = 0; i < aiNumber; i++)
        {
            _aiHumanFormList.Add(humanFormFactory.Get(HumanFormSkins.White, aiStartPositions[i].position, aiTransforms[i]));
            _aiCarFormsList.Add(carFormFactory.Get(CarFormSkins.White, aiStartPositions[i].position, aiTransforms[i]));
            _aiHelicopterFormsList.Add(helicopterFormFactory.Get(HelicopterFormSkins.Scout, aiStartPositions[i].position, aiTransforms[i]));
            _aiBoatFormsList.Add(boatFormFactory.Get(BoatFormSkins.Boat1, aiStartPositions[i].position, aiTransforms[i]));
        }
    }

    private void OnStartGame()
    {
        startUi.gameObject.SetActive(false);
        victoryUi.gameObject.SetActive(false);
        formChangeUi.gameObject.SetActive(true);
        
        _playerFormStateMachine.SetState<HumanFormState>();

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
        
        _playerFormStateMachine.SetState<NoneFormState>();
        playerPoofParticleSystem.Clear();
            
        foreach (var formStateMachine in _formStateMachine)
        {
            formStateMachine.SetState<NoneFormState>();
        }

        playerBody.position = playerStartPosition.position;

        for (var i = 0; i < aiNumber; i++)
        {
            aiPoofParticleSystems[i].Clear();
            enemyAis[i].StopAllCoroutines();
            aiBodies[i].position = aiStartPositions[i].position;
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
