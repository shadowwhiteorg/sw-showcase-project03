using _Game.Core.DI;
using _Game.Interfaces;
using _Game.Systems.FinanceSystem;
using _Game.Systems.UISystem.Models;
using _Game.Systems.UISystem.Screens;
using _Game.Systems.UISystem.Views;
using UnityEngine;

namespace _Game.Systems.UISystem
{
    public class UIInstaller : MonoBehaviour
    {
        [Header("Betting UI")]
        [SerializeField] private GameObject bettingUIPrefab;
        [Header("Bet Area UI")]
        [SerializeField] private GameObject betAreaUIPrefab;
        [Header("Start UI")]
        [SerializeField] private GameObject startUIPrefab;
        [Header("Table UI")]
        [SerializeField] private GameObject tableUIPrefab;
        [Header("Roulette UI")]
        [SerializeField] private GameObject rouletteUIPrefab;
        [Header("Finance UI")]
        [SerializeField] private GameObject financeUIPrefab;


        public void Install(DIContainer container, IEventBus eventBus)
        {
            // Install UI systems
            InstallBettingUI(container, eventBus);
            InstallBetAreaUI(container, eventBus);
            InstallStartUI(container, eventBus);
            InstallTabletUI(container, eventBus);
            InstallRouletteUI(container, eventBus);
            InstallFinanceUI(container, eventBus);
        }

        private void InstallBettingUI(DIContainer container, IEventBus eventBus)
        {
            // 1. Instantiate UI prefab
            var bettingUIInstance = Instantiate(bettingUIPrefab);
            DontDestroyOnLoad(bettingUIInstance);

            // 2. Get components
            var view = bettingUIInstance.GetComponentInChildren<BettingUIView>();
            var screen = bettingUIInstance.GetComponentInChildren<BettingUIScreen>();

            // 3. Create and bind model
            var model = new BettingUIModel();
            container.BindSingleton(model);

            // 4. Bind view and screen
            container.BindSingleton(view);
            container.BindSingleton(screen);

            // 5. Manual construction (for MonoBehaviour-based screens)
            screen.Construct(model, view, eventBus);
            
            container.Resolve<BettingUIScreen>().Show();
        }

        private void InstallBetAreaUI(DIContainer container, IEventBus eventBus)
        {
            var betAreaUIInstance = Instantiate(betAreaUIPrefab);
            DontDestroyOnLoad(betAreaUIInstance);
            var view = betAreaUIInstance.GetComponentInChildren<BetAreaUIView>();
            var screen = betAreaUIInstance.GetComponentInChildren<BetAreaUIScreen>();
            var model = new BetAreaUIModel();
            container.BindSingleton(model);
            container.BindSingleton(view);
            container.BindSingleton(screen);
            screen.Construct(model, view, eventBus);
            
        }

        private void InstallStartUI(DIContainer container, IEventBus eventBus)
        {
            var startUIInstance = Instantiate(startUIPrefab);
            DontDestroyOnLoad(startUIInstance);
            var view = startUIInstance.GetComponentInChildren<StartUIView>();
            var screen = startUIInstance.GetComponentInChildren<StartUIScreen>();
            var model = new StartUIModel();
            container.BindSingleton(model);
            container.BindSingleton(view);
            container.BindSingleton(screen);
            screen.Construct(model, view, eventBus);
            // container.Resolve<StartUIScreen>().Show();
        }

        private void InstallTabletUI(DIContainer container, IEventBus eventBus)
        {
            var tableUIInstance = Instantiate(tableUIPrefab);
            DontDestroyOnLoad(tableUIInstance);
            var view = tableUIInstance.GetComponentInChildren<TableUIView>();
            var screen = tableUIInstance.GetComponentInChildren<TableUIScreen>();
            var model = new TableUIModel();
            container.BindSingleton(model);
            container.BindSingleton(view);
            container.BindSingleton(screen);
            screen.Construct(model, view, eventBus);
        }
        
        private void InstallRouletteUI(DIContainer container, IEventBus eventBus)
        {
            var rouletteUIInstance = Instantiate(rouletteUIPrefab);
            DontDestroyOnLoad(rouletteUIInstance);
            var view = rouletteUIInstance.GetComponentInChildren<RouletteUIView>();
            var screen = rouletteUIInstance.GetComponentInChildren<RouletteUIScreen>();
            var model = new RouletteUIModel();
            container.BindSingleton(model);
            container.BindSingleton(view);
            container.BindSingleton(screen);
            screen.Construct(model, view, eventBus);
        }
        
        private void InstallFinanceUI(DIContainer container, IEventBus eventBus)
        {
            var financeUIInstance = Instantiate(financeUIPrefab);
            
            FinancialDataManager financeDataManager = new FinancialDataManager();
            
            var model = new FinanceUIModel(financeDataManager);
            var view = financeUIInstance.GetComponentInChildren<FinanceUIView>();
            var screen = financeUIInstance.GetComponentInChildren<FinanceUIScreen>();
            
            container.BindSingleton(model);
            container.BindSingleton(view);
            container.BindSingleton(screen);
            screen.Construct(model, view, eventBus);
        }
        
        
        private T InstantiateFromPath<T>(string path) where T : Component
        {
            var prefab = Resources.Load<GameObject>(path);
            return Object.Instantiate(prefab).GetComponent<T>();
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (bettingUIPrefab != null)
            {
                if (bettingUIPrefab.GetComponentInChildren<BettingUIView>() == null)
                    Debug.LogError("BettingUI prefab missing BettingUIView component!");
            
                if (bettingUIPrefab.GetComponentInChildren<BettingUIScreen>() == null)
                    Debug.LogError("BettingUI prefab missing BettingUIScreen component!");
            }
        }
#endif
    }
        
}
