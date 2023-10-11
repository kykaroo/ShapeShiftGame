using Ai;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class EnemyAiInstaller : MonoInstaller
    {
        [SerializeField] private Rigidbody[] aiBodies;
        [SerializeField] private Transform[] aiTransforms;
        [SerializeField] private ParticleSystem[] aiPoofParticleSystems;
        [SerializeField] private EnemyAi[] enemyAis;
        [SerializeField] private int aiNumber;
        [SerializeField] private AiDifficulty[] aiDifficulties;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EnemyHandler>().AsSingle().WithArguments(aiTransforms).NonLazy();

            Container.BindInterfacesAndSelfTo<AiDifficulty[]>().FromInstance(aiDifficulties).AsSingle();
            Container.BindInterfacesAndSelfTo<int>().FromInstance(aiNumber).AsSingle();
            Container.BindInterfacesAndSelfTo<EnemyAi[]>().FromInstance(enemyAis).AsSingle();
            Container.BindInterfacesAndSelfTo<ParticleSystem[]>().FromInstance(aiPoofParticleSystems).AsSingle();
            Container.BindInterfacesAndSelfTo<Rigidbody[]>().FromInstance(aiBodies).AsSingle();
        }
    }
}