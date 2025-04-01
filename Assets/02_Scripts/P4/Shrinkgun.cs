using UnityEngine;

public class Shrinkgun : MonoBehaviour
{
    public string targetTag = "Shrinkable";
    public float detectionRadius = 5f;
    public float shrinkAmount = 0.5f;
    private bool shrinkSuccessful = false;

    public void ShrinkObject()
    {
        GameObject targetObject = GetTargetInProximity();

        if (targetObject != null)
        {
            Vector3 newScale = targetObject.transform.localScale - new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);

            if (newScale.x > 0 && newScale.y > 0 && newScale.z > 0)
            {
                targetObject.transform.localScale = newScale;
                shrinkSuccessful = true;
            }
            else
            {
                shrinkSuccessful = false;
            }
        }
        else
        {
            shrinkSuccessful = false;
        }
    }

    public bool IsShrinkSuccessful()
    {
        return shrinkSuccessful;
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
