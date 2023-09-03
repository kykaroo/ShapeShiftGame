using UnityEngine;
using UnityEngine.UI;

public class FormChangeUi : MonoBehaviour
{
    [SerializeField] private Button humanFormButton;
    [SerializeField] private Button carFormButton;
    [SerializeField] private Button helicopterFormButton;
    [SerializeField] private Button boatFormButton;

    public Button HumanFormButton => humanFormButton;

    public Button CarFormButton => carFormButton;

    public Button HelicopterFormButton => helicopterFormButton;

    public Button BoatFormButton => boatFormButton;
}