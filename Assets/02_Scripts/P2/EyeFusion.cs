using UnityEngine;
using System.Collections.Generic;

public class EyeFusion : MonoBehaviour
{
    [Header("Referenzen")]
    public Transform fusionPoint;

    [Header("Augen-Prefabs")]
    public GameObject prefabYellowSmall;
    public GameObject prefabYellowMedium;
    public GameObject prefabYellowLarge;

    public GameObject prefabRedSmall;
    public GameObject prefabRedMedium;
    public GameObject prefabRedLarge;

    public GameObject prefabBlueSmall;
    public GameObject prefabBlueMedium;
    public GameObject prefabBlueLarge;

    public GameObject prefabOrangeSmall;
    public GameObject prefabOrangeMedium;
    public GameObject prefabOrangeLarge;

    public GameObject prefabPurpleSmall;
    public GameObject prefabPurpleMedium;
    public GameObject prefabPurpleLarge;

    public GameObject prefabGreenSmall;
    public GameObject prefabGreenMedium;
    public GameObject prefabGreenLarge;

    private List<GameObject> eyesInTrigger = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (IsValidEye(other.tag))
        {
            eyesInTrigger.Add(other.gameObject);

            if (eyesInTrigger.Count == 2)
            {
                CombineEyes();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (eyesInTrigger.Contains(other.gameObject))
        {
            eyesInTrigger.Remove(other.gameObject);
        }
    }

    private bool IsValidEye(string tag)
    {
        return tag.StartsWith("Eye");
    }

    private void CombineEyes()
    {
        GameObject eye1 = eyesInTrigger[0];
        GameObject eye2 = eyesInTrigger[1];

        string tag1 = eye1.tag;
        string tag2 = eye2.tag;

        Debug.Log("Combining Eyes: " + tag1 + " + " + tag2);

        Destroy(eye1);
        Destroy(eye2);
        eyesInTrigger.Clear();

        CreateEyePrefab(tag1, tag2);
    }

    private void CreateEyePrefab(string tag1, string tag2)
    {
        GameObject prefabToSpawn = null;
        string colorResult = GetColorResult(tag1, tag2);
        string sizeResult = GetSizeResult(tag1, tag2);

        Debug.Log("Color Result: " + colorResult);
        Debug.Log("Size Result: " + sizeResult);

        prefabToSpawn = GetPrefab(colorResult, sizeResult);

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, fusionPoint.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Prefab to spawn is null! Color: " + colorResult + ", Size: " + sizeResult);
        }
    }

    private string GetColorResult(string tag1, string tag2)
    {
        // Orange
        if ((tag1.Contains("Orange") && tag2.Contains("Orange")) ||
            (tag1.Contains("Yellow") && tag2.Contains("Red")) ||
            (tag1.Contains("Red") && tag2.Contains("Yellow")) ||
            (tag1.Contains("Yellow") && tag2.Contains("Purple")) ||
            (tag1.Contains("Purple") && tag2.Contains("Yellow")) ||
            (tag1.Contains("Orange") && tag2.Contains("Purple")) ||
            (tag1.Contains("Purple") && tag2.Contains("Orange")))
            return "Orange";

        // Purple
        if ((tag1.Contains("Purple") && tag2.Contains("Purple")) ||
            (tag1.Contains("Red") && tag2.Contains("Blue")) ||
            (tag1.Contains("Blue") && tag2.Contains("Red")) ||
            (tag1.Contains("Purple") && tag2.Contains("Green")) ||
            (tag1.Contains("Green") && tag2.Contains("Purple")) ||
            (tag1.Contains("Green") && tag2.Contains("Red")) ||
            (tag1.Contains("Red") && tag2.Contains("Green")))
            return "Purple";

        // Green
        if ((tag1.Contains("Green") && tag2.Contains("Green")) ||
            (tag1.Contains("Yellow") && tag2.Contains("Blue")) ||
            (tag1.Contains("Blue") && tag2.Contains("Yellow")) ||
            (tag1.Contains("Green") && tag2.Contains("Orange")) ||
            (tag1.Contains("Orange") && tag2.Contains("Green")) ||
            (tag1.Contains("Blue") && tag2.Contains("Orange")) ||
            (tag1.Contains("Orange") && tag2.Contains("Blue")))
            return "Green";

        // Yellow
        if ((tag1.Contains("Yellow") && tag2.Contains("Yellow")) ||
            (tag1.Contains("Yellow") && tag2.Contains("Orange")) ||
            (tag1.Contains("Orange") && tag2.Contains("Yellow")))
            return "Yellow";

        // Red
        if ((tag1.Contains("Red") && tag2.Contains("Red")) ||
            (tag1.Contains("Purple") && tag2.Contains("Red")) ||
            (tag1.Contains("Red") && tag2.Contains("Purple")))
            return "Red";

        // Blue
        if ((tag1.Contains("Blue") && tag2.Contains("Blue")) ||
            (tag1.Contains("Purple") && tag2.Contains("Green")) ||
            (tag1.Contains("Green") && tag2.Contains("Purple")))
            return "Blue";

        return "Undefined";
    }

    private string GetSizeResult(string tag1, string tag2)
    {
        bool isLarge1 = tag1.Contains("Large");
        bool isLarge2 = tag2.Contains("Large");
        bool isSmall1 = tag1.Contains("Small");
        bool isSmall2 = tag2.Contains("Small");

        if (isSmall1 && isSmall2) return "Small";
        if (isLarge1 && isLarge2) return "Large";

        return "Medium";
    }

    private GameObject GetPrefab(string color, string size)
    {
        switch (color)
        {
            case "Yellow":
                return size == "Small" ? prefabYellowSmall : size == "Medium" ? prefabYellowMedium : prefabYellowLarge;
            case "Red":
                return size == "Small" ? prefabRedSmall : size == "Medium" ? prefabRedMedium : prefabRedLarge;
            case "Blue":
                return size == "Small" ? prefabBlueSmall : size == "Medium" ? prefabBlueMedium : prefabBlueLarge;
            case "Orange":
                return size == "Small" ? prefabOrangeSmall : size == "Medium" ? prefabOrangeMedium : prefabOrangeLarge;
            case "Purple":
                return size == "Small" ? prefabPurpleSmall : size == "Medium" ? prefabPurpleMedium : prefabPurpleLarge;
            case "Green":
                return size == "Small" ? prefabGreenSmall : size == "Medium" ? prefabGreenMedium : prefabGreenLarge;
            default:
                return null;
        }
    }
}
