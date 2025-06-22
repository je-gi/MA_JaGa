using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PushVRButton : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    [Header("Bewegungseinstellungen")]
    public Axis moveAxis = Axis.Z;
    public float pressDepth = 0.05f;
    public float moveSpeed = 0.1f;
    public bool invertDirection = false;

    [Header("Trigger-Einstellungen")]
    public float deadTime = 1.0f;
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private Vector3 initialLocalPosition;
    private float currentDepth = 0f;
    private bool isPressed = false;
    private bool wasPressedLastFrame = false;
    private bool deadTimeActive = false;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        float targetDepth = isPressed ? pressDepth : 0f;
        currentDepth = Mathf.MoveTowards(currentDepth, targetDepth, moveSpeed * Time.deltaTime);

        float offset = invertDirection ? currentDepth : -currentDepth;
        transform.localPosition = initialLocalPosition + AxisOffset(offset);

        if (!deadTimeActive)
        {
            if (isPressed && !wasPressedLastFrame)
            {
                onPressed?.Invoke();
            }
            else if (!isPressed && wasPressedLastFrame)
            {
                onReleased?.Invoke();
                StartCoroutine(WaitForDeadTime());
            }
        }

        wasPressedLastFrame = isPressed;
        isPressed = false;
    }

    private Vector3 AxisOffset(float amount)
    {
        switch (moveAxis)
        {
            case Axis.X: return new Vector3(amount, 0, 0);
            case Axis.Y: return new Vector3(0, amount, 0);
            case Axis.Z: return new Vector3(0, 0, amount);
            default: return Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isPressed = true;
    }

    private System.Collections.IEnumerator WaitForDeadTime()
    {
        deadTimeActive = true;
        yield return new WaitForSeconds(deadTime);
        deadTimeActive = false;
    }
}
