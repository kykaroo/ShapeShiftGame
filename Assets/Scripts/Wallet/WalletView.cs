using TMPro;
using UnityEngine;
using Zenject;

namespace Wallet
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI value;

        private Wallet _wallet;

        [Inject]
        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;

            UpdateValue(_wallet.GetCurrentCoins());

            _wallet.CoinsChanged += UpdateValue;
        }

        private void UpdateValue(int currentCoins) => value.text = currentCoins.ToString();
    }
}
