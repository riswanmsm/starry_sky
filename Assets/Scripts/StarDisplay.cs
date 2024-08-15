using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDisplay : MonoBehaviour
{
    public GameObject starPrefab;  // Assign this in the Inspector
    private StarDataLoader starLoader = new StarDataLoader();

    void Start()
    {
        // Load the stars
        List<StarDataLoader.Star> stars = starLoader.LoadData();
        //Debug.Log("Calling LoadData...");

        // Loop through each star and instantiate it in the scene
        foreach (var star in stars)
        {
            //Debug.Log(star.position);
            // Instantiate the star prefab at the position of the star
            GameObject starInstance = Instantiate(starPrefab, star.position * 100, Quaternion.identity);

            // Set the size and color of the star
            starInstance.transform.localScale = Vector3.one * star.size;
            starInstance.GetComponent<Renderer>().material.color = star.colour;
        }
    }

    // This method calculates the size of the star based on its magnitude.
    private float SetSize(short magnitude)
    {
        // The size decreases exponentially with increasing magnitude
        float size = Mathf.Pow(2.0f, (float)(-magnitude + 5) / 5.0f);
        return size * 0.1f; // Adjust the scaling factor as needed
    }
}
