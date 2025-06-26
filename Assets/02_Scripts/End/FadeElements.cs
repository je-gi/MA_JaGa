using System.Collections;
using UnityEngine;
using TMPro;

public class FadeElements : MonoBehaviour
{
    [System.Serializable]
    public class FadeElement
    {
        public TextMeshProUGUI text;
        public Renderer image;
    }

    [Header("Einstellungen")]
    public FadeElement[] elements;
    public float initialDelay = 1f;      
    public float delayBetweenElements = 1.5f; 
    public float fadeDuration = 2f;      

    private void Start()
    {
        foreach (var element in elements)
        {
            if (element.text) SetTMPAlpha(element.text, 0f);
            if (element.image) SetRendererAlpha(element.image, 0f);
        }

        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(initialDelay);

        foreach (var element in elements)
        {
            StartCoroutine(FadeInElement(element));
            yield return new WaitForSeconds(delayBetweenElements);
        }
    }

    private IEnumerator FadeInElement(FadeElement element)
    {
        float elapsed = 0f;

        Color startTextColor = element.text ? element.text.color : Color.clear;
        Color startImageColor = element.image ? element.image.material.color : Color.clear;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);

            if (element.text)
                element.text.color = new Color(startTextColor.r, startTextColor.g, startTextColor.b, alpha);

            if (element.image)
                element.image.material.color = new Color(startImageColor.r, startImageColor.g, startImageColor.b, alpha);

            yield return null;
        }

        if (element.text)
            element.text.color = new Color(startTextColor.r, startTextColor.g, startTextColor.b, 1f);

        if (element.image)
            element.image.material.color = new Color(startImageColor.r, startImageColor.g, startImageColor.b, 1f);
    }

    private void SetRendererAlpha(Renderer renderer, float alpha)
    {
        if (!renderer) return;
        var mat = renderer.material;
        var c = mat.color;
        mat.color = new Color(c.r, c.g, c.b, alpha);
    }

    private void SetTMPAlpha(TextMeshProUGUI tmp, float alpha)
    {
        if (!tmp) return;
        var c = tmp.color;
        tmp.color = new Color(c.r, c.g, c.b, alpha);
    }
}
