using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class P4AudioManager : MonoBehaviour

{
    public AudioSource audioSource;

    public GameObject object1;
    public GameObject object2;


    public AudioClip object1Audio;
    public AudioClip object2Audio;


    public AudioClip socket1OccupiedAudio;
    public AudioClip socket2OccupiedAudio;
    public AudioClip socket3OccupiedAudio;
    public AudioClip socket4OccupiedAudio;
    public AudioClip socket5OccupiedAudio;
    public AudioClip socket6OccupiedAudio;

    private bool object1Grabbed = false;
    private bool object2Grabbed = false;

    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public XRSocketInteractor socket3;
    public XRSocketInteractor socket4;
    public XRSocketInteractor socket5;
    public XRSocketInteractor socket6;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (object1 != null && !object1Grabbed && IsObjectGrabbed(object1))
        {
            PlayAudio(object1Audio);
            object1Grabbed = true; 
        }

        if (object1Grabbed && object2 != null && !object2Grabbed && IsObjectGrabbed(object2))
        {
            PlayAudio(object2Audio);
            object2Grabbed = true; 
        }

        if (socket1 != null && socket1.hasSelection && socket1OccupiedAudio != null)
        {
            PlayAudio(socket1OccupiedAudio);
            socket1OccupiedAudio = null;
        }

        if (socket2 != null && socket2.hasSelection && socket2OccupiedAudio != null)
        {
            PlayAudio(socket2OccupiedAudio);
            socket2OccupiedAudio = null;
        }

        if (socket3 != null && socket3.hasSelection && socket3OccupiedAudio != null)
        {
            PlayAudio(socket3OccupiedAudio);
            socket3OccupiedAudio = null; 
        }

        if (socket4 != null && socket4.hasSelection && socket4OccupiedAudio != null)
        {
            PlayAudio(socket4OccupiedAudio);
            socket4OccupiedAudio = null; 
        }

        if (socket5 != null && socket5.hasSelection && socket5OccupiedAudio != null)
        {
            PlayAudio(socket5OccupiedAudio);
            socket5OccupiedAudio = null; 
        }

        if (socket6 != null && socket6.hasSelection && socket6OccupiedAudio != null)
        {
            PlayAudio(socket6OccupiedAudio);
            socket6OccupiedAudio = null; 
        }
    }

    private bool IsObjectGrabbed(GameObject obj)
    {

        XRGrabInteractable grabInteractable = obj.GetComponent<XRGrabInteractable>();

        if (grabInteractable != null && grabInteractable.isSelected)
        {
            return true; 
        }

        return false; 
    }

    private void PlayAudio(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.PlayOneShot(clip);
        }
    }
}
