using Data.PlayerGameData;
using FormsFactories;
using FormStateMachine;
using FormStateMachine.Forms;
using FormStateMachine.States;
using Level;
using UnityEngine;
using Zenject;

public class Player
{
    private FormStateMachine.FormStateMachine _formStateMachine;
    private HumanForm _playerHumanForm;
    private CarForm _playerCarForm;
    private HelicopterForm _playerHelicopterForm;
    private BoatForm _playerBoatForm;
    private Ground _ground;
    private readonly ParticleSystem _playerPoofParticleSystem;
    private readonly float _gravityForce;
    private readonly PersistentPlayerGameData _persistentPlayerGameData;
    private readonly Transform _playerTransform;
    private readonly HumanFormFactory _humanFormFactory;
    private readonly CarFormFactory _carFormFactory;
    private readonly HelicopterFormFactory _helicopterFormFactory;
    private readonly BoatFormFactory _boatFormFactory;
    private readonly LevelGenerator _levelGenerator;

    public CameraHolder CameraHolder { get; }

    public Rigidbody PlayerBody { get; }

    [Inject]
    public Player(Rigidbody playerBody, ParticleSystem playerPoofParticleSystem, 
        PersistentPlayerGameData persistentPlayerGameData, Transform playerTransform, HumanFormFactory humanFormFactory, 
        CarFormFactory carFormFactory, HelicopterFormFactory helicopterFormFactory, BoatFormFactory boatFormFactory, 
        Ground ground, CameraHolder cameraHolder, LevelGenerator levelGenerator)
    {
        PlayerBody = playerBody;
        _playerPoofParticleSystem = playerPoofParticleSystem;
        _persistentPlayerGameData = persistentPlayerGameData;
        _playerTransform = playerTransform;
        _humanFormFactory = humanFormFactory;
        _carFormFactory = carFormFactory;
        _helicopterFormFactory = helicopterFormFactory;
        _boatFormFactory = boatFormFactory;
        _ground = ground;
        CameraHolder = cameraHolder;
        _levelGenerator = levelGenerator;

        SpawnHumanForm();
        SpawnCarForm();
        SpawnHelicopterForm();
        SpawnBoatForm();
        
        CreatePlayerForms();
    }

    public void CreatePlayerForms()
    {
        _formStateMachine = new(new()
        {
            {
                typeof(HumanFormState),
                new HumanFormState(_playerHumanForm, _ground, PlayerBody, _playerPoofParticleSystem)
            },
            {
                typeof(CarFormState),
                new CarFormState(_playerCarForm, _ground, PlayerBody, _playerPoofParticleSystem)
            },
            {
                typeof(HelicopterFormState),
                new HelicopterFormState(_playerHelicopterForm, _ground, PlayerBody, _playerPoofParticleSystem)
            },
            {
                typeof(BoatFormState),
                new BoatFormState(_playerBoatForm, _ground, PlayerBody, _playerPoofParticleSystem)
            },
            { typeof(NoneFormState), new NoneFormState(PlayerBody) }
        });
    }
    
    public void SpawnHumanForm()
    {
        if (_playerHumanForm != null)
        {
            Object.Destroy(_playerHumanForm.gameObject);
        }
        
        _playerHumanForm = _humanFormFactory.Get(_persistentPlayerGameData.SelectedHumanFormSkin, _playerTransform.position, _playerTransform);
    }

    public void SpawnCarForm()
    {
        if (_playerCarForm != null)
        {
            Object.Destroy(_playerCarForm.gameObject);
        }
        
        _playerCarForm = _carFormFactory.Get(_persistentPlayerGameData.SelectedCarFormSkin, _playerTransform.position, _playerTransform);
    }

    public void SpawnHelicopterForm()
    {
        if (_playerHelicopterForm != null)
        {
            Object.Destroy(_playerHelicopterForm.gameObject);
        }

        _playerHelicopterForm = _helicopterFormFactory.Get(_persistentPlayerGameData.SelectedHelicopterFormSkin, _playerTransform.position, _playerTransform);
    }

    public void SpawnBoatForm()
    {
        if (_playerBoatForm != null)
        {
            Object.Destroy(_playerBoatForm.gameObject);
        }
        
        _playerBoatForm = _boatFormFactory.Get(_persistentPlayerGameData.SelectedBoatFormSkin, _playerTransform.position, _playerTransform);
    }

    public void SetHumanFormState()
    {
        _formStateMachine.SetState<HumanFormState>();
    }
    
    public void SetCarFormState()
    {
        _formStateMachine.SetState<CarFormState>();
    }
    
    public void SetHelicopterFormState()
    {
        _formStateMachine.SetState<HelicopterFormState>();
    }
    
    public void SetBoatFormState()
    {
        _formStateMachine.SetState<BoatFormState>();
    }
    
    public void SetNoneFormState()
    {
        _formStateMachine.SetState<NoneFormState>();
    }

    public void ClearPoofParticleSystem()
    {
        _playerPoofParticleSystem.Clear();
    }

    public void MoveToStartPosition()
    {
        PlayerBody.position = _levelGenerator.PlayerStartPosition.position; 
    }

    public IFormState GetCurrentState()
    {
        return _formStateMachine.GetCurrentState();
    }
}