using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Ui
{
    public class DebugUi : MonoBehaviour
    {
        [Header("Money")]
        [SerializeField] private Button setMoneyButton;
        [SerializeField] private TMP_InputField setMoneyInputField;
        [SerializeField] private Button applyInputFieldMoneyValueButton;
        [Header("Fortune wheel")]
        [SerializeField] private Button setSpinCooldownButtons;
        [Space]
        [SerializeField] private Button setSpinCooldownButton;
        [SerializeField] private TMP_InputField setSpinCooldownInputField;    
        [SerializeField] private Button applyInputFieldSpinCooldownValueButton;
        [Space]
        [SerializeField] private Button setSpinCurrentCooldownButton;
        [SerializeField] private TMP_InputField setSpinCurrentCooldownInputField;    
        [SerializeField] private Button applyInputFieldSpinCurrentCooldownValueButton;
        [Header("Music")]
        [SerializeField] private Button nextTrackButton;
        [SerializeField] private TextMeshProUGUI currentTrackText;
        [Header("Data")] 
        [SerializeField] private Button deleteSaveButton;
        [SerializeField] private TextMeshProUGUI onDeleteSaveButtonClickText;

        public event Action<int> OnChangeMoneyButtonClick;
        public event Action<TimeSpan> OnChangeCurrentSpinCooldownButtonClick;
        public event Action<TimeSpan> OnChangeSpinCooldownButtonClick;
        public event Action OnNextTrackButtonClicked;
        public event Action OnDeleteSaveButtonClicked;
        
        private void NextTrackButtonClick() => OnNextTrackButtonClicked?.Invoke();
        private void DeleteSaveButtonClick()
        {
            OnDeleteSaveButtonClicked?.Invoke();
            onDeleteSaveButtonClickText.gameObject.SetActive(true);
        }

        [Inject]
        public void Initialize()
        {
            setMoneyButton.onClick.AddListener(ToggleSetMoneyInputField);
            
            setSpinCooldownButtons.onClick.AddListener(ToggleSetSpinButtons);
            setSpinCooldownButton.onClick.AddListener(ToggleSetSpinCooldownInputField);
            setSpinCurrentCooldownButton.onClick.AddListener(ToggleSetSpinCurrentCooldownInputField);
        
            applyInputFieldMoneyValueButton.onClick.AddListener(OnApplyInputFieldMoneyValueButton);
            applyInputFieldSpinCooldownValueButton.onClick.AddListener(OnApplyInputFieldSpinCooldownValueButton);
            applyInputFieldSpinCurrentCooldownValueButton.onClick.AddListener(OnApplyInputFieldSpinCurrentCooldownValueButton);
            
            nextTrackButton.onClick.AddListener(NextTrackButtonClick);
            
            deleteSaveButton.onClick.AddListener(DeleteSaveButtonClick);
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
            OnChangeCurrentSpinCooldownButtonClick?.Invoke(timeSpan);
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
            OnChangeSpinCooldownButtonClick?.Invoke(timeSpan);
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
            OnChangeMoneyButtonClick?.Invoke(int.Parse(setMoneyInputField.text));
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

        public void PrepareButtons()
        {
            setMoneyInputField.gameObject.SetActive(false);
            applyInputFieldMoneyValueButton.gameObject.SetActive(false);
            setSpinCooldownButton.gameObject.SetActive(false);
            setSpinCooldownInputField.gameObject.SetActive(false);
            applyInputFieldSpinCooldownValueButton.gameObject.SetActive(false);
            setSpinCurrentCooldownButton.gameObject.SetActive(false);
            setSpinCurrentCooldownInputField.gameObject.SetActive(false);
            applyInputFieldSpinCurrentCooldownValueButton.gameObject.SetActive(false);
            currentTrackText.gameObject.SetActive(true);
            nextTrackButton.gameObject.SetActive(true);
            onDeleteSaveButtonClickText.gameObject.SetActive(false);
            setSpinCooldownButtons.gameObject.SetActive(true);
            setMoneyButton.gameObject.SetActive(true);
        }
        
        public void UpdateCurrentTrack(string trackName)
        {
            currentTrackText.text = $"Now playing: {trackName}";
        }
    }
}