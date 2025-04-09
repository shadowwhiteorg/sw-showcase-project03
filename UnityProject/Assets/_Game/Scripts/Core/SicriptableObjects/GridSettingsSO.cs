using UnityEngine;

namespace _Game.Core.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Roulette/Grid Settings", fileName = "RouletteGridSettings")]
    public class GridSettingsSO : ScriptableObject
    {
        [Header("Cell Dimensions")] [Tooltip("Width/height of each grid cell in world units")]
        public float CellSize = 1f;

        [Header("Grid Alignment")] [Tooltip("World position where grid (0,0) should be")]
        public Vector2 GridOrigin = Vector2.zero;

        [Header("Axis Configuration")] [Tooltip("Flip X axis coordinates if needed")]
        public bool InvertXAxis = false;

        [Tooltip("Flip Y axis coordinates if needed")]
        public bool InvertYAxis = true;

        [Header("Editor Visuals")] public Color GridGizmoColor = new Color(1, 0.5f, 0, 0.5f); // Orange
        [Range(0.1f, 2f)] public float GizmoOpacity = 0.7f;

        // Conversion methods
        public Vector2Int WorldToGrid(Vector3 worldPos)
        {
            float x = (worldPos.x - GridOrigin.x) / CellSize;
            float y = (worldPos.y - GridOrigin.y) / CellSize;

            return new Vector2Int(
                InvertXAxis ? -Mathf.RoundToInt(x) : Mathf.RoundToInt(x),
                InvertYAxis ? -Mathf.RoundToInt(y) : Mathf.RoundToInt(y)
            );
        }

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            float x = gridPos.x * CellSize + GridOrigin.x;
            float y = gridPos.y * CellSize + GridOrigin.y;

            return new Vector3(
                InvertXAxis ? -x : x,
                InvertYAxis ? -y : y,
                0
            );
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            CellSize = Mathf.Max(0.01f, CellSize);
        }

        public void DrawEditorGizmos(Vector2Int gridDimensions)
        {
            Gizmos.color = new Color(GridGizmoColor.r, GridGizmoColor.g, GridGizmoColor.b, GizmoOpacity);

            for (int x = 0; x < gridDimensions.x; x++)
            {
                for (int y = 0; y < gridDimensions.y; y++)
                {
                    Vector3 center = GridToWorld(new Vector2Int(x, y));
                    Vector3 size = Vector3.one * CellSize * 0.95f;
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }
#endif
    }
}