using System.Collections;
using UnityEngine;

public class FadeScreen : MonoBehaviour
{
    public bool fadeOnStart = true;

    [Header("Fade Dauer")]
    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;

    public Color fadeColor = Color.black;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        Color c = fadeColor;
        c.a = 1f;
        rend.material.SetColor("_Color", c);

        if (fadeOnStart)
            FadeIn(true); 
    }

    public void FadeIn(bool disableAfter = false)
    {
        Fade(1f, 0f, fadeInDuration, disableAfter);
    }

    public void FadeOut()
    {
        Fade(0f, 1f, fadeOutDuration, false);
    }

    public void Fade(float alphaIn, float alphaOut, float duration, bool disableAfter = false)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut, duration, disableAfter));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut, float duration, bool disableAfter)
    {
        float timer = 0f;

        while (timer <= duration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);
            rend.material.SetColor("_Color", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color finalColor = fadeColor;
        finalColor.a = alphaOut;
        rend.material.SetColor("_Color", finalColor);

        if (disableAfter)
            gameObject.SetActive(false);
    }
}
