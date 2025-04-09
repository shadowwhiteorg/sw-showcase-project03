using UnityEngine;

namespace _Game.Core.DI
{
    // No static Instance! Dependencies are injected via DI container.
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour 
        where T : MonoBehaviour
    {
        // Safety check for duplicated instances.
        protected virtual void Awake()
        {
            var existingInstances = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            if (existingInstances.Length > 1)
            {
                Destroy(gameObject);
                Debug.LogWarning($"Duplicated {typeof(T)} destroyed.");
            }
        }
    }
}