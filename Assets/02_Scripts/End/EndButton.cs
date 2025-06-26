using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class EndButton : MonoBehaviour
{
    public FadeScreen fadeScreen;
    public AudioSource audioSource;
    public AudioClip pressAClip;
    public Animator startButtonAnimator;
    public float delayAfterFade = 0.5f;

    private bool isTransitioning = false;

    void Update()
    {
        if (!isTransitioning)
        {
            InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            bool aButtonPressed = false;

            if (rightHand.isValid &&
                rightHand.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) &&
                aButtonPressed)
            {
                StartCoroutine(StartSceneTransition());
            }
        }
    }

    private IEnumerator StartSceneTransition()
    {
        isTransitioning = true;

        if (startButtonAnimator != null)
            startButtonAnimator.SetTrigger("Pressed");

        if (audioSource != null && pressAClip != null)
            audioSource.PlayOneShot(pressAClip);

        if (fadeScreen != null)
            fadeScreen.FadeOut();

        yield return new WaitForSeconds(fadeScreen != null ? fadeScreen.fadeOutDuration + delayAfterFade : 1f);

        SceneManager.LoadScene(0);
    }
}
