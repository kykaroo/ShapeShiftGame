using System;
using System.Collections.Generic;
using FortuneWheel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugModeManager : MonoBehaviour
{
    [SerializeField] private Transform debugUi;
    [Header("Money")]
    [SerializeField] private Button setMoneyButton;
    [SerializeField] private TMP_InputField setMoneyInputField;
    [SerializeField] private Button applyInputFieldMoneyValueButton;
    [Header("Fortune wheel")]
    [SerializeField] private Button setSpinCooldownButtons;
    [SerializeField] private Button setSpinCooldownButton;
    [SerializeField] private TMP_InputField setSpinCooldownInputField;    
    [SerializeField] private Button applyInputFieldSpinCooldownValueButton;
    [SerializeField] private Button setSpinCurrentCooldownButton;
    [SerializeField] private TMP_InputField setSpinCurrentCooldownInputField;    
    [SerializeField] private Button applyInputFieldSpinCurrentCooldownValueButton;

    
    private Wallet.Wallet _wallet;
    private Timer _timer;

    public void Initialize(Wallet.Wallet wallet, Timer timer)
    {
        _wallet = wallet;
        _timer = timer;
    }
    
    private void Awake()
    {
        setMoneyButton.onClick.AddListener(ToggleSetMoneyInputField);
        setSpinCooldownButtons.onClick.AddListener(ToggleSetSpinButtons);
        setSpinCooldownButton.onClick.AddListener(ToggleSetSpinCooldownInputField);
        setSpinCurrentCooldownButton.onClick.AddListener(ToggleSetSpinCurrentCooldownInputField);
        
        applyInputFieldMoneyValueButton.onClick.AddListener(OnApplyInputFieldMoneyValueButton);
        applyInputFieldSpinCooldownValueButton.onClick.AddListener(OnApplyInputFieldSpinCooldownValueButton);
        applyInputFieldSpinCurrentCooldownValueButton.onClick.AddListener(OnApplyInputFieldSpinCurrentCooldownValueButton);
    }

    private void ToggleSetSpinButtons()
    {
        if (setSpinCooldownButton.gameObject.activeSelf || setSpinCurrentCooldownButton.gameObject.activeSelf)
        {
            setSpinCooldownButton.gameObject.SetActive(false);
            setSpinCurrentCooldownButton.gameObject.SetActive(false);
            applyInputFieldSpinCurrentCooldownValueButton.gameObject.SetActive(false);
            applyInputFieldSpinCooldownValueButton.gameObject.SetActive(false);
            setSpinCooldownInputField.gameObject.SetActive(false);
            setSpinCurrentCooldownInputField.gameObject.SetActive(false);
            return;
        }
        
        setSpinCooldownButton.gameObject.SetActive(true);
        setSpinCurrentCooldownButton.gameObject.SetActive(true);
    }

    private void OnApplyInputFieldSpinCurrentCooldownValueButton()
    {
        var cooldownTime = setSpinCurrentCooldownInputField.text.Split(":");
        if (TryParseUserInput(cooldownTime, out var timeSpan)) return; //TODO Сделать вывод ошибки формата пользователю
        _timer.SetCurrentCooldownTime(timeSpan);
        applyInputFieldSpinCurrentCooldownValueButton.gameObject.SetActive(false);
        ToggleSetSpinCurrentCooldownInputField();
    }

    private void ToggleSetSpinCurrentCooldownInputField()
    {
        if (setSpinCurrentCooldownInputField.gameObject.activeSelf)
        {
            setSpinCurrentCooldownInputField.gameObject.SetActive(false);
            applyInputFieldSpinCurrentCooldownValueButton.gameObject.SetActive(false);
            return;
        }
        
        setSpinCurrentCooldownInputField.gameObject.SetActive(true);
        applyInputFieldSpinCurrentCooldownValueButton.gameObject.SetActive(true);
    }

    private void OnApplyInputFieldSpinCooldownValueButton()
    {
        var cooldownTime = setSpinCooldownInputField.text.Split(":");
        if (TryParseUserInput(cooldownTime, out var timeSpan)) return; //TODO Сделать вывод ошибки формата пользователю
        _timer.SetCooldownTime(timeSpan);
        applyInputFieldSpinCooldownValueButton.gameObject.SetActive(false);
        ToggleSetSpinCooldownInputField();
    }

    private static bool TryParseUserInput(IEnumerable<string> cooldownTime, out TimeSpan timeSpan)
    {
        var cooldowns = new List<int>();
        var counter = 0;
        foreach (var time in cooldownTime)
        {
            if (!int.TryParse(time, out var value))
            {
                timeSpan = default;
                return true;
            }

            cooldowns.Add(value);
            counter++;
        }

        if (counter != 3)
        {
            timeSpan = default;
            return true;
        }

        timeSpan = new(cooldowns[0], cooldowns[1], cooldowns[2]);
        return false;
    }

    private void ToggleSetSpinCooldownInputField()
    {
        if (setSpinCooldownInputField.gameObject.activeSelf)
        {
            setSpinCooldownInputField.gameObject.SetActive(false);
            applyInputFieldSpinCooldownValueButton.gameObject.SetActive(false);
            return;
        }
        
        setSpinCooldownInputField.gameObject.SetActive(true);
        applyInputFieldSpinCooldownValueButton.gameObject.SetActive(true);
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
        
        setMoneyInputField.gameObject.SetActive(false);
        applyInputFieldMoneyValueButton.gameObject.SetActive(false);
        setSpinCooldownButton.gameObject.SetActive(false);
        setSpinCooldownInputField.gameObject.SetActive(false);
        applyInputFieldSpinCooldownValueButton.gameObject.SetActive(false);
        setSpinCurrentCooldownButton.gameObject.SetActive(false);
        setSpinCurrentCooldownInputField.gameObject.SetActive(false);
        applyInputFieldSpinCurrentCooldownValueButton.gameObject.SetActive(false);
        debugUi.gameObject.SetActive(true);
    }
}