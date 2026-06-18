using UnityEngine;

public class Pistol : Weapon
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    private void Start()
    {
        maxAmmo = 10;
        ammo = maxAmmo;
    }

    public override void Fire()
    {
        if (ammo <= 0) return;

        ammo--;

        PlayFireSound();

        NoiseManager.MakeNoise(transform.position, noiseRadius);

        
GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
    }
}
