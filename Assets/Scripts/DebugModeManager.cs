using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugModeManager : MonoBehaviour
{
    [SerializeField] private Transform debugUi;
    [SerializeField] private Button setMoneyButton;
    [SerializeField] private TMP_InputField setMoneyInputField;
    [SerializeField] private Button applyInputFieldMoneyValueButton;
    [SerializeField] private Button setSpinCooldown;
    
    private Wallet.Wallet _wallet;

    public void Initialize(Wallet.Wallet wallet)
    {
        _wallet = wallet;
    }
    
    private void Awake()
    {
        setMoneyButton.onClick.AddListener(ToggleSetMoneyInputField);
        applyInputFieldMoneyValueButton.onClick.AddListener(OnApplyInputFieldMoneyValueButton);
    }

    private void OnApplyInputFieldMoneyValueButton()
    {
        _wallet.SetValue(int.Parse(setMoneyInputField.text));
        applyInputFieldMoneyValueButton.gameObject.SetActive(false);
        ToggleSetMoneyInputField();
    }

    private void ToggleSetMoneyInputField()
    {
        if (setMoneyInputField.gameObject.activeSelf)
        {
            setMoneyInputField.gameObject.SetActive(false);
            applyInputFieldMoneyValueButton.gameObject.SetActive(false);
            return;
        }
        
        setMoneyInputField.gameObject.SetActive(true);
        applyInputFieldMoneyValueButton.gameObject.SetActive(true);
    }

    public void Update()
    {
        if (!Input.GetKeyDown(KeyCode.P)) return;

        if (debugUi.gameObject.activeSelf)
        {
            debugUi.gameObject.SetActive(false);
            return;
        }
        
        debugUi.gameObject.SetActive(true);
    }
}