using System.Collections.Generic;
using FormsFactories;
using FormStateMachine.Forms;
using FormStateMachine.States;
using Level;
using Shop.BoatFormSkins;
using Shop.CarFormSkins;
using Shop.HelicopterFormSkins;
using Shop.HumanFormSkins;
using UnityEngine;
using Zenject;

public class EnemyHandler
{
    private readonly EnemyAi[] _enemyAis;
    private readonly AiDifficulty[] _aiDifficulties;
    private List<HumanForm> _aiHumanFormList;
    private readonly Ground _ground;
    private readonly Rigidbody[] _aiBodies;
    private List<CarForm> _aiCarFormsList;
    private readonly ParticleSystem[] _aiPoofParticleSystems;
    private readonly int _aiNumber;
    private List<HelicopterForm> _aiHelicopterFormsList;
    private List<BoatForm> _aiBoatFormsList;
    private readonly HumanFormFactory _humanFormFactory;
    private readonly CarFormFactory _carFormFactory;
    private readonly HelicopterFormFactory _helicopterFormFactory;
    private readonly BoatFormFactory _boatFormFactory;
    private readonly Transform[] _aiTransforms;
    private readonly LevelGenerator _levelGenerator;
    
    public List<FormStateMachine.FormStateMachine> AiFormStateMachine;
    
    public Rigidbody[] AIBodies => _aiBodies;

    [Inject]
    public EnemyHandler(EnemyAi[] enemyAis, List<FormStateMachine.FormStateMachine> aiFormStateMachine,
        AiDifficulty[] aiDifficulties, List<HumanForm> aiHumanFormList, Ground ground, Rigidbody[] aiBodies,
        List<CarForm> aiCarFormsList, ParticleSystem[] aiPoofParticleSystems, int aiNumber,
        List<HelicopterForm> aiHelicopterFormsList,
        List<BoatForm> aiBoatFormsList, HumanFormFactory humanFormFactory,
        CarFormFactory carFormFactory, HelicopterFormFactory helicopterFormFactory, BoatFormFactory boatFormFactory,
        Transform[] aiTransforms, LevelGenerator levelGenerator)
    {
        _enemyAis = enemyAis;
        AiFormStateMachine = aiFormStateMachine;
        _aiDifficulties = aiDifficulties;
        _aiHumanFormList = aiHumanFormList;
        _ground = ground;
        _aiBodies = aiBodies;
        _aiCarFormsList = aiCarFormsList;
        _aiPoofParticleSystems = aiPoofParticleSystems;
        _aiNumber = aiNumber;
        _aiHelicopterFormsList = aiHelicopterFormsList;
        _aiBoatFormsList = aiBoatFormsList;
        _humanFormFactory = humanFormFactory;
        _carFormFactory = carFormFactory;
        _helicopterFormFactory = helicopterFormFactory;
        _boatFormFactory = boatFormFactory;
        _aiTransforms = aiTransforms;
        _levelGenerator = levelGenerator;
        
        InitializeLists();
        CreateForms();
        InitializeAi();
    }
    
    private void InitializeLists()
    {
        AiFormStateMachine = new();
        _aiHumanFormList = new();
        _aiCarFormsList = new();
        _aiHelicopterFormsList = new();
        _aiBoatFormsList = new();
    }

    private void InitializeAi()
    {
        for (var i = 0; i < _aiNumber; i++)
        {
            CreateAiForms(i);
            _enemyAis[i].Initialize(_aiDifficulties[i], AiFormStateMachine[i]);
        }
    }

    private void CreateAiForms(int playerId)
    {
        AiFormStateMachine.Add(new(new()
        {
            {
                typeof(HumanFormState),
                new HumanFormState(_aiHumanFormList[playerId], _ground, _aiBodies[playerId], 
                    _aiPoofParticleSystems[playerId])
            },
            {
                typeof(CarFormState),
                new CarFormState(_aiCarFormsList[playerId], _ground, _aiBodies[playerId],
                    _aiPoofParticleSystems[playerId])
            },
            {
                typeof(HelicopterFormState),
                new HelicopterFormState(_aiHelicopterFormsList[playerId], _ground, _aiBodies[playerId],
                    _aiPoofParticleSystems[playerId])
            },
            {
                typeof(BoatFormState),
                new BoatFormState(_aiBoatFormsList[playerId], _ground, _aiBodies[playerId],
                    _aiPoofParticleSystems[playerId])
            },
            { typeof(NoneFormState), new NoneFormState(_aiBodies[playerId]) }
        }));
    }
    
    private void CreateForms()
    {
        for (var i = 0; i < _aiNumber; i++)
        {
            _aiHumanFormList.Add(_humanFormFactory.Get(HumanFormSkins.White, _aiTransforms[i].position, _aiTransforms[i]));
            _aiCarFormsList.Add(_carFormFactory.Get(CarFormSkins.SportWhite, _aiTransforms[i].position, _aiTransforms[i]));
            _aiHelicopterFormsList.Add(_helicopterFormFactory.Get(HelicopterFormSkins.Scout, _aiTransforms[i].position, _aiTransforms[i]));
            _aiBoatFormsList.Add(_boatFormFactory.Get(BoatFormSkins.Boat1, _aiTransforms[i].position, _aiTransforms[i]));
        }
    }

    public void RestartAllBots()
    {
        for (var i = 0; i < _aiNumber; i++)
        {
            _aiPoofParticleSystems[i].Clear();
            _enemyAis[i].StopAllCoroutines();
            _aiBodies[i].position = _levelGenerator.AIStartPositions[i].position;
        }
    }
}