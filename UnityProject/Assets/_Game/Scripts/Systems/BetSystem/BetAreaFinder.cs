using System.Collections.Generic;
using UnityEngine;

namespace _Game.Systems.BetSystem
{
    public static class BetAreaFinder
    {
        private static List<BetArea> _areas;

        public static void Initialize(List<BetArea> areas) => _areas = areas;

        public static BetArea FindClosest(Vector3 position)
        {
            BetArea closest = null;
            float minDist = Mathf.Infinity;

            foreach (var area in _areas)
            {
                float dist = Vector3.Distance(position, area.Data.Positions[0]);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = area;
                }
            }
            return closest;
        }
    }
}