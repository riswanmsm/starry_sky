using UnityEngine;

public class SpaceShooterMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;

    void Update()
    {
        // Horizontal movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 horizontalMovement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        transform.Translate(horizontalMovement * speed * Time.deltaTime, Space.World);

        // Up and down movement
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 verticalMovement = new Vector3(0.0f, 0.0f, moveVertical);
        transform.Translate(verticalMovement * speed * Time.deltaTime, Space.World);

        // Rotation based on mouse movement or arrow keys
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        transform.Rotate(-verticalRotation, horizontalRotation, 0, Space.Self);
    }
}
