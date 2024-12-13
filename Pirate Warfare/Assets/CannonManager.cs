using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonManager : MonoBehaviour
{
    public bool canShoot = true;
    //private bool reloading = false;
    public float shootForce = 750f;

    public GameObject cannonBallSpawnPoint;
    public GameObject cannonBallPrefab;
    public GameObject smokeParticleEffectPrefab;

    private AudioSource cannonFireSound;

    private void Start()
    {
        UnityEngine.Random.InitState(System.Environment.TickCount);

        cannonFireSound = GetComponent<AudioSource>();
    }

    public void Fire(float delay)
    {
        if (canShoot)
        {
            StartCoroutine(FireWithDelay(delay));
        }

    }
    private void Reload()
    {
        float reloadDelay = UnityEngine.Random.Range(20f, 40f);

        StartCoroutine(ReloadWithDelay(reloadDelay));
    }






    // Corutines to delay
    private IEnumerator FireWithDelay(float delay)
    {
        canShoot = false;

        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Logic to shoot the cannonball
        GameObject cannonball = Instantiate(cannonBallPrefab, cannonBallSpawnPoint.transform.position, cannonBallSpawnPoint.transform.rotation);
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();
        // Apply force in the direction the cannon is facing
        rb.AddForce(-cannonBallSpawnPoint.transform.right * shootForce, ForceMode.Impulse);


        PlaySoundClip();


        GameObject particleInstance = Instantiate(smokeParticleEffectPrefab, cannonBallSpawnPoint.transform.position, Quaternion.identity);
        particleInstance.transform.forward = -cannonBallSpawnPoint.transform.right; // turn towards cannon front face
        ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play(); // create smoke for cannon
        }

        // play cannon explosion


        Reload();
    }
    private IEnumerator ReloadWithDelay(float reloadDelay)
    {
        yield return new WaitForSeconds(reloadDelay);

        Debug.Log("Reloaded");
        canShoot = true;

    }



    public void PlaySoundClip()
    {
        if (cannonFireSound != null && cannonFireSound.clip != null)
        {
            cannonFireSound.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }
    }
}
