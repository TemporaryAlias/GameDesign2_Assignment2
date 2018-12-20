using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StainEnemyScript : MonoBehaviour {

    public float stainHealth;
    [SerializeField] float groundCd, groundTimer, germCd, germTimer, shotTimer, shotCD, agroRange, rangedAttackDist, dist;
    [SerializeField] GameObject GermEnemy, GermSpawn1, GermSpawn2, GermSpawn3, Player, EnemyProjectile;
    [SerializeField] Transform projectileSpawnPoint;
    private bool goToGround, inGround, createGerm, isAggro;
    Vector3 startYPos;

    [SerializeField] AudioClip deathClip, hitClip, shootClip, spawnClip;

    AudioSource audioSource;

    NavMeshAgent navAgent;

	// Use this for initialization
	void Start () {
        //goToGround = true;
        audioSource = GetComponent<AudioSource>();

        groundTimer = 5;
        groundCd = 15;
        germTimer = germCd;
        shotTimer = Random.Range(1, 5);
        shotCD = shotTimer;
        startYPos = transform.position;
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = rangedAttackDist;
	}
	
	// Update is called once per frame
	void FixedUpdate () {


        transform.LookAt(Player.transform);
        dist = Vector3.Distance(transform.position, Player.transform.position);

        if (dist > rangedAttackDist && dist < agroRange)
        {
            isAggro = true;
        }

        if(isAggro)
        {
            navAgent.SetDestination(Player.transform.position);
        }

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
            audioSource.PlayOneShot(shootClip);

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
        audioSource.PlayOneShot(spawnClip);

        Instantiate(GermEnemy,GermSpawn1.transform.position, transform.rotation);
        Instantiate(GermEnemy, GermSpawn2.transform.position, transform.rotation);
        Instantiate(GermEnemy, GermSpawn3.transform.position, transform.rotation);
    }

    public void TakeDamage(float dmg)
    {
        stainHealth -= dmg;
        audioSource.PlayOneShot(hitClip);

        if (stainHealth <= 0f)
        {
            Die();
        }

    }

    void Die()
    {
        LevelManager.instance.PlaySound(deathClip);

        Destroy(gameObject);
    }
}
