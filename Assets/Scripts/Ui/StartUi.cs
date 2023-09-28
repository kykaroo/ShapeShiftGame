using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class StartUi : MonoBehaviour
    { 
        [SerializeField] private Button startButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private TMP_Dropdown aiDifficultyDropdown;
        [SerializeField] private Button optionsButton;
        
        public event Action OnStartButtonClick;
        public event Action OnShopButtonClick;
        public event Action<int> OnDifficultyChanged;
        public event Action OnOptionsButtonClicked;

        public TMP_Dropdown AIDifficultyDropdown => aiDifficultyDropdown;

        private void StartButtonClick() => OnStartButtonClick?.Invoke();
        private void ShopButtonClick() => OnShopButtonClick?.Invoke();
        private void OptionsButtonClick() => OnOptionsButtonClicked?.Invoke();
        
        private void Awake()
        {
            startButton.onClick.AddListener(StartButtonClick);
            shopButton.onClick.AddListener(ShopButtonClick);
            optionsButton.onClick.AddListener(OptionsButtonClick);

            List<string> difficulties = new();
            foreach (var value in Enum.GetValues(typeof(AiDifficulty)))
            {
                difficulties.Add(value.ToString());
            }
            aiDifficultyDropdown.AddOptions(difficulties);
            aiDifficultyDropdown.onValueChanged.AddListener((change) => OnDifficultyChanged?.Invoke(change));
            aiDifficultyDropdown.value = 1;
        }
    }
}