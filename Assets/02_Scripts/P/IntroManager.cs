using System.Collections;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public AudioSource audioSource;
    public AudioClip introAudioClip;

    public GameObject[] objectsToActivateAfterIntro;

    public delegate void IntroCompletedHandler();
    public event IntroCompletedHandler OnIntroCompleted;

    private IEnumerator Start()
    {
        foreach (var obj in objectsToActivateAfterIntro)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        if (fadeScreen != null)
        {
            fadeScreen.FadeIn(true);
            yield return new WaitForSeconds(fadeScreen.fadeInDuration);
        }

        audioSource.clip = introAudioClip;
        audioSource.Play();
        yield return new WaitForSeconds(introAudioClip.length);

        foreach (var obj in objectsToActivateAfterIntro)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        OnIntroCompleted?.Invoke();
    }
}
