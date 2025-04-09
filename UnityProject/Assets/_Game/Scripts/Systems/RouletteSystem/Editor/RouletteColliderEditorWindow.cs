using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Game.Systems.RouletteSystem
{
#if UNITY_EDITOR
    public class RouletteColliderEditorWindow : EditorWindow
    {
        private Transform _numberParent;
        private Transform _wheelCenter;
        private GameObject _colliderPrefab;
        private Transform _collidersParent;

        private Color _gizmoColor = Color.yellow;
        private bool _showGizmos = true;

        [MenuItem("Tools/Roulette Collider Generator")]
        public static void ShowWindow()
        {
            GetWindow<RouletteColliderEditorWindow>("Roulette Collider Generator");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Roulette Collider Generator", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Automatically generate colliders and NumberSlotData for your roulette wheel.",
                MessageType.Info);

            _numberParent =
                (Transform)EditorGUILayout.ObjectField("Number Parent", _numberParent, typeof(Transform), true);
            _wheelCenter = (Transform)EditorGUILayout.ObjectField("Wheel Center", _wheelCenter, typeof(Transform), true);
            _colliderPrefab =
                (GameObject)EditorGUILayout.ObjectField("Collider Prefab", _colliderPrefab, typeof(GameObject), false);
            _collidersParent =
                (Transform)EditorGUILayout.ObjectField("Colliders Parent", _collidersParent, typeof(Transform), true);

            _showGizmos = EditorGUILayout.Toggle("Show Gizmo Preview", _showGizmos);
            _gizmoColor = EditorGUILayout.ColorField("Gizmo Color", _gizmoColor);

            EditorGUILayout.Space();

            if (GUILayout.Button("Generate Colliders (Resets Existing)"))
            {
                GenerateColliders();
            }
        }

        private List<(int number, Vector3 position)> ReadSlotNumbers()
        {
            List<(int number, Vector3 position)> slots = new();

            foreach (Transform child in _numberParent)
            {
                var tmp = child.GetComponentInChildren<TextMeshPro>();
                if (tmp == null)
                    continue;

                if (int.TryParse(tmp.text, out int num))
                    slots.Add((num, child.GetChild(0).position));
                else
                    Debug.LogWarning($"Skipped invalid TMP on {child.name}");
            }

            return slots;
        }

        private void GenerateColliders()
        {
            if (!_numberParent || !_wheelCenter || !_colliderPrefab)
            {
                Debug.LogError("Missing required fields");
                return;
            }

            var slotData = ReadSlotNumbers();

            if (slotData.Count < 2)
            {
                Debug.LogError("Not enough slots detected.");
                return;
            }

            if (_collidersParent)
            {
                for (int i = _collidersParent.childCount - 1; i >= 0; i--)
                    DestroyImmediate(_collidersParent.GetChild(i).gameObject);
            }

            for (int i = 0; i < slotData.Count; i++)
            {
                Vector3 posA = slotData[i].position;
                Vector3 posB = slotData[(i + 1) % slotData.Count].position;

                // Vector3 midPoint = (posA + posB) * 0.5f;
                Vector3 midPoint = posA;
                Vector3 dir = (midPoint - _wheelCenter.position).normalized;

                GameObject colliderObj = (GameObject)PrefabUtility.InstantiatePrefab(_colliderPrefab);
                colliderObj.transform.SetParent(_collidersParent ? _collidersParent : _wheelCenter, true);
                colliderObj.transform.position = midPoint;
                colliderObj.transform.LookAt(_wheelCenter);
                // colliderObj.transform.up = dir;

                var data = colliderObj.GetComponent<NumberSlotData>() ?? colliderObj.AddComponent<NumberSlotData>();
                data.Number = slotData[i].number;
                data.Index = i;
                data.Position = colliderObj.transform.position;
                PrefabUtility.RecordPrefabInstancePropertyModifications(colliderObj);
                EditorUtility.SetDirty(colliderObj);
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }

            Debug.Log($"Generated {slotData.Count} colliders successfully.");
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!_showGizmos || _numberParent == null)
                return;

            Handles.color = _gizmoColor;

            foreach (Transform child in _numberParent)
            {
                Handles.DrawSolidDisc(child.position, Vector3.up, 0.05f);
                Handles.Label(child.position + Vector3.up * 0.1f, child.name);
            }
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }
#endif
}