using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LearningTypeCalculator : MonoBehaviour
{
    public int acScore = 0;
    public int ceScore = 0;
    public int aeScore = 0;
    public int roScore = 0;

    public GameObject accommodatingObject;
    public GameObject divergingObject;
    public GameObject convergingObject;
    public GameObject assimilatingObject;

    public void AddScores(XRSocketInteractor[] sockets)
    {
        for (int i = 0; i < sockets.Length; i++)
        {
            IXRSelectInteractable obj = sockets[i].GetOldestInteractableSelected();
            if (obj != null)
            {
                CardData cardData = obj.transform.GetComponent<CardData>();
                if (cardData != null)
                {
                    int socketPoints = i + 1;

                    if (cardData.learningType == "AC")
                    {
                        acScore += socketPoints;
                    }
                    else if (cardData.learningType == "CE")
                    {
                        ceScore += socketPoints;
                    }
                    else if (cardData.learningType == "AE")
                    {
                        aeScore += socketPoints;
                    }
                    else if (cardData.learningType == "RO")
                    {
                        roScore += socketPoints;
                    }
                }
            }
        }
    }

    public string CalculateFinalLearningType()
    {
        int acCeDifference = acScore - ceScore;
        int aeRoDifference = aeScore - roScore;

        string learningType = "";

        if (acCeDifference <= 7 && aeRoDifference >= 7)
        {
            learningType = "Accommodating";
        }
        else if (acCeDifference <= 7 && aeRoDifference <= 6)
        {
            learningType = "Diverging";
        }
        else if (acCeDifference >= 8 && aeRoDifference >= 7)
        {
            learningType = "Converging";
        }
        else if (acCeDifference >= 8 && aeRoDifference <= 6)
        {
            learningType = "Assimilating";
        }

        return learningType;
    }

    public void ShowLearningTypeObject(string learningType)
    {
        accommodatingObject.SetActive(false);
        divergingObject.SetActive(false);
        convergingObject.SetActive(false);
        assimilatingObject.SetActive(false);

        switch (learningType)
        {
            case "Accommodating":
                accommodatingObject.SetActive(true);
                break;
            case "Diverging":
                divergingObject.SetActive(true);
                break;
            case "Converging":
                convergingObject.SetActive(true);
                break;
            case "Assimilating":
                assimilatingObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
