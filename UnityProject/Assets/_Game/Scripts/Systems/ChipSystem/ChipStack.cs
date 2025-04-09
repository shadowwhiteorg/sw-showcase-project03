using System.Collections.Generic;
using UnityEngine;

namespace _Game.Systems.ChipSystem
{
    public class ChipStack : MonoBehaviour
    {
        private float _chipSpacing = 0.1f;
        private readonly List<Chip> _chips = new();
        public IReadOnlyList<Chip> Chips => _chips;

        public void SetChipSpacing(float chipSpacing)
        {
            _chipSpacing = 0.1f;
        }
        
        public void PushChip(Chip chip)
        {
            chip.SetStack(this);
            chip.transform.SetParent(transform);
            _chips.Add(chip);
            UpdateVisuals();
        }

        public Chip RemoveChipByDenomination(int denomination)
        {
            int index = _chips.FindIndex(c => c.Denomination == denomination);
            if (index == -1) return null;

            Chip chipToRemove = _chips[index];
            _chips.RemoveAt(index);
            UpdateVisuals();
            return chipToRemove;
        }

        public void Clear()
        {
            foreach (Chip chip in _chips)
            {
                chip.SetStack(null);
                chip.transform.SetParent(null);
                chip.gameObject.SetActive(false);
            }
            _chips.Clear();
        }

        public int GetTotalValue()
        {
            int total = 0;
            foreach (var chip in _chips)
                total += chip.Denomination;

            return total;
        }

        public void UpdateVisuals()
        {
            for (int i = 0; i < _chips.Count; i++)
            {
                var chip = _chips[i];
                chip.transform.localPosition = Vector3.up * i * _chipSpacing;
            }
        }
    }
}