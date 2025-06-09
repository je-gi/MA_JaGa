using UnityEngine;

public class VisibilityCallout : MonoBehaviour
{
    [System.Serializable]
    public class CalloutEntry
    {
        public string key;
        public GameObject targetObject;
    }

    [Header("Zuweisbare Callouts")]
    public CalloutEntry[] callouts;

    public void SetCalloutVisibility(string key, bool visible)
    {
        foreach (var entry in callouts)
        {
            if (entry.key == key && entry.targetObject != null)
            {
                entry.targetObject.SetActive(visible);
                return;
            }
        }
    }
}
