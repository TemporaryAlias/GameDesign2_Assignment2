using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DustEnemyScript : MonoBehaviour {

    private bool _attack, _attack2, isAggro, nearPlayer;
    [SerializeField] float agroDistance, meleeRange, attackCD, CDTime;
    [SerializeField] GameObject Player;
    public float EnemyHealth;
    private Rigidbody EnemyRB;
    [SerializeField] Animator anim;
    NavMeshAgent navAgent;

    [SerializeField] AudioClip deathSound, playerHitClip, hitClip;

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        navAgent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();
        
        //attackCD = 2;
        //CDTime = attackCD;
        EnemyRB = GetComponent<Rigidbody>();

        navAgent.stoppingDistance = meleeRange;
        
	}
	
	// Update is called once per frame
	void Update () {

        anim.SetBool("Chasing", isAggro);
        anim.SetBool("nearPlayer", nearPlayer);

        transform.LookAt(Player.transform);

        float dist = Vector3.Distance(transform.position, Player.transform.position);

        if (dist < agroDistance && dist > meleeRange) {
            isAggro = true;
        }

        if(isAggro)
        {            
            navAgent.SetDestination(Player.transform.position);
        }

        if(dist == meleeRange)
        {
            nearPlayer = true;
        } else
        {
            nearPlayer = false;
        }

        //Raycast that constantly checks if player is in melee range, and initiates attack if true

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, meleeRange))
        {
            PlayerScript target = hit.transform.GetComponent<PlayerScript>();
            MouseMovementScript target2 = hit.transform.GetComponent<MouseMovementScript>();

            if(target != null && target.tag == "Player")
            {
                _attack = true;                
            }

            if (target2 != null)
            {
                _attack2 = true;
            }
        }

        //calls the attack function

        if(_attack == true && attackCD <= 0 && Player.GetComponent<CombatScript>().isSucking == false)
        {
            anim.SetTrigger("Attack");
            Attack();            
        }

        if (_attack2 == true && attackCD <= 0 && Player.GetComponent<MouseCombatScript>().isSucking == false)
        {
            anim.SetTrigger("Attack");

            Attack();
        }


        // starts attackCD

        if(_attack == true || _attack2 == true)
        {
            attackCD -= Time.deltaTime;            
        }

        
        //resets attack

        if(attackCD >= 0)
        {
            _attack = false;
            _attack2 = false;
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
        transform.localScale = new Vector3((float)(transform.localScale.x * 0.75), (float)(transform.localScale.y * 0.75), (float)(transform.localScale.z * 0.75));

        audioSource.PlayOneShot(hitClip);

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

        audioSource.PlayOneShot(playerHitClip);

        Player.GetComponent<MouseMovementScript>().playerHealth -= 1;

        attackCD = 4;

        //makes enemy jump just to show the attack went through, placeholder anim
        //transform.Translate(new Vector3(0f, 0.2f, 0f));

    }

    void Die()
    {
        LevelManager.instance.PlaySound(deathSound);
        LevelManager.instance.dustKilled += 1;

        Destroy(gameObject);

        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, agroDistance);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && (Player.GetComponent<CombatScript>().isSucking) || Player.GetComponent<MouseCombatScript>().isSucking)
        {
            Destroy(this.gameObject);
        }
    }
}
