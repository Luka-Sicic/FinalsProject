using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour
{
    public int ammo;
    public int maxAmmo;
    public int spareReloads;
    public float reloadDuration = 2f;
    public float noiseRadius = 15f;
    public bool IsReloading { get; protected set; }

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] fireSounds;
    public AudioClip[] reloadSounds;

    protected void PlayFireSound()
    {
        if (audioSource != null && fireSounds != null && fireSounds.Length > 0)
        {
            AudioClip clip = fireSounds[Random.Range(0, fireSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    protected void PlayReloadSound()
    {
        if (audioSource != null && reloadSounds != null && reloadSounds.Length > 0)
        {
            AudioClip clip = reloadSounds[Random.Range(0, reloadSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    public abstract void Fire();

    public virtual void Reload()
    {
        if (spareReloads > 0 && ammo < maxAmmo && !IsReloading)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        IsReloading = true;
        Debug.Log(gameObject.name + " reloading...");

        PlayReloadSound();
        
        yield return new WaitForSeconds(reloadDuration);

        ammo = maxAmmo;
        spareReloads--;
        IsReloading = false;
        Debug.Log(gameObject.name + " reloaded. Spare reloads: " + spareReloads);
    }
}
