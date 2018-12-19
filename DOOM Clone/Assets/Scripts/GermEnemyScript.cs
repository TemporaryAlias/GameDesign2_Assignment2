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
    [SerializeField] bool isAggro;
    NavMeshAgent navAgent;
    [SerializeField] Animator bodyAnim, leftArmAnim, rightArmAnim, mouthAnim, feetAnim;

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
            isAggro = true;
        } 

        if(isAggro)
        {
            navAgent.SetDestination(Player.transform.position);
            feetAnim.SetBool("Moving", true);
        }

        if (shotTimer > 0)
        {
            shotTimer -= Time.deltaTime;
        }

        if (dist <= rangedAttackDist && shotTimer <= 0)
        {
            //Instantiate(EnemyProjectile, new Vector3(transform.position.x, transform.position.y + 150, transform.position.z),transform.rotation);
            mouthAnim.SetTrigger("Attack");
            Instantiate(EnemyProjectile, projectileSpawnPoint.position, transform.rotation);
            shotTimer = shotCD;
        }
    }

    //void looking for any damage being given to the enemy (Combat Script). Then calls die funtion when hp is = 0

    public void TakeDamage (float dmg)
    {
        EnemyHealth -= dmg;
        transform.localScale = new Vector3((float)(transform.localScale.x * 0.75), (float)(transform.localScale.y * 0.75), (float)(transform.localScale.z * 0.75));

        if (EnemyHealth <= 0f)
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && Player.GetComponent<CombatScript>().isSucking)
        {
            Destroy(this.gameObject);
        }
    }

}
