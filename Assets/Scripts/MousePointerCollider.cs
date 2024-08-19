using UnityEngine;

public class MousePointerCollider : MonoBehaviour
{
    public ScoreManager scoreManager;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Satellite"))
            {
                // End the game if the mouse hits the satellite
                //scoreManager.EndLevel();
            }
        }
    }
}
