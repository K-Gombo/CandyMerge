using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Vector3 StartPosition { get; private set; }
    public bool IsTracking { get; private set; } = false;

    public void StartTracking(Vector3 startPos)
    {
        StartPosition = startPos;
        IsTracking = true;
    }

    public void StopTracking()
    {
        IsTracking = false;
    }
}