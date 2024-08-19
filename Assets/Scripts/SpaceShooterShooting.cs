using UnityEngine;

public class SpaceShooterShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign this in the Inspector
    public Transform bulletSpawnPoint; // Assign the bullet spawn point
    public float bulletSpeed = 50.0f;
    public Camera mainCamera; // Assign the main camera in the Inspector
    public Camera bulletCameraPrefab; // Assign the bullet camera prefab

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Start()
    {
        // Ensure the main camera is active and bullet camera is inactive initially
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(true);
        }

        if (bulletCameraPrefab != null)
        {
            bulletCameraPrefab.gameObject.SetActive(false);
        }
    }

    void Shoot()
    {
        // Instantiate the bullet at the spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Attach the bullet camera to the bullet
        Camera bulletCam = Instantiate(bulletCameraPrefab, bullet.transform);
        bulletCam.transform.localPosition = Vector3.zero; // Position camera at the bullet's center
        bulletCam.transform.localRotation = Quaternion.identity;

        // Set the bullet's velocity
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * bulletSpeed;

        // Switch to bullet camera
        mainCamera.gameObject.SetActive(true);
        bulletCam.gameObject.SetActive(false);

        // Destroy the bullet and camera after a few seconds
        Destroy(bullet, 10.0f);
        Destroy(bulletCam.gameObject, 10.0f); // This is optional, depending on how long you want to keep the camera
    }
}
