using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PrefabDuplicator : MonoBehaviour
{
    public XRSocketInteractor mainSocketInteractor;
    public XRSocketInteractor variantSocketInteractor;
    public Transform spawnPoint;
    public GameObject defaultPrefab;
    public GameObject variantPrefab1;
    public GameObject variantPrefab2;
    public GameObject variantPrefab3;
    public GameObject variantPrefab4;

    private GameObject selectedPrefab;

    public GlassesShow glassesShow;

    private void Start()
    {
        selectedPrefab = defaultPrefab;
    }

    public void OnButtonPressed()
    {
        if (mainSocketInteractor.hasSelection)
        {
            if (variantSocketInteractor.hasSelection)
            {
                var selectedInteractable = variantSocketInteractor.GetOldestInteractableSelected();

                if (selectedInteractable != null)
                {
                    GameObject variantObject = selectedInteractable.transform.gameObject;

                    if (variantObject.CompareTag("Variant01"))
                    {
                        selectedPrefab = variantPrefab1;
                    }
                    else if (variantObject.CompareTag("Variant02"))
                    {
                        selectedPrefab = variantPrefab2;
                    }
                    else if (variantObject.CompareTag("Variant03"))
                    {
                        selectedPrefab = variantPrefab3;
                    }
                    else if (variantObject.CompareTag("Variant04"))
                    {
                        selectedPrefab = variantPrefab4;
                    }
                }
            }
            else
            {
                selectedPrefab = defaultPrefab;
            }

            GameObject spawnedObject = Instantiate(selectedPrefab, spawnPoint.position, spawnPoint.rotation);

            if (glassesShow != null)
            {
                glassesShow.RegisterEyeObject(spawnedObject);
            }
        }
    }
}
