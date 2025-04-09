using _Game.Core.Events;
using _Game.Core.ServiceLocation;
using _Game.Core.Singletons;
using _Game.Interfaces;
using _Game.Systems;
using _Game.Systems.BetSystem;
using _Game.Systems.FinanceSystem;
using _Game.Systems.RouletteSystem;
using _Game.Systems.UISystem;
using UnityEngine;

namespace _Game.Core.DI
{
    public class GameBootstrapper : MonoBehaviour
    {
        [SerializeField] private UIInstaller uiInstaller;
        [SerializeField] private GameInstaller gameInstaller;
        [SerializeField] private RouletteInstaller rouletteInstaller;

        private void Awake() {
            var container = new DIContainer();
            var eventBus = new EventBus();

            // Core Services
            container.BindSingleton<IEventBus>(eventBus);
            // Register scene-local or optional services here
            var serviceLocator = new ServiceLocator();
            serviceLocator.Register<IServiceLocator>(serviceLocator);

            // Gameplay
            container.BindSingleton(new GameManager(eventBus));
            
            // Installer Classes
            uiInstaller.Install(container, eventBus);
            rouletteInstaller.Install(container, eventBus);
            gameInstaller.Install(container, eventBus);

            // Resolve and start
            container.Resolve<GameManager>();
            eventBus.Fire(new GameInitializedEvent());
        }
    }
}