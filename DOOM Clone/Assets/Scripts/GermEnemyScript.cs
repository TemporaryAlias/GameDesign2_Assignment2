using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GermEnemyScript : MonoBehaviour {

    //This script so far holds enemy HP, will later be used to determine enemy behaviour.

    public float EnemyHealth;
    [SerializeField] GameObject Player, EnemyProjectile;
    [SerializeField] Transform projectileSpawnPoint;
    [SerializeField] float shotTimer, shotCD, speed, dist, agroRange, rangedAttackDist;

    NavMeshAgent navAgent;

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = rangedAttackDist;
        Player = GameObject.FindGameObjectWithTag("Player");
        shotTimer = Random.Range(1, 5);
        shotCD = shotTimer;
    }

    private void FixedUpdate()
    {
        transform.LookAt(Player.transform);

        dist = Vector3.Distance(transform.position, Player.transform.position);
        
        if(dist > rangedAttackDist && dist < agroRange) {
            navAgent.SetDestination(Player.transform.position);
        } 

        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
        }

        if (dist <= rangedAttackDist && shotTimer <= 0)
        {
            //Instantiate(EnemyProjectile, new Vector3(transform.position.x, transform.position.y + 150, transform.position.z),transform.rotation);
            Instantiate(EnemyProjectile, projectileSpawnPoint.position, transform.rotation);
            shotTimer = shotCD;
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

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, agroRange);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, rangedAttackDist);
    }

}
