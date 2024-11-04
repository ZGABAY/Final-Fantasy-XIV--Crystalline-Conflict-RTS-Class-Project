using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // The point the camera rotates around
    public float distance = 10f;  // Distance from the target (arbitrary)
    public float rotationSpeed = 50f;  // Camera rotation speed (arbitrary)
    public float angleX;  // Fixed X rotation angle (elevation) (arbitrary)
    private float angleY;  // Current Y rotation angle

    void Start()
    {
        angleY = transform.eulerAngles.y; // Use the current Y rotation as starting angle
        angleX = 30f; // Set the X angle to an arbitrary amount
    }

    void Update()
    {
        // Update horizontal angle based on input
        angleY += Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        // Set new camera position and rotation
        Quaternion rotation = Quaternion.Euler(angleX, angleY, 0);
        transform.position = target.position + rotation * new Vector3(0, 0, -distance);
        transform.LookAt(target);
    }
}
