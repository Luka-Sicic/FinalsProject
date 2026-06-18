using UnityEngine;

public class Shotgun : Weapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public int pelletCount = 5;
    public float spreadAngle = 15f;

    private void Start()
    {
        maxAmmo = 7;
        ammo = maxAmmo;
    }

    public override void Fire()
    {
        if (ammo <= 0) return;

        ammo--;

        PlayFireSound();

        NoiseManager.MakeNoise(transform.position, noiseRadius);

        for (int i = 0; i < pelletCount; i++)
        {
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            Quaternion spreadRotation = firePoint.rotation * Quaternion.Euler(0, 0, randomSpread);
            
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, spreadRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = (Vector2)(spreadRotation * Vector3.right) * bulletSpeed;
            }
        }
    }
}
