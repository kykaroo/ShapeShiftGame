using FortuneWheel;
using Ui;
using UnityEngine;
using Zenject;

namespace Presenters
{
    public class DebugUiPresenter : ITickable
    {
        private readonly DebugUi _debugUi;
        private Timer _timer;
        private Wallet.Wallet _wallet;

        [Inject]
        public DebugUiPresenter(DebugUi debugUi)
        {
            _debugUi = debugUi;
        }

        public void Tick()
        {
            if (!Input.GetKeyDown(KeyCode.P)) return;

            if (_debugUi.gameObject.activeSelf)
            {
                _debugUi.gameObject.SetActive(false);
                return;
            }
        
            _debugUi.PrepareButtons();
            _debugUi.gameObject.SetActive(true);
        }
    }
}