using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    //This script so far holds enemy HP, will later be used to determine enemy behaviour.

    public float EnemyHealth;
    [SerializeField] GameObject Player;

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
