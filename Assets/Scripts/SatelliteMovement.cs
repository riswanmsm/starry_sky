using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteMovement : MonoBehaviour
{
    public Transform centerPoint;   // The point around which the satellite orbits
    public float orbitRadius = 10.0f;  // The radius of the satellite's orbit
    public float orbitPeriod = 5400.0f;  // Time to complete one orbit in seconds (5400s = 90 minutes for LEO)
    public float inclinationAngle = 0.0f;  // Angle of inclination of the orbit

    private float currentAngle = 0.0f;

    private Renderer satelliteRenderer;
    private Material satelliteMaterial;

    void Start()
    {
        // Get the Renderer component attached to the satellite
        satelliteRenderer = GetComponent<Renderer>();
        if (satelliteRenderer != null)
        {
            // Get the material of the satellite to modify its emission color
            satelliteMaterial = satelliteRenderer.material;
        }
    }

    void Update()
    {
        // Calculate the angular speed based on the orbit period
        float angularSpeed = 2 * Mathf.PI / orbitPeriod;  // Full circle in the given period

        // Calculate the satellite's position in its orbit
        currentAngle += angularSpeed * Time.deltaTime;
        float x = orbitRadius * Mathf.Cos(currentAngle);
        float z = orbitRadius * Mathf.Sin(currentAngle);

        // Apply inclination by rotating around the z-axis
        Vector3 inclinedPosition = new Vector3(x, Mathf.Sin(inclinationAngle) * x, z);
        transform.position = centerPoint.position + inclinedPosition;

        // Apply blinking effect by modulating the emission color over time
        if (satelliteMaterial != null)
        {
            float blinkIntensity = Mathf.PingPong(Time.time, 1.0f);
            satelliteMaterial.SetColor("_EmissionColor", Color.white * blinkIntensity);
            Debug.Log("Satellite is blinking.");
        }
    }
}
