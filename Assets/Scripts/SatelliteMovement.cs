using UnityEngine;
using UnityEngine.SceneManagement;

public class SatelliteMovement : MonoBehaviour
{
    public Transform centerPoint; // The point around which the satellite will orbit
    public float orbitSpeed = 5f; // Speed of the orbit
    public float orbitRadius = 10f; // Radius of the orbit
    public float detectionRange = 10f; // Range to detect the mouse pointer
    public float attackSpeed = 10f; // Speed when attacking the mouse pointer
    private bool isAttacking = false; // Whether the satellite is attacking the mouse pointer
    private float angle; // Current angle of the orbit

    void Update()
    {
        if (!isAttacking)
        {
            // Orbit around the center point
            OrbitAroundCenter();

            // Detect the mouse pointer
            DetectMousePointer();
        }
        else
        {
            // Attack the mouse pointer
            AttackMousePointer();
        }
    }

    void OrbitAroundCenter()
    {
        angle += orbitSpeed * Time.deltaTime;
        float x = Mathf.Cos(angle) * orbitRadius;
        float z = Mathf.Sin(angle) * orbitRadius;
        transform.position = new Vector3(x, transform.position.y, z) + centerPoint.position;
    }

    void DetectMousePointer()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        //{
        //    float distanceToMouse = Vector3.Distance(transform.position, hit.point);

        //    if (distanceToMouse <= detectionRange)
        //    {
        //        isAttacking = true;
        //    }
        //}
    }

    void AttackMousePointer()
    {
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePosition.z = transform.position.z; // Keep z-axis constant if 2D

        //// Move toward the mouse pointer
        //transform.position = Vector3.MoveTowards(transform.position, mousePosition, attackSpeed * Time.deltaTime);

        //// Check for collision with mouse pointer
        //if (Vector3.Distance(transform.position, mousePosition) < 0.1f)
        //{
        //    EndGame();
        //}
    }

    void EndGame()
    {
        // Logic to end the game
        Debug.Log("Game Over: Satellite has attacked the player!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart the level
    }
}
