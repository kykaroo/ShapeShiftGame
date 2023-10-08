using Ui;

namespace Presenters
{
    public class FortuneWheelUiPresenter
    {
        private readonly FortuneWheelUi _fortuneWheelUi;
        private readonly StartUi _startUi;

        public FortuneWheelUiPresenter(FortuneWheelUi fortuneWheelUi, StartUi startUi)
        {
            _fortuneWheelUi = fortuneWheelUi;
            _startUi = startUi;
            
            fortuneWheelUi.OnBackButtonClick += CloseFortuneWheelWindow;
        }
        
        private void CloseFortuneWheelWindow()
        {
            _fortuneWheelUi.gameObject.SetActive(false);
            _startUi.gameObject.SetActive(true);
        }
    }
}