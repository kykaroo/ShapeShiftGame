using Audio;
using Presenters;
using Ui;
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
        [SerializeField] private Slider playerProgressIndicator;
        [SerializeField] private Slider[] aiProgressIndicators;
        [SerializeField] private Transform playerStartPosition;
        [SerializeField] private Transform[] aiStartPositionTransform;
        [SerializeField] private Sound[] musicSounds; 
        [SerializeField] private Sound[] sfxSounds;
        [SerializeField] private GameObjectContext playerPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StartUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<OptionsUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<VictoryUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FormChangeUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ShopUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FortuneWheelUiPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AudioManager>().AsSingle().WithArguments(musicSounds, sfxSounds).NonLazy();
            Container.BindInterfacesAndSelfTo<LevelProgressBar.LevelProgressBar>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<LevelProgressBar.ProgressBarUi>().AsSingle().WithArguments(playerProgressIndicator, aiProgressIndicators).NonLazy();

            Container.BindInterfacesAndSelfTo<FormChangeUi>().FromInstance(formChangeUi).AsSingle();
            Container.BindInterfacesAndSelfTo<StartUi>().FromInstance(startUi).AsSingle();
            Container.BindInterfacesAndSelfTo<VictoryUi>().FromInstance(victoryUi).AsSingle();
            Container.BindInterfacesAndSelfTo<ShopUi>().FromInstance(shopUi).AsSingle();
            Container.BindInterfacesAndSelfTo<OptionsUi>().FromInstance(optionsUi).AsSingle();
            Container.BindInterfacesAndSelfTo<FortuneWheelUi>().FromInstance(fortuneWheelUi).AsSingle();
            Container.BindInterfacesAndSelfTo<Slider>().FromInstance(playerProgressIndicator).AsSingle();
            Container.BindInterfacesAndSelfTo<Slider[]>().FromInstance(aiProgressIndicators).AsSingle();
            Container.BindInterfacesAndSelfTo<Transform[]>().FromInstance(aiStartPositionTransform).AsSingle();
        }
    }
}
