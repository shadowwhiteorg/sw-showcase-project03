using _Game.Core.DI;
using _Game.Core.Events;
using _Game.Core.Singletons;
using _Game.Interfaces;
using _Game.Systems.AudioSystem;
using _Game.Systems.BetSystem;
using _Game.Systems.ChipSystem;
using _Game.Systems.FinanceSystem;
using _Game.Systems.PayoutSystem;
using UnityEngine;

namespace _Game.Systems
{
    public class GameInstaller : MonoBehaviour
    {
        [Header("Singletons")] [SerializeField]
        private ChipManager chipManagerPrefab;

        [SerializeField] private ChipDragger chipDraggerPrefab;
        [SerializeField] private StateManager stateManagerPrefab;
        [SerializeField] private BetAreaGenerator betAreaGeneratorPrefab;
        [SerializeField] private BetHighlightManager betHighlightManagerPrefab;
        [SerializeField] private AudioManager audioManagerPrefab;

        private FinancialService _financeSystem;
        private ActiveBetsManager _activeBetsManager;
        private ChipManager _chipManager;

        public void Install(DIContainer container, IEventBus eventBus)
        {
            InstallChipManager(container, eventBus);
            InstallChipDragger(container, eventBus);
            InstallStateManager(container, eventBus);
            InstallBetAreaGenerator(container, eventBus);
            InstallFinancialSystem(container, eventBus);
            InstallActiveBetsManager(container, eventBus);
            InstallBetPayoutCalculator(container, eventBus);
            InstallPayoutIntegrator(container, eventBus);
            InstallRoundManager(container, eventBus);
            InstallBetHighlightManager(container, eventBus);
            InstallAudioManager(container, eventBus);
        }

        private void InstallChipManager(DIContainer container, IEventBus eventBus)
        {
            // 1. Instantiate ChipManager prefab
            _chipManager = Instantiate(chipManagerPrefab);
            // If needed!
            // DontDestroyOnLoad(chipManagerInstance);

            // 2. Bind ChipManager to DI container
            container.BindSingleton(_chipManager);

            // 3. Construct ChipManager with event bus
            _chipManager.Construct(eventBus);

            eventBus.Fire(new ChipSystemInitialized(_chipManager));

        }

        private void InstallChipDragger(DIContainer container, IEventBus eventBus)
        {
            var chipDraggerInstance = Instantiate(chipDraggerPrefab);
            DontDestroyOnLoad(chipDraggerInstance);
            container.BindSingleton(chipDraggerInstance);
            chipDraggerInstance.Construct(eventBus, _chipManager);
        }

        private void InstallStateManager(DIContainer container, IEventBus eventBus)
        {
            var stateManagerInstance = Instantiate(stateManagerPrefab);
            DontDestroyOnLoad(stateManagerInstance);
            container.BindSingleton(stateManagerInstance);
            stateManagerInstance.Construct(eventBus);
        }

        private void InstallBetAreaGenerator(DIContainer container, IEventBus eventBus)
        {
            var betAreaGeneratorInstance = Instantiate(betAreaGeneratorPrefab);
            DontDestroyOnLoad(betAreaGeneratorInstance);
            container.BindSingleton(betAreaGeneratorInstance);
            betAreaGeneratorInstance.Construct(eventBus);
        }

        private void InstallFinancialSystem(DIContainer container, IEventBus eventBus)
        {
            var financialDataManager = new FinancialDataManager();
            container.BindSingleton(financialDataManager);
            _financeSystem = new FinancialService(financialDataManager, eventBus);
            container.BindSingleton<IFinancialService>(_financeSystem);
            eventBus.Fire(new FinanceSystemInitialized(_financeSystem));
        }

        private void InstallActiveBetsManager(DIContainer container, IEventBus eventBus)
        {
            _activeBetsManager = new GameObject("ActiveBetsManager").AddComponent<ActiveBetsManager>();
            DontDestroyOnLoad(_activeBetsManager);
            container.BindSingleton(_activeBetsManager);
            _activeBetsManager.Construct(eventBus);
        }

        private void InstallBetPayoutCalculator(DIContainer container, IEventBus eventBus)
        {
            var betPayoutCalculator = new GameObject("BetPayoutCalculator").AddComponent<BetPayoutCalculator>();
            DontDestroyOnLoad(betPayoutCalculator);
            container.BindSingleton(betPayoutCalculator);
            betPayoutCalculator.Construct(eventBus, container.Resolve<ActiveBetsManager>());
        }

        private void InstallPayoutIntegrator(DIContainer container, IEventBus eventBus)
        {
            var payoutIntegrator = new GameObject("PayoutIntegrator").AddComponent<PayoutIntegrator>();
            DontDestroyOnLoad(payoutIntegrator);
            container.BindSingleton(payoutIntegrator);
            payoutIntegrator.Construct(eventBus, _financeSystem);
        }

        private void InstallRoundManager(DIContainer container, IEventBus eventBus)
        {
            var roundManager = new GameObject("RoundManager").AddComponent<RoundManager>();
            DontDestroyOnLoad(roundManager);
            container.BindSingleton(roundManager);
            roundManager.Construct(eventBus, _activeBetsManager);
        }

        private void InstallBetHighlightManager(DIContainer container, IEventBus eventBus)
        {
            var betHighlightManagerInstance = Instantiate(betHighlightManagerPrefab);
            container.BindSingleton(betHighlightManagerInstance);
            betHighlightManagerInstance.Construct(eventBus);
        }

        private void InstallAudioManager(DIContainer container, IEventBus eventBus)
        {
            var audioManagerInstance = Instantiate(audioManagerPrefab);
            container.BindSingleton(audioManagerInstance);
            audioManagerInstance.Construct(eventBus);
        }
    }
}