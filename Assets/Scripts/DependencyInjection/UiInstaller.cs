using Audio;
using FortuneWheel;
using Presenters;
using Ui;
using Ui.ProgressBar;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace DependencyInjection
{
    public class UiInstaller : MonoInstaller
    {
        [SerializeField] private FormChangeUi formChangeUi;
        [SerializeField] private StartUi startUi;
        [SerializeField] private VictoryUi victoryUi;
        [SerializeField] private ShopUi shopUi;
        [SerializeField] private OptionsUi optionsUi;
        [SerializeField] private FortuneWheelUi fortuneWheelUi;
        [SerializeField] private DebugUi debugUi;
        [SerializeField] private DefeatUi defeatUi;
        [SerializeField] private Timer timer;
        [SerializeField] private Slider playerProgressIndicator;
        [SerializeField] private Slider[] aiProgressIndicators;
        [SerializeField] private Transform[] aiStartPositionTransform;
        [SerializeField] private Sound[] musicSounds; 
        [SerializeField] private AudioClip[] humanSounds;
        [SerializeField] private AudioClip[] carSounds;
        [SerializeField] private AudioClip[] helicopterSounds;
        [SerializeField] private AudioClip[] boatSounds;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StartUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<OptionsUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<VictoryUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FormChangeUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShopUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FortuneWheelUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DebugUiPresenter>().AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().WithArguments(musicSounds, humanSounds, carSounds, helicopterSounds, boatSounds).NonLazy();
            Container.BindInterfacesAndSelfTo<Ui.ProgressBar.LevelProgressBar>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ProgressBarUi>().AsSingle().WithArguments(playerProgressIndicator, aiProgressIndicators).NonLazy();
            Container.BindInterfacesAndSelfTo<WheelManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DebugManager>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<FormChangeUi>().FromInstance(formChangeUi).AsSingle();
            Container.BindInterfacesAndSelfTo<StartUi>().FromInstance(startUi).AsSingle();
            Container.BindInterfacesAndSelfTo<VictoryUi>().FromInstance(victoryUi).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopUi>().FromInstance(shopUi).AsSingle();
            Container.BindInterfacesAndSelfTo<OptionsUi>().FromInstance(optionsUi).AsSingle();
            Container.BindInterfacesAndSelfTo<FortuneWheelUi>().FromInstance(fortuneWheelUi).AsSingle();
            Container.BindInterfacesAndSelfTo<DefeatUi>().FromInstance(defeatUi).AsSingle();
            Container.BindInterfacesAndSelfTo<Slider>().FromInstance(playerProgressIndicator).AsSingle();
            Container.BindInterfacesAndSelfTo<Slider[]>().FromInstance(aiProgressIndicators).AsSingle();
            Container.BindInterfacesAndSelfTo<Transform[]>().FromInstance(aiStartPositionTransform).AsSingle();
            Container.BindInterfacesAndSelfTo<DebugUi>().FromInstance(debugUi).AsSingle();
            Container.BindInterfacesAndSelfTo<Timer>().FromInstance(timer).AsSingle();
        }
    }
}
