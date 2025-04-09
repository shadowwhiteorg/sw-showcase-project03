using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Core.Constants;
using _Game.Enums;
using _Game.Systems.BetSystem;
using UnityEngine;

namespace _Game.Utils.Helpers
{
    public static class BetHelpers
    {
        public static void AddStreetBet(TableElement[] elements, ref List<BetArea> generatedBetAreas)
        {
            BetAreaData betAreaData = new BetAreaData(
                type: BetType.Street,
                positions: elements.Select(e => e.Position).ToArray(),
                numbers: elements.SelectMany(e => ParseNumbers(e.Value)).ToArray(),
                sourceObjects: elements.Select(e => e.gameObject).ToArray(),
                payout: PayoutConstants.StreetPayout
            );
            generatedBetAreas.Add(new BetArea(
                data: betAreaData
            ));
        }
        
        public static bool IsConsecutiveStreet(TableElement[] elements, int startIndex, int count, float tolerance)
        {
            for (int i = 1; i < count; i++)
            {
                float expectedX = elements[startIndex].Position.x + i * tolerance;
                if (Mathf.Abs(elements[startIndex + i].Position.x - expectedX) > tolerance * 0.5f)
                    return false;
            }

            return true;
        }
        
        public static int GetOutsidePayout(BetType type) => type switch
        {
            BetType.Dozen or BetType.Column => PayoutConstants.DozenPayout,
            BetType.Red or BetType.Black or BetType.Even or BetType.Odd or BetType.Low or BetType.High => 1,
            _ => 0
        };
        
        public static (int[] numbers, BetType type) ParseOutsideBet(TableElement element, float maxX1, float maxX2)
        {
            string value = element.Value.ToLower().Trim();

            return value switch
            {
                "1st12" or "1st 12" => (Enumerable.Range(1, 12).ToArray(), BetType.Dozen),
                "2nd12" or "2nd 12" => (Enumerable.Range(13, 12).ToArray(), BetType.Dozen),
                "3rd12" or "3rd 12" => (Enumerable.Range(25, 12).ToArray(), BetType.Dozen),
                "1 to 18" or "1-18" => (Enumerable.Range(1, 18).ToArray(), BetType.Low),
                "19 to 36" or "19-36" => (Enumerable.Range(19, 18).ToArray(), BetType.High),
                "even" => (GetEvenNumbers(), BetType.Even),
                "odd" => (GetOddNumbers(), BetType.Odd),
                "red" => (GetRedNumbers(), BetType.Red),
                "black" => (GetBlackNumbers(), BetType.Black),
                "2 to 1" or "2:1" => (GetColumnNumbers(element, maxX1, maxX2), BetType.Column),
                _ => (Array.Empty<int>(), BetType.None)
            };
        }
        private static int[] GetColumnNumbers(TableElement element, float maxX1, float maxX2)
        {
            float x = element.Position.x;
            if (x < maxX1) return new[] { 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34 };
            if (x < maxX2) return new[] { 2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35 };
            return new[] { 3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36 };
        }
        private static int[] GetEvenNumbers() => Enumerable.Range(1, 36).Where(n => n % 2 == 0).ToArray();
        private static int[] GetOddNumbers() => Enumerable.Range(1, 36).Where(n => n % 2 == 1).ToArray();

        private static int[] GetRedNumbers() => new[]
        {
            1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36
        };

        private static int[] GetBlackNumbers() => new[]
        {
            2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35
        };
        
       
        public  static int[] ParseNumbers(string value)
        {
            if (value == "00") return new[] { -1 };
            if (value == "0") return new[] { 0 };
            return int.TryParse(value, out int num) ? new[] { num } : Array.Empty<int>();
        }
        
        
        public static bool IsNumber(string value) =>
            value == "00" || value == "0" || int.TryParse(value, out _);
    }
}