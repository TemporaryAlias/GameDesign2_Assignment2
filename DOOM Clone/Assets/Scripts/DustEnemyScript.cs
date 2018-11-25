using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustEnemyScript : MonoBehaviour {

    private bool _attack;
    [SerializeField] float attackCD, CDTime;
    [SerializeField] GameObject Player;
    public float EnemyHealth;
    private Rigidbody EnemyRB;

	// Use this for initialization
	void Start () {
        attackCD = 4;
        CDTime = attackCD;
        EnemyRB = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {

        //Raycast that constantly checks if player is in melee range, and initiates attack if true

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, 150))
        {
            PlayerScript target = hit.transform.GetComponent<PlayerScript>();

            if(target != null && target.tag == "Player")
            {
                _attack = true;                
            }
        }

        //calls the attack function

        if(_attack == true && attackCD == 4)
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
        if(attackCD <= 3.50f)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }

       
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
        transform.Translate(transform.up * 50);
        
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
