using UnityEngine;

public class AutoFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // The player object to follow
    public Vector3 offset = new Vector3(0, 5, -10); // Offset relative to the target
    public float smoothSpeed = 0.125f; // Smoothness of the follow movement
    public Quaternion manualRotation = Quaternion.identity; // Adjustable rotation

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not set for CameraFollow script!");
            return;
        }

        // Calculate the desired position with the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate between the current position and the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply the smoothed position
        transform.position = smoothedPosition;

        // Apply the manually set rotation to the camera
        transform.rotation = manualRotation;
    }
}
