using UnityEngine;

public class SpaceJetAI : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float aggressiveSpeed = 15f;
    public float detectionRange = 0.00001f;
    public AudioClip collisionSound;
    private bool isAggressive = false;

    private Transform playerTransform;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // Assuming the player has the "Player" tag
    }

    void Update()
    {
        if (isAggressive)
        {
            // Move faster and more aggressively towards the player
            MoveTowardsPlayer(aggressiveSpeed);
        }
        else
        {
            // Normal movement or patrol logic
            MoveTowardsPlayer(normalSpeed);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enter aggressive mode after colliding with the player
            isAggressive = true;
            Debug.Log("Space Jet has entered aggressive mode!");

            // Play collision sound at the collision point
            AudioSource.PlayClipAtPoint(collisionSound, transform.position);

            // Optionally apply some force back to the player
            rb.AddForce(-collision.contacts[0].normal * aggressiveSpeed, ForceMode.Impulse);
        }
    }

    void MoveTowardsPlayer(float speed)
    {
        if (Vector3.Distance(transform.position, playerTransform.position) <= detectionRange)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }
}
