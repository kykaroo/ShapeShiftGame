using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody playerBody;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private ParticleSystem playerPoofParticleSystem;
        [SerializeField] private Transform cameraHolderGo;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform cameraGamePosition;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Player>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CameraHolder>().AsSingle().WithArguments(cameraHolderGo, playerTransform, playerCamera, cameraGamePosition).NonLazy();
            
            Container.BindInterfacesAndSelfTo<Rigidbody>().FromInstance(playerBody).AsSingle();
            Container.BindInterfacesAndSelfTo<Transform>().FromInstance(playerTransform).AsSingle();
            Container.BindInterfacesAndSelfTo<ParticleSystem>().FromInstance(playerPoofParticleSystem).AsSingle();
        }
    }
}