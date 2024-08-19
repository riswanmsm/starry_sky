using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    public GameObject explosionEffect; // Assign a particle effect prefab for the explosion
    public int pointPerHit = 10; // Points awarded per star hit

    void OnTriggerEnter(Collider other)
    {
        // Check if the bullet collides with a star
        if (other.CompareTag("Star"))
        {
            // Instantiate the explosion effect at the star's position
            Instantiate(explosionEffect, other.transform.position, other.transform.rotation);

            // Award points
            GameManager.instance.AddScore(pointPerHit);

            // Destroy the star and the bullet
            Destroy(other.gameObject);
            Destroy(gameObject);
            // Debug.Log("Star has destroyed");
        }

        // Check if the bullet collide with a space jet
        if (other.CompareTag("NPCSpaceJet"))
        {
            Debug.Log("Bullet hit NPC jet!");

            // Instantiate the explosion effect at the jet's position
            Instantiate(explosionEffect, other.transform.position, other.transform.rotation);

            // Register the hit with the GameManager
            GameManager.instance.RegisterHit();

            // Destroy the space jet and the bullet
            //Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

        void EndGame()
    {
        Debug.Log("Game Over! You've been hit three times.");
        // Example game over logic:
        // - Stop the player from moving
        // - Show a game over UI
        // - Restart the level or quit the game
        Time.timeScale = 0; // This freezes the game
                            // Display Game Over UI here
    }
}
