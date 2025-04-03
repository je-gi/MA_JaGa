using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Microwave : MonoBehaviour
{
    [Header("Referenzen")]
    public XRSocketInteractor socketInteractor; 

    private bool isEyeInSocket = false;

    public void OnButtonPressed()
    {
        Debug.Log("Button pressed!"); 


        if (socketInteractor.hasSelection)
        {
            if (!isEyeInSocket)
            {
                Debug.Log("Socket is filled with an object!"); 
                Debug.Log("is now clonable");

                isEyeInSocket = true; 
            }
        }
        else
        {
            if (isEyeInSocket)
            {
                Debug.Log("Socket is empty!"); 
                isEyeInSocket = false;
            }
        }
    }
}
