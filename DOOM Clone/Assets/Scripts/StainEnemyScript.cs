using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StainEnemyScript : MonoBehaviour {

    public float stainHealth;
    [SerializeField] float groundCd, groundTimer, germCd, germTimer, shotTimer, shotCD, agroRange, rangedAttackDist, dist;
    [SerializeField] GameObject GermEnemy, GermSpawn1, GermSpawn2, GermSpawn3, Player, EnemyProjectile;
    [SerializeField] Transform projectileSpawnPoint;
    private bool goToGround, inGround, createGerm;
    Vector3 startYPos;


	// Use this for initialization
	void Start () {
        //goToGround = true;
        groundTimer = 5;
        groundCd = 15;
        germTimer = germCd;
        shotTimer = Random.Range(1, 5);
        shotCD = shotTimer;
        startYPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {


        transform.LookAt(Player.transform);
        dist = Vector3.Distance(transform.position, Player.transform.position);

        if ((stainHealth == 60 || stainHealth == 30) && inGround == false)
        {
           
            goToGround = true;
            CreateGerm();
        }
       
        if(goToGround == true || inGround == true)
        {
            StainGround();
        }
        else if (inGround == false && goToGround == false)
        {
            transform.position = startYPos;
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

    void StainGround()
    {
        if (goToGround)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
            groundCd = 15;
            
            inGround = true;
        }

        if (inGround)
        {
            goToGround = false;
            groundTimer -= Time.deltaTime;
        }
        

        if (groundTimer <= 0)
        {
            inGround = false;
            stainHealth -= 10;
            groundTimer = 5;
           
        }
    }

    void CreateGerm()
    {        
        Instantiate(GermEnemy,GermSpawn1.transform.position, transform.rotation);
        Instantiate(GermEnemy, GermSpawn2.transform.position, transform.rotation);
        Instantiate(GermEnemy, GermSpawn3.transform.position, transform.rotation);
    }

    public void TakeDamage(float dmg)
    {
        stainHealth -= dmg;
        if (stainHealth <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        Destroy(gameObject);
    }
}
