using _Game.Core.ScriptableObjects;
using _Game.Interfaces;
using UnityEngine;

namespace _Game.Systems.RouletteSystem
{
    public class RouletteFactory
    {
        public RouletteContext CreateRoulette(WheelView wheelPrefab, BallView ballPrefab, RouletteConfigSO config,
            GameObject rouletteTable, IEventBus eventBus)
        {

            var table = Object.Instantiate(rouletteTable);
            var wheel = Object.Instantiate(wheelPrefab, table.transform);
            wheel.Construct();
            var ball = Object.Instantiate(ballPrefab, table.transform);
            ball.Construct(eventBus,wheel);

            return new RouletteContext(wheel, ball, config);
        }
    }
}