using _Game.Core.DI;
using _Game.Core.ScriptableObjects;
using _Game.Core.Singletons;
using _Game.Interfaces;
using _Game.Utils;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteInstaller : MonoBehaviour
    {
        [SerializeField] private WheelView wheelPrefab;
        [SerializeField] private BallView ballPrefab;
        [SerializeField] private GameObject rouletteParent;
        [SerializeField] private ManagerRunner managerRunnerPrefab;
        [SerializeField] private RouletteConfigSO rouletteConfig;
        
        public void Install(DIContainer container, IEventBus eventBus)
        {
            // Create a RouletteContext from the factory.
            var factory = new RouletteFactory();
            var context = factory.CreateRoulette(wheelPrefab, ballPrefab, rouletteConfig, rouletteParent,eventBus);
            container.BindSingleton(context);
            
            // Create and bind the wheel service.
            var wheelService = new RouletteWheelService(context);
            container.BindSingleton<IRouletteWheelService>(wheelService);
            
            // Create and bind the ball service.
            var ballService = new RouletteBallService(context, wheelService,eventBus);
            container.BindSingleton<IRouletteBallService>(ballService);
            
            // Optionally, bind outcome and collider services.
            var outcomeService = new RouletteOutcomeService();
            container.BindSingleton<IRouletteOutcomeService>(outcomeService);
            
            var colliderService = new RouletteColliderService();
            container.BindSingleton<IRouletteColliderService>(colliderService);
            
            // Instantiate and bind the RouletteManager.
            var manager = new RouletteManager(wheelService, ballService, eventBus);
            container.BindSingleton(manager);
            
            // Instantiate the ManagerRunner and register the manager.
            var managerRunnerInstance = Instantiate(managerRunnerPrefab);
            managerRunnerInstance.Register(manager);
        }
    }
}
