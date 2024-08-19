using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public int maxHits = 3;
    private int currentHits = 0;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("NPCSpaceJet"))
        {
            currentHits++;
            Debug.Log("Space jet collided! Hits: " + currentHits);

            if (currentHits >= maxHits)
            {
                //EndGame();
            }
        }
    }

    void EndGame()
    {
        Debug.Log("Game Over! You've been hit three times.");
        // Example game over logic:
        // - Stop the player from moving
        // - Show a game over UI
        // - Restart the level or quit the game
       
        // Display Game Over UI here
        GameManager.instance.EndGame();  // Call the end game method
        Time.timeScale = 0; // This freezes the game
    }
}
