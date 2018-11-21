using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Rigidbody _myRb;
    public float playerHealth;
    
    [SerializeField] float speed;
    

	// Use this for initialization
	void Start () {
        _myRb = GetComponent<Rigidbody>();
        
        //locks the cursor, making it unusable

        Cursor.lockState = CursorLockMode.Locked;
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
        

        if(playerHealth <= 0)
        {
            Die();
        }
       
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
