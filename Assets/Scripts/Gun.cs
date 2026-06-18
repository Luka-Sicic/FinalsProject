using UnityEngine;

public class Gun : Weapon
{
    public GameObject Bullet;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    private void Start()
    {
        if (maxAmmo == 0) maxAmmo = 10;
        ammo = maxAmmo;
    }

    public override void Fire()
    {
        if (ammo <= 0) return;
        ammo--;

        PlayFireSound();

        NoiseManager.MakeNoise(transform.position, noiseRadius);

        GameObject bullet = Instantiate(
Bullet,
            firePoint.position,
            firePoint.rotation
        );

        bullet.GetComponent<Rigidbody2D>().linearVelocity =
            firePoint.right * bulletSpeed;
    }
}