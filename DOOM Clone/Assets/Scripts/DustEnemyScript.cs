using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DustEnemyScript : MonoBehaviour {

    private bool _attack, playerEncountered = false;
    [SerializeField] float agroDistance, meleeRange, attackCD, CDTime;
    [SerializeField] GameObject Player;
    public float EnemyHealth;
    private Rigidbody EnemyRB;

    NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();

        attackCD = 4;
        CDTime = attackCD;
        EnemyRB = GetComponent<Rigidbody>();

        navAgent.stoppingDistance = meleeRange;
        
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(Player.transform);

        float dist = Vector3.Distance(transform.position, Player.transform.position);

        if (dist < agroDistance && dist > meleeRange && playerEncountered == false) {
            navAgent.SetDestination(Player.transform.position);
            playerEncountered = true;
        }

        if(playerEncountered)
        {
            navAgent.SetDestination(Player.transform.position);
        }
        //Raycast that constantly checks if player is in melee range, and initiates attack if true

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, meleeRange))
        {
            PlayerScript target = hit.transform.GetComponent<PlayerScript>();

            if(target != null && target.tag == "Player")
            {
                _attack = true;                
            }
        }

        //calls the attack function

        if(_attack == true && attackCD == 4 && Player.GetComponent<CombatScript>().isSucking == false)
        {
            Attack();            
        }

        // starts attackCD

        if(_attack == true)
        {
            attackCD -= Time.deltaTime;            
        }

        
        //resets attack

        if(attackCD <= 0)
        {
            _attack = false;
            attackCD = 4;
            
        }


        //resets enemy y position
        /*if(attackCD <= 3.50f)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }*/

       
	}


    public void TakeDamage(float dmg)
    {
        EnemyHealth -= dmg;
        if (EnemyHealth <= 0f)
        {
            Die();
        }

    }

    //function that calls for minus playerHealth
    void Attack()
    {
        Debug.Log("PLayerHit");
        Player.GetComponent<PlayerScript>().playerHealth -= 1;

        //makes enemy jump just to show the attack went through, placeholder anim
        //transform.Translate(new Vector3(0f, 0.2f, 0f));
        
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, agroDistance);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
