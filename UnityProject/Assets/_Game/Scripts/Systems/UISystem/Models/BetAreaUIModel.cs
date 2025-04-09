using _Game.Enums;
using _Game.Systems.BetSystem;
using _Game.Systems.ChipSystem;
using UnityEngine;

namespace _Game.Systems.UISystem.Models
{
    public class BetAreaUIModel : BaseUIModel
    {
        public int[] Numbers { get;private set; }
        public BetType BetType{ get;private set; }
        public int Payout{ get;private set; }
        public int TotalPayout{ get;private set; }
        public Vector3 BetAreaPosition { get;private set; }
        

        public void UpdateData(BetAreaData betAreaData, ChipStack stack)
        {
            Numbers = betAreaData.Numbers;
            BetType = betAreaData.Type;
            Payout = betAreaData.Payout;
            TotalPayout = betAreaData.Payout * stack.GetTotalValue();
            BetAreaPosition = betAreaData.Positions[0];
            NotifyUpdate();
        }
    }
}