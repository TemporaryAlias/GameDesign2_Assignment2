using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TEMP
using UnityEngine.UI;

public class CombatScript : MonoBehaviour {

    private Animator playerAnim;
    [SerializeField] Animator swordAnim, gunAnim;
    [SerializeField] GameObject _blueWeapon, _redWeapon, _redEnemy, _blueEnemy, _swordHitBox;
    [SerializeField] ParticleSystem muzzleFlash;
    public float damage = 10f, attackCD;
    private float cdTimer;
    private bool _isRed, _isBlue, _hasAttacked;
    public GameObject _impactEffect;

    //TEMP: Text to notify if wrong weapon was used
    public Text notifyText;

    // Use this for initialization
    void Start () {
        playerAnim = GetComponent<Animator>();
        //swordAnim = GetComponentInChildren
        _isBlue = true;
        cdTimer = attackCD;

        //TEMP: Set notify text to nothing
        notifyText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
		
        //Switches between the two weapon types. Blue Weapon is the shotgun, RedWeapon is the sword/melee

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            BlueWeapon();
            _isBlue = true;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            RedWeapon();
            _isBlue = false;
        }


        //Calls the shoot function, and sets the cooldown (attackCD) for the shot, also calls animation

        if (_isBlue && Input.GetMouseButtonDown(0) && _hasAttacked == false)
        {
            _hasAttacked = true;
            //_blueEnemy.SetActive(false);
            gunAnim.SetTrigger("Shoot");
            Shoot();
            
        } 

        //Sword swing, calls the swing function and sets the cooldown for the attack, also calls swing animation

        else if (_isBlue == false && Input.GetMouseButtonDown(0) && _hasAttacked == false)
        {
            _hasAttacked = true;
            //_redEnemy.SetActive(false);
            swordAnim.SetTrigger("swing");
            _swordHitBox.SetActive(true);
            Swing();
        }

        //Starts the cooldown timer once Shoot function has been called

        if(_hasAttacked)
        {
            attackCD -= Time.deltaTime;
        }

        //Resets the cooldown and the shot status once the timer hits 0

        if(attackCD <= 0)
        {
            attackCD = cdTimer;
            _hasAttacked = false;
        }
    }

    //function for holding blue weapon, aka shotgun
    void BlueWeapon()
    {
        _blueWeapon.SetActive(true);
        _redWeapon.SetActive(false);
    }

    //function for holding red weapon, aka sword
    void RedWeapon()
    {
        _blueWeapon.SetActive(false);
        _redWeapon.SetActive(true);
    }

    //shoot function, creates a raycast in front of player, if it hits the "Blue Enemy", that enemy's script is called and it takes damage

    public void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            GermEnemyScript target = hit.transform.GetComponent<GermEnemyScript>();

            //TEMP: Get targets gameobject to test if wrong weapon used
            GameObject targetGO = hit.transform.gameObject;

            if (target != null && target.tag == "BlueEnemy")
            {
                target.TakeDamage(damage);
                //TEMP: Notify on wrong weapon used
            }
            else if (targetGO != null && targetGO.tag == "RedEnemy") {
                StartCoroutine("WrongWeaponNotify");
            }

            //instantiates a particle system to simulate shot hitting the target, currently not working, no idea why
            GameObject impactGO = Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impactGO.GetComponent<ParticleSystem>().Play();
        }
       

    }

    //swing function, works in the same way as the shoot function, instead looking for "Red Enemy".

    public void Swing()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 150))
        {
            Debug.Log(hit.transform.name);

            DustEnemyScript target = hit.transform.GetComponent<DustEnemyScript>();

            //TEMP: Get targets gameobject to test if wrong weapon used
            GameObject targetGO = hit.transform.gameObject;

            if(target != null && target.tag == "RedEnemy")
            {
                target.TakeDamage(damage);
                //TEMP: Notify on wrong weapon used
            } else if (targetGO != null && targetGO.tag == "BlueEnemy") {
                StartCoroutine("WrongWeaponNotify");
            }
        }
    }

    //TEMP: Coroutine to handle notify text on wrong weapon used for playtest
    IEnumerator WrongWeaponNotify() {
        notifyText.text = "Attack is ineffective! Try a different weapon!";

        yield return new WaitForSeconds(2);

        notifyText.text = "";
    }
}
