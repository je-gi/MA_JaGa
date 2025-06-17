using System.Collections;
using UnityEngine;

public class Spraycan : MonoBehaviour
{
    [Header("Spray Settings")]
    public string targetTag = "Sprayable";
    public float detectionRadius = 5f;
    public Material newMaterial;
    public float colorChangeDelay = 0.5f;

    [Header("Effects")]
    public ParticleSystem sprayEffect;     
    public AudioSource spraySource;       
    public AudioClip sprayClip;           
    private bool isSpraySuccessful = false;

    public bool IsSpraySuccessful()
    {
        return isSpraySuccessful;
    }

    public void Spray()
    {
        GameObject targetObject = GetTargetInProximity();
        if (sprayEffect != null)
        {
            sprayEffect.Play();
        }

        if (targetObject != null)
        {
            StartCoroutine(ApplySprayEffectWithDelay(targetObject));
        }
        else
        {
            isSpraySuccessful = false;
        }
    }

    private IEnumerator ApplySprayEffectWithDelay(GameObject target)
    {
        yield return new WaitForSeconds(colorChangeDelay);

        if (spraySource != null && sprayClip != null)
        {
            spraySource.PlayOneShot(sprayClip);
        }

        Renderer targetRenderer = target.GetComponent<Renderer>();
        if (targetRenderer != null && newMaterial != null)
        {
            targetRenderer.material = newMaterial;
            isSpraySuccessful = true;
        }
    }

    public void ResetSpray()
    {
        isSpraySuccessful = false;
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
