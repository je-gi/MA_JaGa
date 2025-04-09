using System.Collections;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public Transform startPoint; // Der Startpunkt (Plughalterung)
    public Transform endPoint;   // Der Endpunkt (Plug)
    public LineRenderer lineRenderer;  // Der LineRenderer f�r das Seil
    public float ropeSpeed = 5f; // Geschwindigkeit, mit der sich das Seil bewegt

    private bool isHeld = false;   // �berpr�ft, ob der Plug gehalten wird
    private bool isPlugInserted = false; // �berpr�ft, ob der Plug eingesteckt ist

    void Start()
    {
        // Initialisiere den LineRenderer f�r das Seil
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        if (isHeld)
        {
            // Ziehe das Seil und folge dem Plug
            float distance = Vector3.Distance(startPoint.position, endPoint.position);
            UpdateRopeLength(distance);

            // Bewege das Seil entlang des Plugs
            if (!isPlugInserted)
            {
                // Wenn der Plug nicht eingesteckt ist, ziehe ihn zur�ck zur Halterung, wenn er losgelassen wird
                endPoint.position = Vector3.Lerp(endPoint.position, startPoint.position, ropeSpeed * Time.deltaTime);
            }
        }
        else if (isPlugInserted)
        {
            // Wenn der Plug in der Endposition (Schnecke) ist, ziehe das Seil nicht zur�ck
            UpdateRopeLength(Vector3.Distance(startPoint.position, endPoint.position));
        }
    }

    private void UpdateRopeLength(float distance)
    {
        // Stelle sicher, dass der LineRenderer eine neue L�nge bekommt, die dem Abstand entspricht
        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }

    // Methoden zum Setzen der Interaktion
    public void OnGrab()
    {
        isHeld = true;
    }

    public void OnRelease()
    {
        isHeld = false;
    }

    public void OnPlugInserted()
    {
        isPlugInserted = true;
    }

    public void OnPlugRemoved()
    {
        isPlugInserted = false;
    }
}
