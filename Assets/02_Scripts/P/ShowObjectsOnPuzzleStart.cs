using UnityEngine;
using System.Collections.Generic;

public class ShowObjectsOnPuzzleStart : MonoBehaviour
{
    public List<GameObject> objectsToShow;
    public MonoBehaviour puzzleManager;

    private bool hasShown = false;
    private System.Reflection.FieldInfo hasStartedField;

    void Start()
    {
        foreach (var obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        if (puzzleManager != null)
        {
            hasStartedField = puzzleManager.GetType().GetField("hasStarted", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        }
    }

    void Update()
    {
        if (hasShown || puzzleManager == null || hasStartedField == null) return;

        bool hasStarted = (bool)hasStartedField.GetValue(puzzleManager);
        if (hasStarted)
        {
            ShowObjects();
            hasShown = true;
        }
    }

    private void ShowObjects()
    {
        foreach (var obj in objectsToShow)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }
}
