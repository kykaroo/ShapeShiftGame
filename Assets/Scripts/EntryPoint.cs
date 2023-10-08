using FortuneWheel;
using Presenters;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour, IInitializable
{
    private WheelManager _wheelManager;
    private StartUiPresenter _startUiPresenter;

    [Inject]
    public void Construct(WheelManager wheelManager, StartUiPresenter startUiPresenter)
    {
        _wheelManager = wheelManager;
        _startUiPresenter = startUiPresenter;
    }

    public void Initialize()
    {
        if (_wheelManager.Timer.CanClaimFreeReward)
        {
            _startUiPresenter.OpenFortuneWheelWindow();
        }
    }
}
