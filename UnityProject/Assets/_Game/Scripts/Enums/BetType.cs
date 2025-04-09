using UnityEngine;

namespace _Game.Enums
{
    public enum BetType
    {
        Straight = 0,   // Single number
        Split = 1,      // 2 adjacent numbers
        Street = 2,     // 3 numbers in a row
        Corner = 3,     // 4-number block
        Line = 4,       // 6-number block
        Column = 5,     // 12-number vertical
        Dozen = 6,      // 1-12, 13-24, etc.
        RedBlack = 7,
        EvenOdd = 8,
        HighLow = 9,
        None = 10,
        Red = 11,
        Black = 12,
        Even = 13,
        Odd = 14,
        Low = 15,
        High = 16
    }
}