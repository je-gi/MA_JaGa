using UnityEngine;

public class SceneDarkener : MonoBehaviour
{
    public Light lightToKeepOn;

    void Start()
    {
        if (lightToKeepOn == null)
        {
            Debug.LogError("Kein Licht zugewiesen! Bitte weise ein Licht im Inspector zu.");
            return;
        }

        Light[] allLights = FindObjectsOfType<Light>();
        Debug.Log($"Gefundene Lichter: {allLights.Length}");

        foreach (Light light in allLights)
        {
            if (light == lightToKeepOn)
            {
                light.enabled = true;
                Debug.Log($"Licht '{light.name}' bleibt AN");
            }
            else
            {
                light.enabled = false;
                Debug.Log($"Licht '{light.name}' wird AUSgeschaltet");
            }
        }
    }
}
