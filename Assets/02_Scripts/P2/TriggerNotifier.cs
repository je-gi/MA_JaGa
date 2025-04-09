using UnityEngine;
using UnityEngine.Events;

public class TriggerNotifier : MonoBehaviour
{
    public UnityEvent<GameObject> OnObjectEnteredTrigger = new UnityEvent<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        OnObjectEnteredTrigger.Invoke(other.gameObject);
    }
}
