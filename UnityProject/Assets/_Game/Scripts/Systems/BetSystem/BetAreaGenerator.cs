using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Core.Constants;
using _Game.Core.Events;
using _Game.Enums;
using _Game.Interfaces;
using _Game.Systems.ChipSystem;
using _Game.Utils.Helpers;
using UnityEngine;

namespace _Game.Systems.BetSystem
{
    public class BetAreaGenerator : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private float positionTolerance = 0.1f;
        [SerializeField] private bool debugDrawGizmos = true;

        [Header("Column Settings")] [SerializeField]
        private float column1MaxX = -1f;

        [SerializeField] private float column2MaxX = 1f;

        [Header("Intersection Settings")] [SerializeField]
        private float numberSplitTolerance = 0.5f;
        [SerializeField] private float otherSplitTolerance = 0.7f;


        private List<BetArea> _generatedBetAreas = new();
        private TableElement[] _elements;
        private TableElement[] _numberElements;
        private IEventBus _eventBus;

        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<GameStartedEvent>(e=>GenerateBetAreas());
        }
        
        public List<BetArea> GenerateBetAreas()
        {
            _generatedBetAreas.Clear();
            _elements = FindObjectsByType<TableElement>(0, 0)
                .Where(e => !string.IsNullOrEmpty(e.Value))
                .ToArray();

            var numberElements = _elements.Where(e => BetHelpers.IsNumber(e.Value)).ToArray();
            var outsideBetElements = _elements.Except(numberElements).ToArray();

            GenerateStraightBets(numberElements);
            GenerateStreetBets(numberElements);
            GenerateOutsideBets(outsideBetElements);
            GenerateSplitBets(_elements);
            BetAreaFinder.Initialize(_generatedBetAreas);

            // Debug.Log($"Generated {_generatedBetAreas.Count} bet areas");
            return _generatedBetAreas;
        }

        #region Bet Generation Methods

        private void GenerateStraightBets(TableElement[] numberElements)
        {
            foreach (var element in numberElements)
            {
                BetAreaData betAreaData = new BetAreaData(
                    type: BetType.Straight,
                    positions: new[] { element.Position },
                    numbers: BetHelpers.ParseNumbers(element.Value),
                    sourceObjects: new[] { element.gameObject },
                    payout: PayoutConstants.StraightPayout
                );
                _generatedBetAreas.Add(new BetArea(
                    data: betAreaData
                    
                ));
            }
        }

        private void GenerateSplitBets(TableElement[] allElements)
        {
            var numberElements = allElements.Where(e => BetHelpers.IsNumber(e.Value)).ToArray();
            var outsideElements = allElements.Except(numberElements).ToArray();

            // Check splits between numbers
            for (int i = 0; i < numberElements.Length; i++)
            {
                for (int j = i + 1; j < numberElements.Length; j++)
                {
                    if (Vector3.Distance(numberElements[i].Position, numberElements[j].Position) <= numberSplitTolerance)
                    {
                        Vector3 centerPos = (numberElements[i].Position + numberElements[j].Position) / 2;
                        BetAreaData betAreaData = new BetAreaData(
                            type: BetType.Split,
                            positions: new[] { centerPos },
                            numbers: BetHelpers.ParseNumbers(numberElements[i].Value).Concat(BetHelpers.ParseNumbers(numberElements[j].Value))
                                .ToArray(),
                            sourceObjects: new[] { numberElements[i].gameObject, numberElements[j].gameObject },
                            payout: PayoutConstants.SplitPayout
                        );
                        _generatedBetAreas.Add(new BetArea(
                            data: betAreaData
                        ));
                    }
                }
            }

            GenerateOutsideToOutsideSplits(outsideElements);
        }

        private void GenerateOutsideToOutsideSplits(TableElement[] outsideElements)
        {
            // Create a spatial lookup for fast neighbor detection
            var spatialLookup = outsideElements
                .Select(e => new
                {
                    Element = e,
                    Key = GetSpatialKey(e.Position)
                })
                .ToLookup(x => x.Key, x => x.Element);

            foreach (var element in outsideElements)
            {
                // Check all 8 possible neighbor directions
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dz = -1; dz <= 1; dz++)
                    {
                        // if (dx == 0 && dz == 0) continue; // Skip self

                        Vector3 neighborPos = element.Position +
                                              new Vector3(dx * otherSplitTolerance, 0, dz * otherSplitTolerance);

                        string neighborKey = GetSpatialKey(neighborPos);

                        foreach (var neighbor in spatialLookup[neighborKey])
                        {
                            if (ShouldFormSplit(element, neighbor))
                            {
                                AddOutsideSplitBet(
                                    element,
                                    neighbor,
                                    $"OutsideSplit-{element.Value}-{neighbor.Value}"
                                );
                            }
                        }
                    }
                }
            }
        }

        private string GetSpatialKey(Vector3 position)
        {
            // Quantize position to grid cells for lookup
            int x = Mathf.RoundToInt(position.x / otherSplitTolerance);
            int z = Mathf.RoundToInt(position.z / otherSplitTolerance);
            return $"{x},{z}";
        }

        private bool ShouldFormSplit(TableElement a, TableElement b)
        {
            // Prevent duplicates and self-matches
            if (a.GetInstanceID() >= b.GetInstanceID()) return false;
            // Debug.Log($"Checking split between {a.Value} and {b.Value}");
            // Check if these outside bets should logically form a split
            return AreOutsideBetsAdjacent(a.Value, b.Value) ;
        }

        private bool AreOutsideBetsAdjacent(string valueA, string valueB)
        {
            // Define valid outside bet pairings
            var validPairs = new Dictionary<string, string[]>
            {
                ["1st 12"] = new[] { "2nd 12", "1 to 18", "Even" },
                ["2nd 12"] = new[] { "Red", "Black","3rd 12" },
                ["3rd 12"] = new[] { "Odd", "19 to 36" },
                ["1 to 18"] = new[] { "Even" },
                ["Odd"] = new[] { "19 to 36" },
                // TODO: Solve tolerance issues
                // ["2 to 1"] = new[] { "2 to 1" },
                
            };

            return validPairs.ContainsKey(valueA) && validPairs[valueA].Contains(valueB) ||
                   validPairs.ContainsKey(valueB) && validPairs[valueB].Contains(valueA);
        }

        private void AddOutsideSplitBet(TableElement a, TableElement b, string splitType)
        {
            var centerPos = (a.Position + b.Position) / 2;
            BetAreaData betAreaData = new BetAreaData(
                type: BetType.Split,
                positions: new[] { centerPos },
                numbers: CombineOutsideNumbers(a.Value, b.Value),
                sourceObjects: new[] { a.gameObject, b.gameObject },
                payout: CalculateOutsideSplitPayout(a.Value, b.Value),
                specialType: splitType
            );
            _generatedBetAreas.Add(new BetArea(
                data: betAreaData
            ));
            // Debug.Log($"Added outside split bet: {splitType} between {a.Value} and {b.Value}");
        }

        private int[] CombineOutsideNumbers(string valueA, string valueB)
        {
            // Combine the number ranges of both outside bets
            var numbersA = GetOutsideBetNumbers(valueA);
            var numbersB = GetOutsideBetNumbers(valueB);
            return numbersA.Concat(numbersB).Distinct().OrderBy(n => n).ToArray();
        }

        private int CalculateOutsideSplitPayout(string valueA, string valueB)
        {
            // Custom payout rules for special outside splits
            return (valueA + valueB) switch
            {
                var s when s.Contains("12") && s.Contains("to18") => 5, // Dozen+Low
                var s when s.Contains("Red") && s.Contains("Even") => 3,
                _ => 2 // Default outside split payout
            };
        }
        
        private int[] GetOutsideBetNumbers(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Array.Empty<int>();

            value = value.Trim().ToLower();

            return value switch
            {
                // Dozens
                "1st12" or "1st 12" => Enumerable.Range(1, 12).ToArray(),
                "2nd12" or "2nd 12" => Enumerable.Range(13, 12).ToArray(),
                "3rd12" or "3rd 12" => Enumerable.Range(25, 12).ToArray(),

                // Columns
                "2to1" or "2:1" or "2 to 1" => GetColumnNumbers(value),

                // Halves
                "1to18" or "1-18" => Enumerable.Range(1, 18).ToArray(),
                "19to36" or "19-36" => Enumerable.Range(19, 18).ToArray(),

                // Even/Odd
                "even" => Enumerable.Range(1, 36).Where(n => n % 2 == 0).ToArray(),
                "odd" => Enumerable.Range(1, 36).Where(n => n % 2 == 1).ToArray(),

                // Colors
                "red" => new[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 },
                "black" => new[] { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 },

                // Special cases
                "0" => new[] { 0 },
                "00" => new[] { -1 }, // Using -1 to represent 00

                _ => Array.Empty<int>()
            };
        }

        private int[] GetColumnNumbers(string columnIdentifier)
        {
            return columnIdentifier switch
            {
                _ => new[] { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 }
            };
        }


        private void GenerateStreetBets(TableElement[] numberElements)
        {
            var rowGroups = numberElements
                .GroupBy(e => Mathf.RoundToInt(e.Position.z / positionTolerance));

            foreach (var group in rowGroups.Where(g => g.Count() >= 3))
            {
                var sorted = group.OrderBy(e => e.Position.x).ToArray();
                for (int i = 0; i <= sorted.Length - 3; i++)
                {
                    if (BetHelpers.IsConsecutiveStreet(sorted, i, 3,positionTolerance ))
                    {
                        var streetElements = new[] { sorted[i], sorted[i + 1], sorted[i + 2] };
                        BetHelpers.AddStreetBet(streetElements,ref _generatedBetAreas);
                    }
                }
            }
        }


        private void GenerateOutsideBets(TableElement[] outsideBetElements)
        {
            foreach (var element in outsideBetElements)
            {
                var (numbers, betType) = BetHelpers.ParseOutsideBet(element, column1MaxX, column2MaxX);

                if (numbers.Length > 0)
                {
                    BetAreaData betAreaData = new BetAreaData(
                        type: betType,
                        positions: new[] { element.Position },
                        numbers: numbers,
                        sourceObjects: new[] { element.gameObject },
                        payout: BetHelpers.GetOutsidePayout(betType)
                    );
                    _generatedBetAreas.Add(new BetArea(
                        data: betAreaData
                    ));
                }
            }
        }

        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!debugDrawGizmos || _generatedBetAreas == null) return;

            foreach (var area in _generatedBetAreas)
            {
                GizmoEditorHelpers.DrawAreaGizmo(area);
            }
        }
#endif
    }
}
