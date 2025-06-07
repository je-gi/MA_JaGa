using UnityEngine;
using System.Collections.Generic;

public class Expandgun : MonoBehaviour
{
    public string targetTag = "Expandable";
    public float detectionRadius = 5f;
    public float expandAmount = 0.5f;
    private bool expandSuccessful = false;

    private HashSet<GameObject> enlargedObjects = new HashSet<GameObject>();

    public void ExpandObject()
    {
        GameObject targetObject = GetTargetInProximity();

        if (targetObject != null && !enlargedObjects.Contains(targetObject))
        {
            Vector3 newScale = targetObject.transform.localScale + new Vector3(expandAmount, expandAmount, expandAmount);
            targetObject.transform.localScale = newScale;
            enlargedObjects.Add(targetObject);
            expandSuccessful = true;
        }
        else
        {
            expandSuccessful = false;
        }
    }

    public bool IsShrinkSuccessful()
    {
        return expandSuccessful;
    }

    public void ResetShrink()
    {
        expandSuccessful = false;
    }

    private GameObject GetTargetInProximity()
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject target in targetObjects)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= detectionRadius)
            {
                return target;
            }
        }

        return null;
    }
}
