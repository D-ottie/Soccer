using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 5;
    //private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public GameObject enemyGoal;
    public int powerUpDuration = 5;
    public int powerBoostDuration = 2;

    private float normalStrength = 25; // how hard to hit enemy without powerup
    private float powerupStrength = 50; // how hard to hit enemy with powerup

    public ParticleSystem dustparticle;
    public float xRange = 18;
    public float zRange = 27;

    public float explosionForce = 50;
    public float explosionRadius = 10;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
       // focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        playerRb.AddForce(Vector3.forward.normalized * verticalInput * speed * Time.deltaTime, ForceMode.VelocityChange); 
        playerRb.AddForce(Vector3.right.normalized * horizontalInput * speed * Time.deltaTime, ForceMode.VelocityChange);

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 1.5f;
            StartCoroutine(BoostStop());
            dustparticle.Play();
        }

        StayWithinPlayingRange();

        var enemies = FindObjectsOfType<EnemyX>();

        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                //Apply an explosion force that originates from our position.
                if(enemies[i] != null)
                {
                    enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce,transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
                }
            }

        }

        
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    IEnumerator BoostStop()
    {
        yield return new WaitForSeconds(powerBoostDuration);
        speed /= 1.5f;
        dustparticle.Stop();
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            //Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
            Vector3 lookDirection = (enemyGoal.transform.position - transform.position).normalized; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(lookDirection * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                //enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
                enemyRigidbody.AddForce(lookDirection * normalStrength , ForceMode.Impulse);
            }


        }
    }

    public void StayWithinPlayingRange()
    {
     
        if(transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange ,transform.position.y, transform.position.z);
        }

        if(transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange ,transform.position.y, transform.position.z);
        }

        if(transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x ,transform.position.y, zRange);
        }

        if(transform.position.z < -10.5)
        {
            transform.position = new Vector3(transform.position.x ,transform.position.y, -10.5f);
        }

    }



}
