using UnityEngine;

public class Spraycan : MonoBehaviour
{
    public string targetTag = "Sprayable";
    public float detectionRadius = 5f;
    public Material newMaterial;

    private bool isSpraySuccessful = false;

    public bool IsSpraySuccessful() 
    {
        return isSpraySuccessful;
    }

    public void Spray()
    {
        GameObject targetObject = GetTargetInProximity();

        if (targetObject != null)
        {
            Renderer targetRenderer = targetObject.GetComponent<Renderer>();
            if (targetRenderer != null && newMaterial != null)
            {
                targetRenderer.material = newMaterial;
                isSpraySuccessful = true; 
            }
        }
        else
        {
            isSpraySuccessful = false; 
        }
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
