using _Game.Systems.BetSystem;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _Game.Utils.Tools
{
    public class BetTools : MonoBehaviour
    {
        [MenuItem("Tools/Setup Table Elements", priority = 0)]
        static void SetupTableElements()
        {
            foreach (var tmp in FindObjectsByType<TMP_Text>( FindObjectsInactive.Exclude ,FindObjectsSortMode.None)   )
            {
                if (tmp.GetComponent<TableElement>() == null)
                {
                    tmp.gameObject.AddComponent<TableElement>();
                }
            }
        }
    }
}