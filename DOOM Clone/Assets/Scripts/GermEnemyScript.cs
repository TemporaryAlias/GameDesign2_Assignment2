using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GermEnemyScript : MonoBehaviour {

    //This script so far holds enemy HP, will later be used to determine enemy behaviour.

    public float EnemyHealth;
    [SerializeField] GameObject Player, EnemyProjectile;
    [SerializeField] float shotTimer, shotCD, speed, dist;

    private void Start()
    {
        shotCD = Random.Range(1, 10);
        shotTimer = shotCD;
    }

    private void FixedUpdate()
    {
        transform.LookAt(Player.transform);

        dist = Vector3.Distance(transform.position, Player.transform.position);
        
        if(dist > 150)
        {
            Vector3.MoveTowards(transform.position, Player.transform.position, speed * Time.deltaTime);
        } 

        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
        }

        if (shotTimer <= 0)
        {
            Instantiate(EnemyProjectile, new Vector3(transform.position.x, transform.position.y + 150, transform.position.z),transform.rotation);
            shotTimer = 6;
        }
    }

    //void looking for any damage being given to the enemy (Combat Script). Then calls die funtion when hp is = 0

    public void TakeDamage (float dmg)
    {
        EnemyHealth -= dmg;
        if(EnemyHealth <= 0f)
        {
            Die();
        }       
        
    }

    //destroys gameobject when HP = 0

    void Die()
    {
        Destroy(gameObject);
    }

    
}
