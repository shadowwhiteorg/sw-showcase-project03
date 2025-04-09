using System.Collections.Generic;
using System.Linq;
using _Game.Core.Events;
using _Game.Interfaces;
using UnityEngine;
using _Game.Utils;
using _Game.Systems.BetSystem;

namespace _Game.Systems.ChipSystem
{
    public class ChipManager : MonoBehaviour
    {
        [Header("Chip Configuration")]
        [SerializeField] private List<ChipData> chipDatas;
        [SerializeField] private Transform spawnArea;
        [SerializeField] private float chipSpacing;

        private ObjectPool<Chip> _chipPool;
        private ChipData _defaultChipData;
        private IEventBus _eventBus;
        private ChipStack _currentStack;
        private List<Chip> _activeChips = new List<Chip>();

        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<ToBettingEvent>(e => InitializeStack());
            _currentStack = new GameObject("ChipStack").AddComponent<ChipStack>();
            _currentStack.SetChipSpacing(chipSpacing);
            _currentStack.transform.position = spawnArea.position;
            _defaultChipData = chipDatas.FirstOrDefault();
            var mCollider = _currentStack.gameObject.AddComponent<BoxCollider>();
            mCollider.size = new Vector3(1f, 0.1f, 1f);
            SubscribeToEvents();
            if (_defaultChipData == null || _defaultChipData.Prefab == null)
            {
                Debug.LogError("ChipManager: Default chip prefab is missing.");
                return;
            }
            _chipPool = new ObjectPool<Chip>(_defaultChipData.Prefab.GetComponent<Chip>(), 20, this.transform);
        }

        private void InitializeStack()
        {
            _activeChips.Clear();
            _currentStack = new GameObject("ChipStack").AddComponent<ChipStack>();
            _currentStack.transform.position = spawnArea.position;
            var mCollider = _currentStack.gameObject.AddComponent<BoxCollider>();
            mCollider.size = new Vector3(1f, 0.1f, 1f);
            UpdateStackCollider();
        }
        
        private void SubscribeToEvents()
        {
            _eventBus.Subscribe<IncreaseBetEvent>(e => PushChip(e.Amount));
            _eventBus.Subscribe<DecreaseBetEvent>(e => TryRemoveChipsForAmount(e.Amount));
        }
        
        
        private void UpdateStackCollider()
        {
            if (_currentStack.TryGetComponent<BoxCollider>(out var collider))
            {
                float height = _activeChips.Count * chipSpacing;
                collider.center = new Vector3(0, height / 2f, 0);
                collider.size = new Vector3(1f, height, 1f);
            }
        }

        public void PushChip(int denomination)
        {
            ChipData data = chipDatas.FirstOrDefault(cd => cd.Denomination == denomination);
            if (data == null)
            {
                Debug.LogWarning($"No ChipData found for denomination {denomination}");
                return;
            }

            Chip chip = _chipPool.Get();
            chip.Initialize(data);
            _activeChips.Add(chip);
            _currentStack.PushChip(chip);
            _eventBus.Fire(new ChipCreatedEvent());
        }

        public bool RemoveChipByDenomination(int denomination)
        {
            Chip chip = _currentStack.RemoveChipByDenomination(denomination);
            if (chip == null) return false;
            _activeChips.Remove(chip);
            _chipPool.Return(chip);
            return true;
        }

        public void ResetStack( ChipStack stack)
        {
            _activeChips.Clear();
            foreach (var chip in stack.Chips)
                _chipPool.Return(chip);

            stack.Clear();
        }

        public void PushMultipleChipsForAmount(int amount)
        {
            var denominations = CalculateDenominations(amount);
            foreach (int denom in denominations)
                PushChip(denom);
        }

        public bool HasChipInStack(int amount)
        {
            var denominations = CalculateDenominations(amount);

            // Dry-run: Check if all required chips are available
            Dictionary<int, int> denomCounts = denominations
                .GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());

            foreach (var pair in denomCounts)
            {
                int available = _currentStack.Chips.Count(c => c.Denomination == pair.Key);
                if (available < pair.Value)
                    return false;
            }
            return true;
        }

        public bool TryRemoveChipsForAmount(int amount)
        {
            var denominations = CalculateDenominations(amount);

            // Dry-run: Check if all required chips are available
            Dictionary<int, int> denomCounts = denominations
                .GroupBy(d => d).ToDictionary(g => g.Key, g => g.Count());

            foreach (var pair in denomCounts)
            {
                int available = _currentStack.Chips.Count(c => c.Denomination == pair.Key);
                if (available < pair.Value)
                    return false;
            }

            // Perform actual removals
            foreach (int denom in denominations)
                RemoveChipByDenomination(denom);

            return true;
        }

        public List<int> CalculateDenominations(int amount)
        {
            List<int> result = new();
            var sorted = chipDatas
                .Select(cd => cd.Denomination)
                .OrderByDescending(v => v);

            foreach (var denom in sorted)
            {
                while (amount >= denom)
                {
                    amount -= denom;
                    result.Add(denom);
                }
            }

            return result;
        }
    }
}
