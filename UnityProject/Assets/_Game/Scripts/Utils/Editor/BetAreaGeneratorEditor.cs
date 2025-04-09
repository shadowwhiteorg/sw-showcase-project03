using _Game.Systems.BetSystem;
using UnityEditor;
using UnityEngine;

namespace _Game.Utils.Editor
{
    [CustomEditor(typeof(BetAreaGenerator))]
    public class BetAreaGeneratorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Now"))
                ((BetAreaGenerator)target).GenerateBetAreas();
        }
    }
}