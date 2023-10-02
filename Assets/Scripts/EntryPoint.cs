using System.Collections.Generic;
using Audio;
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
using UnityEngine.UI;

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
    [SerializeField] private OptionsUi optionsUi;
    [Header("Ground masks")]
    [SerializeField] private LayerMask allEnvironment;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private LayerMask balloonsMask;
    [SerializeField] private LayerMask stairsSlopeMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask underwaterGroundMask;
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
    [SerializeField] private AiDifficulty[] aiDifficulties;
    [SerializeField] private EnemyAi[] enemyAis;
    [Header("Factories")]
    [SerializeField] private HumanFormFactory humanFormFactory;
    [SerializeField] private CarFormFactory carFormFactory;
    [SerializeField] private HelicopterFormFactory helicopterFormFactory;
    [SerializeField] private BoatFormFactory boatFormFactory;
    [Header("ProgressBar")] 
    [SerializeField] private Slider playerProgressIndicator;
    [SerializeField] private Slider[] aiProgressIndicators;
    [Header("Audio")] 
    [SerializeField] private Sound[] musicSounds;
    [SerializeField] private Sound[] sfxSounds;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    private IPersistentPlayerData _persistentPlayerData;
    private LevelProgressBar.LevelProgressBar _levelProgressBar;

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
    
    private AudioManager _audioManager;
    private float _musicVolume;
    private float _sfxVolume;


    private void Start()
    {
        _persistentPlayerData = shopBootstrap.PersistentPlayerData;
        SpawnPlayerForms();
        InitializeLists();
        PrepareGameObjects();
        
        _audioManager = new(_persistentPlayerData, musicSounds, sfxSounds, musicSource, sfxSource);
        _audioManager.OnNewTrackPlay += (songName) => optionsUi.UpdateCurrentTrack(songName);
        optionsUi.Initialize(_persistentPlayerData);

        _ground = new(allEnvironment, waterMask, balloonsMask, stairsSlopeMask, groundMask, underwaterGroundMask);
        _levelGenerator = new(levelConfig, _ground);
        _gravityForce = Gravity * gravityMultiplier;
        
        CreatePlayerForms();
        CreateForms();
        AddButtonListeners();

        for (var i = 0; i < aiNumber; i++)
        {
            CreateAiForms(i);
            enemyAis[i].Initialize(aiDifficulties[i], _formStateMachine[i]);
        }

        _levelProgressBar = new();
        _levelProgressBar.Initialize(playerProgressIndicator, aiProgressIndicators, playerTransform, aiTransforms);
        
        RestartLevel();
    }

    private void SpawnPlayerForms()
    {
        var spawnPointPosition = playerStartPosition.position;
        _playerHumanForm = humanFormFactory.Get(_persistentPlayerData.PlayerGameData.SelectedHumanFormSkin, spawnPointPosition, playerTransform);
        _playerCarForm = carFormFactory.Get(_persistentPlayerData.PlayerGameData.SelectedCarFormSkin, spawnPointPosition, playerTransform);
        _playerHelicopterForm = helicopterFormFactory.Get(_persistentPlayerData.PlayerGameData.SelectedHelicopterFormSkin, spawnPointPosition, playerTransform);
        _playerBoatForm = boatFormFactory.Get(_persistentPlayerData.PlayerGameData.SelectedBoatFormSkin, spawnPointPosition, playerTransform);
    }

    private void PrepareGameObjects()
    {
        victoryCamera.SetActive(false);
        playerCamera.SetActive(true);
        formChangeUi.gameObject.SetActive(false); 
        startUi.gameObject.SetActive(false); 
        victoryUi.gameObject.SetActive(false); 
        shopUi.gameObject.SetActive(false);
        optionsUi.gameObject.SetActive(false);
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
        startUi.OnOptionsButtonClicked += OpenOptionsWindow;

        formChangeUi.OnHumanFormButtonClick += () => _playerFormStateMachine.SetState<HumanFormState>();
        formChangeUi.OnCarFormButtonClick += () => _playerFormStateMachine.SetState<CarFormState>();
        formChangeUi.OnHelicopterFormButtonClick += () => _playerFormStateMachine.SetState<HelicopterFormState>();
        formChangeUi.OnBoatFormButtonClick += () => _playerFormStateMachine.SetState<BoatFormState>();
        
        victoryUi.OnPlayAgainButtonClick += RestartLevel;

        shopUi.OnBackButtonClick += CloseShop;
        startUi.OnDifficultyChanged += ChangeDifficulty;

        optionsUi.OnMusicSliderValueChanged += ChangeMusicVolume;
        optionsUi.OnSfxSliderValueChanged += ChangeSfxVolume;
        optionsUi.OnMusicMuteButtonClick += ToggleMusicMute;
        optionsUi.OnSfxMuteButtonClick += ToggleSfxMute;
        optionsUi.OnNextTrackButtonClicked += SetNextMusicTrack;
        optionsUi.OnBackButtonClick += CloseOptionsWindow;
    }

    private void ToggleSfxMute()
    {
        _audioManager.ToggleSfx();
        optionsUi.UpdateSfxMuteIcon();
    }

    private void ToggleMusicMute()
    {
        _audioManager.ToggleMusic();
        optionsUi.UpdateMusicMuteIcon();
    }

    private void CloseOptionsWindow()
    {
        _audioManager.SaveSettings();
        optionsUi.gameObject.SetActive(false);
        startUi.gameObject.SetActive(true);
    }
    
    private void OpenOptionsWindow()
    {
        optionsUi.gameObject.SetActive(true);
        startUi.gameObject.SetActive(false);
    }

    private void ChangeSfxVolume(float newValue)
    {
        _audioManager.ChangeSfxVolume(newValue);
    }

    private void ChangeMusicVolume(float newValue)
    {
        _audioManager.ChangeMusicVolume(newValue);
    }

    private void SetNextMusicTrack()
    {
        optionsUi.UpdateCurrentTrack(_audioManager.PlayAllMusic());
    }

    private void ChangeDifficulty(int value)
    {
        switch (value)
        {
            case 0:
                SetAiDifficulty(AiDifficulty.Easy);
                break;
            case 1:
                SetAiDifficulty(AiDifficulty.Medium);
                break;
            case 2:
                SetAiDifficulty(AiDifficulty.Hard);
                break;
            case 3:
                SetAiDifficulty(AiDifficulty.Insane);
                break;
        }
    }

    private void SetAiDifficulty(AiDifficulty difficulty)
    {
        for (var i = 0; i < aiDifficulties.Length; i++)
        {
            aiDifficulties[i] = difficulty;
            enemyAis[i].SetDifficulty(difficulty);
        }
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
            _aiCarFormsList.Add(carFormFactory.Get(CarFormSkins.SportWhite, aiStartPositions[i].position, aiTransforms[i]));
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
        _levelProgressBar.SetLevelLenght(_levelGenerator.LevelStartZ, _levelGenerator.LevelEndZ);
        
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

    private void Update()
    {
        if (formChangeUi.gameObject.activeSelf)
        {
            _levelProgressBar.UpdateData();
        }
        
        _audioManager.Update();
    }
}
