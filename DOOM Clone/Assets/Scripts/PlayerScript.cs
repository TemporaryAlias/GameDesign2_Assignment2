﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    private Rigidbody _myRb;
    public float playerHealth, maxHealth;
    
    [SerializeField] float speed;

    [SerializeField] Slider healthBar;
    

	// Use this for initialization
	void Start () {
        _myRb = GetComponent<Rigidbody>();
        
        //locks the cursor, making it unusable

        Cursor.lockState = CursorLockMode.Locked;
        playerHealth = maxHealth;

        healthBar.value = CalculateHealth();
	}
	
	// Update is called once per frame
	void Update () {

        //simple movement script, will need to be tweaked later on to accomodate for player key binding.

        var xMove = Input.GetAxis("Horizontal");
        var yMove = Input.GetAxis("Vertical");

        //var movement = new Vector3(xMove, 0, yMove);

        
        //unlocks the cursor
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(yMove > 0 && Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed);
        } else if (yMove < 0 && Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed);
        }

        if(xMove < 0 && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Tab)))
        {
            transform.Translate(Vector3.left * speed);
        }
        else if (xMove > 0 && Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed);
        }

        healthBar.value = CalculateHealth();


        if (playerHealth <= 0)
        {
            Die();
        }
       
    }

    float CalculateHealth()
    {
        return playerHealth / maxHealth;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            Debug.Log("hit");
            playerHealth -= 1;
        }
    }


}
