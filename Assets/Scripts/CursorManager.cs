using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        //Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        int level = SceneManager.GetActiveScene().buildIndex; // Assuming levels are in order
        float cursorScale = Mathf.Max(1f / level, 0.1f); // Decreases size with higher levels
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
        //cursorTexture.Reinitialize((int)(cursorTexture.width * cursorScale), (int)(cursorTexture.height * cursorScale));
    }
}
