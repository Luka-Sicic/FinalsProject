using UnityEngine;

public class Uzi : Gun
{
    private void Awake()
    {
        isAutomatic = true;
        maxAmmo = 30;
        ammo = maxAmmo;
        fireRate = 0.1f;
        reloadDuration = 1.5f;
    }
}