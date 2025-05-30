using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using TMPro;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public FadeScreen fadeScreen;  
    public Renderer titleRenderer;
    public TextMeshProUGUI object1Text;
    public Renderer object1ImageRenderer;
    public TextMeshProUGUI object2Text;
    public Renderer object2ImageRenderer;

    private bool canLoadNextScene = false;
    private bool isFadingOut = false;

    private void Start()
    {
        fadeScreen.FadeIn();

        SetRendererAlpha(titleRenderer, 0f);
        SetTMPAlpha(object1Text, 0f);
        SetRendererAlpha(object1ImageRenderer, 0f);
        SetTMPAlpha(object2Text, 0f);
        SetRendererAlpha(object2ImageRenderer, 0f);

        StartCoroutine(IntroSequence());
    }

    private IEnumerator IntroSequence()
    {
        yield return new WaitForSeconds(fadeScreen.fadeInDuration);

        yield return StartCoroutine(FadeRendererAlpha(titleRenderer, 0f, 1f, 2f));
        yield return StartCoroutine(FadeTextAndRenderer(object1Text, object1ImageRenderer, 0f, 1f, 2f));

        canLoadNextScene = true;

        yield return StartCoroutine(FadeTextAndRenderer(object2Text, object2ImageRenderer, 0f, 1f, 2f));
    }

    private void Update()
    {
        if (canLoadNextScene && !isFadingOut)
        {
            InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool primaryButtonPressed = false;

            if (rightHand.isValid && rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonPressed) && primaryButtonPressed)
            {
                isFadingOut = true;
                canLoadNextScene = false;
                StartCoroutine(FadeOutAndLoadNextScene());
            }
        }
    }

    private IEnumerator FadeOutAndLoadNextScene()
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(fadeScreen.fadeOutDuration);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator FadeRendererAlpha(Renderer renderer, float from, float to, float duration)
    {
        float elapsed = 0f;
        Material mat = renderer.material;
        Color startColor = mat.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(from, to, elapsed / duration);
            mat.color = new Color(startColor.r, startColor.g, startColor.b, a);
            yield return null;
        }

        mat.color = new Color(startColor.r, startColor.g, startColor.b, to);
    }

    private IEnumerator FadeTextAndRenderer(TextMeshProUGUI tmp, Renderer renderer, float from, float to, float duration)
    {
        float elapsed = 0f;
        Color tmpStartColor = tmp.color;
        Material mat = renderer.material;
        Color matStartColor = mat.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float a = Mathf.Lerp(from, to, elapsed / duration);

            tmp.color = new Color(tmpStartColor.r, tmpStartColor.g, tmpStartColor.b, a);
            mat.color = new Color(matStartColor.r, matStartColor.g, matStartColor.b, a);

            yield return null;
        }

        tmp.color = new Color(tmpStartColor.r, tmpStartColor.g, tmpStartColor.b, to);
        mat.color = new Color(matStartColor.r, matStartColor.g, matStartColor.b, to);
    }

    private void SetRendererAlpha(Renderer renderer, float alpha)
    {
        Material mat = renderer.material;
        Color c = mat.color;
        mat.color = new Color(c.r, c.g, c.b, alpha);
    }

    private void SetTMPAlpha(TextMeshProUGUI tmp, float alpha)
    {
        Color c = tmp.color;
        tmp.color = new Color(c.r, c.g, c.b, alpha);
    }
}
