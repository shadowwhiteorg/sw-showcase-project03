using _Game.Enums;
using _Game.Systems.BetSystem;
using UnityEngine;

namespace _Game.Utils.Helpers
{
    public static class GizmoEditorHelpers
    {
        public static void DrawAreaGizmo(BetArea area)
        {
            Gizmos.color = GetAreaColor(area.Data.Type);
            foreach (var pos in area.Data.Positions)
            {
                Gizmos.DrawWireCube(pos, Vector3.one * 0.1f);
                UnityEditor.Handles.Label(
                    pos + Vector3.up * 0.1f,
                    $"{area.Data.Type}"
                    // $"{area.Type}\n{string.Join(",", area.Numbers)}"
                );
            }
        }
        

        private static Color GetAreaColor(BetType type) => type switch
        {
            BetType.Straight => Color.green,
            BetType.Split => Color.yellow,
            BetType.Street => Color.blue,
            BetType.Corner => Color.magenta,
            BetType.Dozen => new Color(1, 0.5f, 0),
            BetType.Column => new Color(0, 1, 1),
            _ => Color.white
        };
        
    }
}