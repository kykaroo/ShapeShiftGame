using Data;
using Data.PlayerGameData;
using Data.PlayerOptionsData;
using Level;
using Shop;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private LayerMask allEnvironment;
        [SerializeField] private LayerMask waterMask;
        [SerializeField] private LayerMask balloonsMask;
        [SerializeField] private LayerMask stairsSlopeMask;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask underwaterGroundMask;
        [SerializeField] private EntryPoint entryPoint;
        [SerializeField] private GameObjectContext playerPrefab;
        [SerializeField] private Transform playerStartTransform;
        [SerializeField] private float gravityMultiplier;

        public override void InstallBindings()
        { 
            Container.BindInterfacesTo<PlayerPrefsGameDataProvider>().AsSingle();
            Container.BindInterfacesTo<PlayerPrefsOptionsDataProvider>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<EntryPoint>().FromInstance(entryPoint).AsSingle();
            Container.BindInterfacesAndSelfTo<Wallet.Wallet>().AsSingle();
            Container.BindInterfacesAndSelfTo<OpenSkinsChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<SelectedSkinChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<SkinSelector>().AsSingle();
            Container.BindInterfacesAndSelfTo<SkinUnlocker>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<Ground>().AsSingle().WithArguments(allEnvironment, waterMask, balloonsMask, stairsSlopeMask, groundMask, underwaterGroundMask);

            Container.BindInterfacesAndSelfTo<LevelConfig>().FromInstance(levelConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(playerStartTransform).AsSingle();
            Container.BindInterfacesAndSelfTo<float>().FromInstance(gravityMultiplier).AsSingle();

            Container.Bind<Player.Player>().FromSubContainerResolve().ByNewContextPrefab(playerPrefab).AsSingle().NonLazy();
            
            Container.BindInterfacesAndSelfTo<PersistentGameData>()
                .FromMethod(context => context.Container.Resolve<IDataProvider<PersistentGameData>>().GetData());
            Container.BindInterfacesAndSelfTo<PlayerOptionsData>()
                .FromMethod(context => context.Container.Resolve<IDataProvider<PlayerOptionsData>>().GetData());
        }
    }
}