using UnityEngine;
using YG;
using Zenject;

namespace DependencyInjection
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private YandexGame yandexGame;
        [SerializeField] private ReviewYG reviewYg;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<YandexGame>().FromComponentInNewPrefab(yandexGame).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ReviewYG>().FromComponentInNewPrefab(reviewYg).AsSingle().NonLazy();
        }
    }
}
