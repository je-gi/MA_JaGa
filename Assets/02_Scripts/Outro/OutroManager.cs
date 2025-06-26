using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutroSequence : MonoBehaviour
{
    [Header("Timing")]
    public float delayBeforeStart = 1f;
    public float canvasSpawnIntervalMin = 0.2f;
    public float canvasSpawnIntervalMax = 0.7f;

    [Header("Canvas Settings")]
    public GameObject canvasPrefab;
    public int canvasCount = 10;
    public float minDistance = 2f;
    public float maxDistance = 4f;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float minDistanceBetween = 1.0f;

    [Header("References")]
    public AudioSource audioSource;
    public AudioClip outroClip;
    public Transform player;
    public FadeScreen fadeScreen;

    [Header("Scene")]
    public int nextSceneBuildIndex;

    private List<Vector3> usedPositions = new List<Vector3>();

    void Start()
    {
        StartCoroutine(PlayOutroSequence());
    }

    IEnumerator PlayOutroSequence()
    {
        yield return new WaitForSeconds(delayBeforeStart);

        if (audioSource != null && outroClip != null)
        {
            audioSource.clip = outroClip;
            audioSource.Play();
        }

        StartCoroutine(SpawnCanvasesGradually());

        if (audioSource != null)
            yield return new WaitWhile(() => audioSource.isPlaying);

        if (fadeScreen != null)
            fadeScreen.FadeOut();

        yield return new WaitForSeconds(fadeScreen.fadeOutDuration);

        SceneManager.LoadScene(nextSceneBuildIndex);
    }

    IEnumerator SpawnCanvasesGradually()
    {
        usedPositions.Clear();

        for (int i = 0; i < canvasCount; i++)
        {
            Vector3 pos = GetValidRandomPosition();
            GameObject go = Instantiate(canvasPrefab, pos, Quaternion.identity);

            // Set rotation to face the player
            go.transform.LookAt(player);
            go.transform.Rotate(0f, 180f, 0f); // rotate to face camera if canvas is backward by default

            // Random scale
            float scale = Random.Range(minScale, maxScale);
            go.transform.localScale = Vector3.one * scale;

            yield return new WaitForSeconds(Random.Range(canvasSpawnIntervalMin, canvasSpawnIntervalMax));
        }
    }

    Vector3 GetValidRandomPosition()
    {
        int maxTries = 100;
        for (int i = 0; i < maxTries; i++)
        {
            Vector3 dir = Random.onUnitSphere;
            dir.y = Mathf.Clamp(dir.y, 0.2f, 1f);
            float dist = Random.Range(minDistance, maxDistance);
            Vector3 candidate = player.position + dir * dist;

            bool overlap = false;
            foreach (var pos in usedPositions)
            {
                if (Vector3.Distance(candidate, pos) < minDistanceBetween)
                {
                    overlap = true;
                    break;
                }
            }

            if (!overlap)
            {
                usedPositions.Add(candidate);
                return candidate;
            }
        }

        return player.position + Random.onUnitSphere * Random.Range(minDistance, maxDistance);
    }
}
