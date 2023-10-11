using Presenters;
using Ui;
using UnityEngine;
using Zenject;

public class EntryPoint : MonoBehaviour, IInitializable
{
    private FortuneWheelUi _fortuneWheelUi;
    private StartUiPresenter _startUiPresenter;

    [Inject]
    public void Construct(FortuneWheelUi fortuneWheelUi, StartUiPresenter startUiPresenter)
    {
        _fortuneWheelUi = fortuneWheelUi;
        _startUiPresenter = startUiPresenter;
    }

    public void Initialize()
    {
        if (_fortuneWheelUi.Timer.CanClaimFreeReward)
        {
            _startUiPresenter.OpenFortuneWheelWindow();
        }
    }
}
