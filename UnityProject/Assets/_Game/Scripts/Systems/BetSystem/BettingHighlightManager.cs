using System.Collections.Generic;
using UnityEngine;
using _Game.Core.Events;
using _Game.Interfaces;
using _Game.Systems.BetSystem;
using _Game.Utils; // Uses the project's ObjectPool<T>

namespace _Game.Systems.BetSystem
{
    public class BetHighlightManager : MonoBehaviour
    {
        [Header("Highlight Settings")]
        [Tooltip("Prefab for highlighting a number. Ensure this prefab contains a BetHighlight component.")]
        [SerializeField] private BetHighlight highlightPrefab;

        private ObjectPool<BetHighlight> highlightPool;
        // Active highlights indexed by number (use -1 for "00")
        private Dictionary<int, BetHighlight> activeHighlights = new Dictionary<int, BetHighlight>();
        // Cache mapping from number to its TableElement (assumes one TableElement per number)
        private Dictionary<int, TableElement> tableElementCache = new Dictionary<int, TableElement>();
        private IEventBus eventBus;

        // Keep track of the currently active bet area for which the highlights are shown
        private BetArea currentBetArea = null;

        /// <summary>
        /// Initializes the manager, caching TableElements and subscribing to events.
        /// </summary>
        public void Construct(IEventBus eventBus)
        {
            this.eventBus = eventBus;
            // Initialize the pool using the project's ObjectPool (set an initial size, e.g., 20)
            highlightPool = new ObjectPool<BetHighlight>(highlightPrefab, 20, this.transform);

            // Cache all TableElement instances (use true to also include inactive objects)
            List<TableElement> tableElements = new List<TableElement>();
            tableElements.AddRange(FindObjectsByType<TableElement>(0,0));
            foreach (TableElement te in tableElements)
            {
                string value = te.Value.Trim();
                int number;
                if (value == "00")
                {
                    number = -1;
                }
                else if (int.TryParse(value, out number))
                {
                    if (!tableElementCache.ContainsKey(number))
                    {
                        tableElementCache[number] = te;
                    }
                }
            }

            // Subscribe to relevant events.
            eventBus.Subscribe<BetPlacedEvent>(OnBetPlaced);
            eventBus.Subscribe<BetsClearedEvent>(OnBetsCleared);
            eventBus.Subscribe<ChipDragEvent>(OnChipDrag);
        }

        /// <summary>
        /// Called when the player is dragging a chip over a bet area.
        /// If the hovered bet area changes, clear previous highlights and show new ones.
        /// </summary>
        private void OnChipDrag(ChipDragEvent evt)
        {
            // If no valid bet area is hovered, clear any existing highlights.
            if (evt.ClosestArea == null)
            {
                ClearHighlights();
                currentBetArea = null;
                return;
            }

            // If the hovered bet area is different than the current one, update highlights.
            if (currentBetArea != evt.ClosestArea)
            {
                ClearHighlights();
                currentBetArea = evt.ClosestArea;
                ShowHighlightsForBetArea(currentBetArea);
            }
        }

        /// <summary>
        /// Called when a bet is placed.
        /// Clears highlights if necessary and shows highlights for the bet's area.
        /// </summary>
        private void OnBetPlaced(BetPlacedEvent evt)
        {
            // Here you may use an appropriate method to determine the bet area's identity.
            // For this example, we assume that the current hovered bet area is used.
            // If the bet's target area differs from currentBetArea, update accordingly.
            // (Extend this logic if your Bet type holds a reference to its originating BetArea.)
            if (currentBetArea != null)
            {
                // For simplicity, we clear and reuse currentBetArea.
                ClearHighlights();
            }
            // Set current bet area to the one used in this bet.
            currentBetArea = evt.Bet is Bet ? currentBetArea : null;
            // Display highlights for this bet area.
            ShowHighlightsForBetArea(currentBetArea);
        }

        /// <summary>
        /// Displays highlight prefabs on every table element corresponding to the bet area's numbers.
        /// </summary>
        private void ShowHighlightsForBetArea(BetArea betArea)
        {
            if (betArea == null)
                return;

            foreach (int number in betArea.Data.Numbers)
            {
                if (activeHighlights.ContainsKey(number))
                    continue;

                if (tableElementCache.TryGetValue(number, out TableElement targetElement))
                {
                    // Get a highlight object from the pool.
                    BetHighlight highlight = highlightPool.Get();
                    // Parent the highlight to the target TableElement and reset its local position.
                    highlight.transform.SetParent(targetElement.transform, false);
                    highlight.transform.localPosition = Vector3.zero;
                    activeHighlights[number] = highlight;
                }
            }
        }

        /// <summary>
        /// Clears all current highlights by returning them to the pool.
        /// </summary>
        private void ClearHighlights()
        {
            foreach (var kv in activeHighlights)
            {
                highlightPool.Return(kv.Value);
            }
            activeHighlights.Clear();
        }

        /// <summary>
        /// Called when bets are cleared (e.g., at the start of a new round).
        /// Clears all highlights and resets the current bet area.
        /// </summary>
        private void OnBetsCleared(BetsClearedEvent evt)
        {
            ClearHighlights();
            currentBetArea = null;
        }

        private void OnDestroy()
        {
            if (eventBus != null)
            {
                eventBus.Unsubscribe<BetPlacedEvent>(OnBetPlaced);
                eventBus.Unsubscribe<BetsClearedEvent>(OnBetsCleared);
                eventBus.Unsubscribe<ChipDragEvent>(OnChipDrag);
            }
        }
    }
}
