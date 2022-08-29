using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject playerGoal;
    private GameObject enemyGoal;

    public bool scored = false;
    public bool failed = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player Goal");
        enemyGoal = GameObject.Find("Enemy Goal");
        speed = FindObjectOfType<SpawnManagerX>().enemySpeed;

    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed * Time.deltaTime);

        FindObjectOfType<PlayerControllerX>().StayWithinPlayingRange();

    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            FindObjectOfType<SpawnManagerX>().score += 10;
           // scored = true;
            Destroy(gameObject);
            
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            FindObjectOfType<SpawnManagerX>().score -= 5;
           // failed = true;
            Destroy(gameObject);
            
        }

    }

}
