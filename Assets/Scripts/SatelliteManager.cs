using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteManager : MonoBehaviour
{
    public GameObject satellitePrefab;  // The prefab for the satellite
    public int numberOfSatellites = 2;  // Number of satellites to create
    public Transform centerPoint;       // The central point for orbiting

    void Start()
    {
        for (int i = 0; i < numberOfSatellites; i++)
        {
            // Instantiate a satellite prefab at the origin with no rotation
            GameObject satellite = Instantiate(satellitePrefab, Vector3.zero, Quaternion.identity);

            // Get the SatelliteMovement script component attached to the satellite
            SatelliteMovement movement = satellite.GetComponent<SatelliteMovement>();

            // Set the center point for the orbit
            movement.centerPoint = centerPoint;

            // Randomize the orbit radius, speed, and inclination for variation
            movement.orbitRadius = Random.Range(50.0f, 120.0f);  // Orbit radius variation
            movement.orbitPeriod = Random.Range(5400.0f, 7200.0f);  // Orbit period variation between 90 and 120 minutes
            movement.inclinationAngle = Random.Range(-45.0f, 45.0f);  // Inclination angle variation
        }
    }
}
