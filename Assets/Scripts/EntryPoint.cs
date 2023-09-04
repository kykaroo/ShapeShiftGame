using FormStateMachine;
using FormStateMachine.Forms;
using FormStateMachine.States;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private FormFactory formFactory;
    [SerializeField] private GlobalVariables globalVariables;
    [SerializeField] private FormChangeUi formChangeUi;
    [SerializeField] private StartUi startUi;

    private FormStateMachine.FormStateMachine _formStateMachine;
    private HumanForm _humanForm;
    private CarForm _carForm;
    private HelicopterForm _helicopterForm;
    private BoatForm _boatForm;


    private void Start()
    {
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
            { typeof(HumanFormState), new HumanFormState(_humanForm, globalVariables) },
            { typeof(CarFormState), new CarFormState(_carForm, globalVariables) },
            { typeof(HelicopterFormState), new HelicopterFormState(_helicopterForm, globalVariables) },
            { typeof(BoatFormState), new BoatFormState(_boatForm, globalVariables) }
        });
    }

    void StartGame()
    {
        startUi.gameObject.SetActive(false);
        formChangeUi.gameObject.SetActive(true);
        
        _formStateMachine.SetState<HumanFormState>();
    }
}
