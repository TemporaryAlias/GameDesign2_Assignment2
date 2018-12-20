using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

    private Rigidbody _myRb;
    public float playerHealth, maxHealth;
    private float dustEnemycount, germEnemycount, stainEnemycount, maxEnemies;

    public float currentEnemies;
    
    [SerializeField] float speed;
    private GameObject keyFinder;
    [SerializeField] Slider healthBar;
    [SerializeField] Text remainingEnemies;

    [SerializeField] AudioClip deathClip, hitClip;

    [SerializeField] AudioSource audioSource;

    public bool dead;
    

	// Use this for initialization
	void Start () {
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
        //EnemyCount().ToString();
        remainingEnemies.text = EnemyCount().ToString(); 
	}
	
	// Update is called once per frame
	void Update () {

        if (!dead) {

            //simple movement script, will need to be tweaked later on to accomodate for player key binding.

            var xMove = Input.GetAxis("Horizontal");
            var yMove = Input.GetAxis("Vertical");

            //var movement = new Vector3(xMove, 0, yMove);


            //unlocks the cursor
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            if (Input.GetKey(keyFinder.GetComponent<KeyBindsScript>().forward))
            {
                transform.Translate(Vector3.forward * speed);
            } else if (Input.GetKey(keyFinder.GetComponent<KeyBindsScript>().back))
            {
                transform.Translate(Vector3.back * speed);
            }

            if ((Input.GetKey(keyFinder.GetComponent<KeyBindsScript>().left) || Input.GetKey(KeyCode.Tab)))
            {
                transform.Translate(Vector3.left * speed);
            }
            else if (Input.GetKey(keyFinder.GetComponent<KeyBindsScript>().right))
            {
                transform.Translate(Vector3.right * speed);
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

    public float EnemyCount()
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
        
        if (collision.gameObject.tag == "EnemyProjectile" && GetComponent<CombatScript>().isSucking == false && this.isActiveAndEnabled)
        {
            playerHealth -= 1;

            audioSource.PlayOneShot(hitClip);
        }
    }

   


}
