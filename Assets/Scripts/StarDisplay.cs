using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    public GameObject starPrefab;  // Assign this in the Inspector
    private StarDataLoader starLoader = new StarDataLoader();

    void Start()
    {
        // Load the star data
        List<StarDataLoader.Star> starData = starLoader.LoadData();

        Debug.Log(starData.Count);

        // Determine the current level
        int level = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        // Adjust the number of stars and size based on the level
        int starCount = starData.Count;
        float starSizeMultiplier = level == 0 ? 1.1f : 1.0f; // Make stars smaller in Level 2

        // Loop through each star and instantiate it in the scene
        int starCounter = 0;
        for (int i = 0; i < starCount; i++)
        {
            // for level 2 skip 9 stars out of 10 to reduce number of stars on display
            if (level == 1 && i % 10 != 0)
                continue;
            // for level 1 skip 4 stars out of 5 to reduce number of stars on display
            if (level == 0 && i % 5 != 0)
                continue;
            // Instantiate the star prefab at the position of the star
            GameObject starInstance = Instantiate(starPrefab, starData[i].position * 100, Quaternion.identity);

            // Calculate and set the size of the star using SetSize method
            float calculatedSize = SetSize(starData[i].size) * starSizeMultiplier; // Adjust size based on level
            starInstance.transform.localScale = Vector3.one * calculatedSize;
            starInstance.GetComponent<Renderer>().material.color = starData[i].colour;
            starCounter += 1;
        }

        Debug.Log("Total Stars displayed: " + starCounter);
    }

    // This method calculates the size of the star based on its magnitude.
    private float SetSize(float magnitude)
    {
        // The size decreases exponentially with increasing magnitude
        float size = Mathf.Pow(2.0f, (float)(-magnitude + 5) / 5.0f);

        float finalSize = size * 1.5f;
        //Debug.Log("Calculated star size: " + finalSize);
        return finalSize;
    }

}
