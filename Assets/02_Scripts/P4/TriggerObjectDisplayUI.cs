using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TriggerObjectDisplayUI : MonoBehaviour
{
    [Header("UI Text Settings")]
    public TextMeshProUGUI textUI;

    [System.Serializable]
    public class ObjectTextPair
    {
        public GameObject observedObject;
        public string displayText;
    }

    public List<ObjectTextPair> objectsToWatch;

    private HashSet<GameObject> objectsInTrigger = new HashSet<GameObject>();
    private Dictionary<GameObject, string> activeTexts = new Dictionary<GameObject, string>();

    private void OnTriggerEnter(Collider other)
    {
        foreach (var pair in objectsToWatch)
        {
            if (other.gameObject == pair.observedObject && !objectsInTrigger.Contains(other.gameObject))
            {
                objectsInTrigger.Add(other.gameObject);
                DisplayText(pair.observedObject, pair.displayText);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (var pair in objectsToWatch)
        {
            if (other.gameObject == pair.observedObject && objectsInTrigger.Contains(other.gameObject))
            {
                objectsInTrigger.Remove(other.gameObject);
                ClearText(pair.observedObject, pair.displayText);
            }
        }
    }

    private void DisplayText(GameObject observedObject, string text)
    {
        if (!activeTexts.ContainsKey(observedObject))
        {
            activeTexts[observedObject] = text;
            textUI.text += text + "\n";
        }
    }

    private void ClearText(GameObject observedObject, string text)
    {
        if (activeTexts.ContainsKey(observedObject))
        {
            activeTexts.Remove(observedObject);
            textUI.text = textUI.text.Replace(text + "\n", "");
        }
    }

    public void ClearAllText()
    {
        textUI.text = "";
        objectsInTrigger.Clear();
        activeTexts.Clear();
    }
}
