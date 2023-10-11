using System;
using FortuneWheel;
using Ui;
using Zenject;
public class DebugManager
{
    private readonly Wallet.Wallet _wallet;
    private readonly DebugUi _debugUi;
    private readonly Timer _timer;

    [Inject]
    public DebugManager(Wallet.Wallet wallet, DebugUi debugUi, Timer timer)
    {
        _wallet = wallet;
        _debugUi = debugUi;
        _timer = timer;

        _debugUi.OnChangeMoneyButtonClick += ApplyMoneyChange;
        _debugUi.OnChangeCurrentSpinCooldownButtonClick += SetCurrentSpinCooldownTime;
        _debugUi.OnChangeSpinCooldownButtonClick += SetSpinCooldownTime;
    }

    private void ApplyMoneyChange(int value)
    {
        _wallet.SetValue(value);
    }   

    private void SetCurrentSpinCooldownTime(TimeSpan timeSpan)
    {
        _timer.SetCurrentCooldownTime(timeSpan);
    }

    private void SetSpinCooldownTime(TimeSpan timeSpan)
    {
        _timer.SetCooldownTime(timeSpan);
    }
}