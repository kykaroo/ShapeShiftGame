using FormsFactories;
using UnityEngine;
using Zenject;

namespace DependencyInjection
{
    public class FactoryInstaller : MonoInstaller
    {
        [SerializeField] private HumanFormFactory humanFormFactory;
        [SerializeField] private CarFormFactory carFormFactory;
        [SerializeField] private HelicopterFormFactory helicopterFormFactory;
        [SerializeField] private BoatFormFactory boatFormFactory;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HumanFormFactory>().FromInstance(humanFormFactory).AsSingle();
            Container.BindInterfacesAndSelfTo<CarFormFactory>().FromInstance(carFormFactory).AsSingle();
            Container.BindInterfacesAndSelfTo<HelicopterFormFactory>().FromInstance(helicopterFormFactory).AsSingle();
            Container.BindInterfacesAndSelfTo<BoatFormFactory>().FromInstance(boatFormFactory).AsSingle();
        }
    }
}