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
            FadeIn();
    }

    public void FadeIn()
    {
        Fade(1f, 0f, fadeInDuration);
    }

    public void FadeOut()
    {
        Fade(0f, 1f, fadeOutDuration);
    }

    public void Fade(float alphaIn, float alphaOut, float duration)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut, duration));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut, float duration)
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
    }
}
