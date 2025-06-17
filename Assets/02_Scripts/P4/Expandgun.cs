using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Expandgun : MonoBehaviour
{
    public string targetTag = "Expandable";
    public float detectionRadius = 5f;
    public float expandAmount = 0.5f;
    private bool expandSuccessful = false;

    private HashSet<GameObject> enlargedObjects = new HashSet<GameObject>();

    [Header("Timing")]
    public float expandDelay = 0f;

    [Header("Effects")]
    public ParticleSystem expandParticles;
    public int particleRepeatCount = 1;
    public float particleRepeatInterval = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip expandSound;

    public void ExpandObject()
    {
        GameObject targetObject = GetTargetInProximity();
        StartCoroutine(ExpandWithDelay(targetObject));

        if (expandParticles != null && particleRepeatCount > 0)
        {
            StartCoroutine(PlayParticlesRepeatedly(particleRepeatCount, particleRepeatInterval));
        }
    }

    private IEnumerator ExpandWithDelay(GameObject targetObject)
    {
        yield return new WaitForSeconds(expandDelay);

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

        if (audioSource != null && expandSound != null)
        {
            audioSource.PlayOneShot(expandSound);
        }
    }

    private IEnumerator PlayParticlesRepeatedly(int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            expandParticles.Play();
            yield return new WaitForSeconds(interval);
        }
        expandParticles.Stop();
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
