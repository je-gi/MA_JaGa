using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class HandAssemblyManager : MonoBehaviour
{
    [Header("Hand 1")]
    public List<XRSocketInteractor> hand1Sockets;
    public List<GameObject> hand1Parts;
    public GameObject hand1Model;
    public Spraycan spray1;
    public Shrinkgun shrink1;

    [Header("Hand 2")]
    public List<XRSocketInteractor> hand2Sockets;
    public List<GameObject> hand2Parts;
    public GameObject hand2Model;
    public Spraycan spray2;
    public Shrinkgun shrink2;

    [Header("SocketChecker")]
    public DisableGrabAndMakeKinematicOnSocket socketCheckerScript;

    [Header("UI Text Management")]
    public TextMeshProUGUI uiText;
    private Dictionary<string, string> objectTextDict = new Dictionary<string, string>();

    [Header("Audio Management")]
    public AudioClip hand2StartAudio; 
    public AudioSource audioSource;   

    private int hand1Step = 0;
    private int hand2Step = 0;
    private bool hand1Done = false;
    private bool hand2Started = false;
    private bool hand2Done = false;

    private void Start()
    {
        InitSockets(hand1Sockets);
        InitSockets(hand2Sockets);
        if (hand1Sockets.Count > 0) hand1Sockets[0].socketActive = true;
        if (hand1Parts.Count > 0) hand1Parts[0].SetActive(true);
        SetActiveList(hand2Parts, false);
    }

    private void Update()
    {
        HandleHand1();
        if (!hand2Started && IsAnySocketOccupied()) StartHand2();
        if (hand2Started) HandleHand2();
    }

    private void InitSockets(List<XRSocketInteractor> sockets)
    {
        foreach (var s in sockets)
            if (s != null) s.socketActive = false;
    }

    private void SetActiveList(List<GameObject> objects, bool active)
    {
        foreach (var obj in objects)
            if (obj != null) obj.SetActive(active);
    }

    private void HandleHand1()
    {
        if (hand1Done) return;

        if (hand1Step == 0 && hand1Sockets[0].hasSelection)
        {
            hand1Step++;
            ActivateSocketAndPart(hand1Sockets, hand1Parts, hand1Step);
        }
        else if (hand1Step == 1 && hand1Sockets[1].hasSelection && spray1.IsSpraySuccessful())
        {
            hand1Step++;
            ActivateSocketAndPart(hand1Sockets, hand1Parts, hand1Step);
        }
        else if (hand1Step == 2 && hand1Sockets[2].hasSelection && shrink1.IsShrinkSuccessful())
        {
            hand1Step++;
            ActivateSocketAndPart(hand1Sockets, hand1Parts, hand1Step);
        }
        else if (hand1Step == 3 && hand1Sockets[3].hasSelection)
        {
            FinalizeHand(hand1Sockets, hand1Parts, hand1Model);
            hand1Done = true;
            spray1.ResetSpray();
            shrink1.ResetShrink();
        }
    }

    private void HandleHand2()
    {
        if (hand2Done) return;

        if (hand2Step == 0 && hand2Sockets[0].hasSelection)
        {
            hand2Step++;
            ActivateSocketAndPart(hand2Sockets, hand2Parts, hand2Step);
        }
        else if (hand2Step == 1 && hand2Sockets[1].hasSelection && spray2.IsSpraySuccessful())
        {
            hand2Step++;
            ActivateSocketAndPart(hand2Sockets, hand2Parts, hand2Step);
        }
        else if (hand2Step == 2 && hand2Sockets[2].hasSelection && shrink2.IsShrinkSuccessful())
        {
            hand2Step++;
            ActivateSocketAndPart(hand2Sockets, hand2Parts, hand2Step);
        }
        else if (hand2Step == 3 && hand2Sockets[3].hasSelection)
        {
            FinalizeHand(hand2Sockets, hand2Parts, hand2Model);
            hand2Done = true;
        }
    }

    private void ActivateSocketAndPart(List<XRSocketInteractor> sockets, List<GameObject> parts, int index)
    {
        if (index < sockets.Count) sockets[index].socketActive = true;
        if (index < parts.Count) parts[index].SetActive(true);
    }

    private void FinalizeHand(List<XRSocketInteractor> sockets, List<GameObject> parts, GameObject model)
    {
        foreach (var socket in sockets)
            socket.socketActive = false;

        SetActiveList(parts, false);

        if (model != null)
        {
            model.SetActive(true);
        }

        TriggerObjectDisplayUI triggerTextManager = Object.FindFirstObjectByType<TriggerObjectDisplayUI>();

        if (triggerTextManager != null)
        {
            triggerTextManager.ClearAllText();
        }
    }

    private bool IsAnySocketOccupied()
    {
        return socketCheckerScript != null &&
            (
                (socketCheckerScript.socket1Interactor != null && socketCheckerScript.socket1Interactor.hasSelection) ||
                (socketCheckerScript.socket2Interactor != null && socketCheckerScript.socket2Interactor.hasSelection)
            );
    }

    private void StartHand2()
    {
        hand2Started = true;
        SetActiveList(hand2Parts, true);
        if (hand2Sockets.Count > 0)
            hand2Sockets[0].socketActive = true;

        PlayAudio(hand2StartAudio);
    }

    public void AddTextForObject(GameObject obj, string text)
    {
        if (!objectTextDict.ContainsKey(obj.name))
        {
            objectTextDict.Add(obj.name, text);
            UpdateUIText();
        }
    }

    private void UpdateUIText()
    {
        if (uiText != null)
        {
            uiText.text = "";
            foreach (var item in objectTextDict)
            {
                uiText.text += item.Value + "\n";
            }
        }
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
