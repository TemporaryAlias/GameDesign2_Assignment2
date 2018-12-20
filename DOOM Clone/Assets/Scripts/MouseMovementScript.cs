using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseMovementScript : MonoBehaviour
{
    private Rigidbody _myRb;
    public float playerHealth, maxHealth;
    private float dustEnemycount, germEnemycount, stainEnemycount, maxEnemies, currentEnemies;

    [SerializeField] float speed, forBack;
    private GameObject keyFinder;
    [SerializeField] Slider healthBar;
    [SerializeField] Text remainingEnemies;


    [SerializeField] AudioClip deathClip, hitClip;

    [SerializeField] AudioSource audioSource;

    public bool dead, movingLeft;


    // Use this for initialization
    void Start()
    {

        audioSource = GetComponent<AudioSource>();

        keyFinder = Resources.Load<GameObject>("Rebind");
        _myRb = GetComponent<Rigidbody>();

        //locks the cursor, making it unusable

        Cursor.lockState = CursorLockMode.Locked;
        playerHealth = maxHealth;
        dustEnemycount = GameObject.FindGameObjectsWithTag("RedEnemy").Length;
        stainEnemycount = GameObject.FindGameObjectsWithTag("Stain Enemy").Length;
        germEnemycount = GameObject.FindGameObjectsWithTag("BlueEnemy").Length;
        maxEnemies = germEnemycount + dustEnemycount + stainEnemycount;
        currentEnemies = maxEnemies;
        healthBar.value = CalculateHealth();
        forBack = 0;
        //EnemyCount().ToString();
        movingLeft = false;
        remainingEnemies.text = EnemyCount().ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (!dead)
        {

            //simple movement script, will need to be tweaked later on to accomodate for player key binding.

            var xMove = Input.GetAxis("Horizontal");
            var yMove = Input.GetAxis("Vertical");

            //var movement = new Vector3(xMove, 0, yMove);


            //unlocks the cursor
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                forBack += 1;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0) 
            {
                forBack -= 1;
            }

            if (forBack >= 1)
            {
                forBack = 1;
                transform.Translate(Vector3.forward * speed);
            } 
            if(forBack <= -1)
            {
                forBack = -1;
                transform.Translate(Vector3.back * speed);
            }

            if (Input.GetMouseButtonDown(2))
            {
                if(movingLeft == true)
                {
                    Debug.Log("yett");
                    movingLeft = false;
                } else if (movingLeft == false)
                {
                    Debug.Log("left");
                    movingLeft = true;
                }
            }
            

            if(Input.GetMouseButton(1))
            {
                if (movingLeft)
                {
                    transform.Translate(Vector3.left * speed);
                }
                else
                {
                    transform.Translate(Vector3.right * speed);
                }
            }

            healthBar.value = CalculateHealth();
            remainingEnemies.text = "Remaining:\n" + EnemyCount().ToString();

            if (playerHealth <= 0)
            {
                Die();
            }

        }

    }

    float CalculateHealth()
    {
        return playerHealth / maxHealth;
    }

    float EnemyCount()
    {
        dustEnemycount = GameObject.FindGameObjectsWithTag("RedEnemy").Length;
        stainEnemycount = GameObject.FindGameObjectsWithTag("Stain Enemy").Length;
        germEnemycount = GameObject.FindGameObjectsWithTag("BlueEnemy").Length;
        return (dustEnemycount + germEnemycount + stainEnemycount);
    }

    void Die()
    {

        audioSource.PlayOneShot(deathClip);
        dead = true;
        LevelManager.instance.uiHandler.StartFadeOut(0);
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "EnemyProjectile" && GetComponent<MouseCombatScript>().isSucking == false)
        {
            Debug.Log("hit");
            playerHealth -= 1;

            audioSource.PlayOneShot(hitClip);
        }
    }
}
