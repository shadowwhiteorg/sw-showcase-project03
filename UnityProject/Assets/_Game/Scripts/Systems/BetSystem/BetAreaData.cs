using System;
using _Game.Enums;
using UnityEngine;

namespace _Game.Systems.BetSystem
{
    [Serializable]
    public class BetAreaData
    {
        public BetType Type;
        public Vector3[] Positions;
        public int[] Numbers;
        public GameObject[]  SourceObjects;
        public int Payout;
        public string SpecialType;

        public BetAreaData(BetType type, Vector3[] positions, int[] numbers, GameObject[] sourceObjects, int payout,  string specialType=null)
        {
            Type = type;
            Positions = positions;
            Numbers = numbers;
            SourceObjects = sourceObjects;
            Payout = payout;
            if(specialType!=null)
                SpecialType = specialType;
        }
    }
}