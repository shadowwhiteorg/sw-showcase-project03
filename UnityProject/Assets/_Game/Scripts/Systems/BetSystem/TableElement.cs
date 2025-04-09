using _Game.Core.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Game.Systems.BetSystem
{
    [ExecuteAlways]
    public class TableElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text textComponent;
        public string Value => textComponent != null ? textComponent.text.Trim() : "";
        // public Vector3 Position => transform.position;
        public Vector3 Position => GetComponent<RectTransform>().position;

        void OnValidate() => Refresh();
        void Awake() => Refresh();

        public void Refresh()
        {
            if (textComponent == null)
                textComponent = GetComponentInChildren<TMP_Text>(true);
        }
    }
}