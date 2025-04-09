using System;
using System.Collections.Generic;
using System.Linq;
using _Game.Enums;
using UnityEngine;

namespace _Game.Systems.BetSystem
{
    [Serializable]
    public class BetArea
    {
        public BetAreaData Data;
        public BetArea(BetAreaData data)
        {
            Data = data;

        }
    }
}