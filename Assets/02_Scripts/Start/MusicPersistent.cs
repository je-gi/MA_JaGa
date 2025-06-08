using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicPersistent : MonoBehaviour
{
    private static MusicPersistent instance;

    [Header("Audio Settings")]
    public AudioSource musicSource;

    [Header("Fade Settings")]
    public float fadeDuration = 2f;
    public float targetVolume = 0.3f;

    private bool firstSceneLoaded = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null)
            {
                musicSource = GetComponent<AudioSource>();
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (firstSceneLoaded)
        {
            if (musicSource != null)
                StartCoroutine(FadeVolume(targetVolume, fadeDuration));
        }
        else
        {
            firstSceneLoaded = true;
        }
    }

    private IEnumerator FadeVolume(float targetVol, float duration)
    {
        float startVolume = musicSource.volume;
        float time = 0f;

        while (time < duration)
        {
            musicSource.volume = Mathf.Lerp(startVolume, targetVol, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        musicSource.volume = targetVol;
    }
}
