using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntroManager introManager;
    public LSIManager lsiManager;
    public PuzzleManager puzzleManager;

    private void OnEnable()
    {
        if (introManager != null)
            introManager.OnIntroCompleted += lsiManager.StartLSI;

        if (lsiManager != null)
            lsiManager.OnLSIComplete += puzzleManager.StartPuzzleFlow;
    }

    private void OnDisable()
    {
        if (introManager != null)
            introManager.OnIntroCompleted -= lsiManager.StartLSI;

        if (lsiManager != null)
            lsiManager.OnLSIComplete -= puzzleManager.StartPuzzleFlow;
    }
}
