using FormStateMachine;
using FormStateMachine.Forms;
using FormStateMachine.States;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private FormFactory formFactory;
    [SerializeField] private FormChangeUi formChangeUi;

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
        _formStateMachine.SetState<HumanFormState>();
    }

    private void AddButtonListeners()
    {
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
            { typeof(HumanFormState), new HumanFormState(_humanForm) },
            { typeof(CarFormState), new CarFormState(_carForm) },
            { typeof(HelicopterFormState), new HelicopterFormState(_helicopterForm) },
            { typeof(BoatFormState), new BoatFormState(_boatForm) }
        });
    }
}
