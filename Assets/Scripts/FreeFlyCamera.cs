using UnityEngine;

/// <summary>
/// A smooth free-fly camera script to navigate and inspect the gate and road scene.
/// Controls:
///   W/A/S/D - Move forward/left/backward/right
///   Q/E or Space/LeftShift - Move down/up
///   Right Mouse Button (Hold) - Look around
///   Left Shift (Hold) - Speed boost
/// </summary>
public class FreeFlyCamera : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float boostMultiplier = 2.5f;
    public float lookSensitivity = 3f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Start()
    {
        Vector3 rot = transform.localEulerAngles;
        rotationX = rot.x;
        rotationY = rot.y;
    }

    private void Update()
    {
        // Camera rotation with Right Mouse Click
        if (Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            rotationX -= Input.GetAxis("Mouse Y") * lookSensitivity;
            rotationY += Input.GetAxis("Mouse X") * lookSensitivity;
            rotationX = Mathf.Clamp(rotationX, -85f, 85f);

            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        // Camera movement
        float speed = movementSpeed * (Input.GetKey(KeyCode.LeftShift) ? boostMultiplier : 1f);

        float moveX = Input.GetAxis("Horizontal"); // A, D
        float moveZ = Input.GetAxis("Vertical");   // W, S
        float moveY = 0f;

        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.PageUp)) moveY += 1f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.PageDown)) moveY -= 1f;

        Vector3 moveDirection = (transform.forward * moveZ + transform.right * moveX + Vector3.up * moveY).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
