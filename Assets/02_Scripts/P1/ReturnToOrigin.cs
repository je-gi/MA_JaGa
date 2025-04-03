using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ReturnToOrigin : MonoBehaviour
{
    [Header("Return Settings")]
    public float returnSpeed = 2f;
    public float distanceThreshold = 0.05f;

    private XRGrabInteractable grabInteractable;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool returningToOrigin = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        returningToOrigin = true;
    }

    void Update()
    {
        if (returningToOrigin)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, returnSpeed * 100 * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition) <= distanceThreshold)
            {
                returningToOrigin = false;
            }
        }
    }
}
