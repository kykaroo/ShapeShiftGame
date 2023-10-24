using UnityEngine;
using YG;
using Zenject;

namespace DependencyInjection
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private YandexGame yandexGame;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<YG.YandexGame>().FromComponentInNewPrefab(yandexGame).AsSingle().NonLazy();
        }
    }
}
