using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // The boat to follow
    public Vector3 offset;     // Offset from the target
    public float smoothSpeed = 0.125f; // Smoothness of camera movement

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position
        Vector3 desiredPosition = target.position + offset;

        // Smooth the transition
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Update the camera's position
        transform.position = smoothedPosition;

        // Always look at the target
        transform.LookAt(target);
    }
}
