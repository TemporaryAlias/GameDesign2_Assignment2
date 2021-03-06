﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TEMP
using UnityEngine.UI;

public class CombatScript : MonoBehaviour {

    private Animator playerAnim;
    [SerializeField] Animator swordAnim, gunAnim;
    [SerializeField] GameObject _blueWeapon, _redWeapon, _redEnemy, _blueEnemy, _swordHitBox, _vacuum, _reloader;
    GameObject dustEnemy;
    [SerializeField] ParticleSystem muzzleFlash;
    public float damage = 10f, attackCD;
    public float maxAmmo, currentAmmo, reloadTimer, suckTimer = 3, staticForce = 0;
    [SerializeField] Slider ammoBar, reloadBar; 
    private float cdTimer;
    
    private bool _isRed, _isBlue, _hasAttacked, _vaccuumOn, canHoover;
    public bool swingHit = false, isSucking;
    public GameObject _impactEffect;
    private GameObject keyFinder;

    //TEMP: Text to notify if wrong weapon was used
    public Text notifyText;

    // Use this for initialization
    void Start () {

        keyFinder = Resources.Load<GameObject>("Rebind");
        playerAnim = GetComponent<Animator>();
        //swordAnim = GetComponentInChildren
        _isBlue = true;
        cdTimer = attackCD;
        currentAmmo = maxAmmo;
        ammoBar.value = AmmoCount();
        
        //TEMP: Set notify text to nothing
        notifyText.text = "";
    }
	
	// Update is called once per frame
	void Update () {

        dustEnemy = GameObject.FindGameObjectWithTag("RedEnemy");
        //Switches between the two weapon types. Blue Weapon is the shotgun, RedWeapon is the sword/melee

        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown(keyFinder.GetComponent<KeyBindsScript>().ranged))
        {
            BlueWeapon();
            _isBlue = true;
            _isRed = false;
            _vaccuumOn = false;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown(keyFinder.GetComponent<KeyBindsScript>().melee))
        {
            RedWeapon();
            _isRed = true;
            _isBlue = false;
            _vaccuumOn = false;
        } else if(Input.GetKeyDown(keyFinder.GetComponent<KeyBindsScript>().hoover) && canHoover)
        {
            Vacuum();
            _vaccuumOn = true;
            _isRed = false;
            _isBlue = false;
        }


        //Calls the shoot function, and sets the cooldown (attackCD) for the shot, also calls animation

        if (_isBlue && Input.GetMouseButtonDown(0) && _hasAttacked == false && isSucking == false && currentAmmo > 0)
        {
            currentAmmo -= 1;
            _hasAttacked = true;
            //_blueEnemy.SetActive(false);
            gunAnim.SetTrigger("Shoot");
            Shoot();
        } 

        //Sword swing, calls the swing function and sets the cooldown for the attack, also calls swing animation

        else if (_isRed && Input.GetMouseButtonDown(0) && _hasAttacked == false && isSucking == false)
        {
            _hasAttacked = true;
            //_redEnemy.SetActive(false);
            
            
            Swing();
        }

        if(Input.GetMouseButton(0) && _vaccuumOn == true && isSucking == false)
        {
            isSucking = true;
            suckTimer -= Time.deltaTime;
            SuckemUp();
        } else
        {
            isSucking = false;
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

        ammoBar.value = AmmoCount();

        if(Input.GetKey(KeyCode.E))
        {
            Reload();
        }
        else
        {
            _reloader.SetActive(false);
        }

        if (reloadTimer <= 0 && currentAmmo <= 20)
        {
            currentAmmo += 2;
            reloadTimer = 0.5f;
        } else if(currentAmmo >= 20)
        {
            currentAmmo = 20;
            reloadTimer = 0.5f;
        }

        if(staticForce >= 8)
        {
            canHoover = true;
        }

        if(suckTimer <= 0)
        {
            canHoover = false;
            staticForce = 0;
            suckTimer = 3;
            BlueWeapon();
            _isBlue = true;
            _isRed = false;
            _vaccuumOn = false;
        }
    }

    float AmmoCount()
    {
        return currentAmmo / maxAmmo;
    }

    //function for holding blue weapon, aka shotgun
    void BlueWeapon()
    {
        _blueWeapon.SetActive(true);
        _redWeapon.SetActive(false);
        _vacuum.SetActive(false);
    }

    //function for holding red weapon, aka sword
    void RedWeapon()
    {
        _blueWeapon.SetActive(false);
        _redWeapon.SetActive(true);
        _vacuum.SetActive(false);
    }
    //function for vacuum
    void Vacuum()
    {
        _vacuum.SetActive(true);
        _blueWeapon.SetActive(false);
        _redWeapon.SetActive(false);

    }

    //shoot function, creates a raycast in front of player, if it hits the "Blue Enemy", that enemy's script is called and it takes damage

    public void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            GermEnemyScript target1 = hit.transform.GetComponent<GermEnemyScript>();
            StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();

            //TEMP: Get targets gameobject to test if wrong weapon used
            GameObject targetGO = hit.transform.gameObject;

            if (target1 != null && target1.tag == "BlueEnemy")
            {
                staticForce += 1;
                target1.TakeDamage(damage);
                //TEMP: Notify on wrong weapon used
            }

            if (target2 != null && target2.tag == "Stain Enemy" && target2.stainHealth > 10)
            {
                staticForce += 1;
                target2.TakeDamage(damage);
            }
            else if (targetGO != null && targetGO.tag == "RedEnemy")
            {
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
            StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();

            //TEMP: Get targets gameobject to test if wrong weapon used
            GameObject targetGO = hit.transform.gameObject;

            if(target != null && target.tag == "RedEnemy")
            {
                staticForce += 1;
                swingHit = true;
                
                //TEMP: Notify on wrong weapon used
            }

            if (targetGO != null && targetGO.tag == "BlueEnemy") {
                StartCoroutine("WrongWeaponNotify");
            }

            if (target2 != null && target2.tag == "Stain Enemy" && target2.stainHealth == 10)
            {
                swingHit = true;
                Invoke("SendDamage2", 0.25f);
            }

            if (swingHit)
            {
                swordAnim.SetTrigger("swing2");

                if (target.tag == "RedEnemy")
                {
                    Invoke("SendDamage1", 0.25f);
                    //target.TakeDamage(damage);
                }

                if (target2.tag == "Stain Enemy")
                {
                    Invoke("SendDamage2", 0.25f);
                }

                swingHit = false;
            }
        }

        if (swingHit == false)
        {
            swordAnim.SetTrigger("swing");
        }
       
    }

    void SendDamage1()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 150)) {

            DustEnemyScript target = hit.transform.GetComponent<DustEnemyScript>();
            target.TakeDamage(damage);
            swingHit = false;
        }
        
    }

    void SendDamage2()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 150))
        {

            StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();
            target2.TakeDamage(damage);
            swingHit = false;
        }
        
    }

    public void SuckemUp()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            DustEnemyScript target = hit.transform.GetComponent<DustEnemyScript>();
            StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();
            GermEnemyScript target3 = hit.transform.GetComponent<GermEnemyScript>();

            //TEMP: Get targets gameobject to test if wrong weapon used
            GameObject targetGO = hit.transform.gameObject;

            if (target != null && target.tag == "RedEnemy")
            {
                target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, 0.8f);
                //TEMP: Notify on wrong weapon used
            }

            if( target3 != null && target3.tag == "BlueEnemy")
            {
                target3.transform.position = Vector3.MoveTowards(target3.transform.position, transform.position, 0.8f);
            }

            if (targetGO != null && targetGO.tag == "BlueEnemy")
            {
                StartCoroutine("WrongWeaponNotify");
            }
            
        }
    }

    public void Reload()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            Debug.Log(hit.transform.name);

            GameObject target = hit.transform.gameObject;

            if(target != null && target.name == "Reload Station")
            {
                _reloader.SetActive(true);
                reloadBar.value = AmmoCount();
                reloadTimer -= Time.deltaTime;
            } 
        }
        
        
    }

    //TEMP: Coroutine to handle notify text on wrong weapon used for playtest
    IEnumerator WrongWeaponNotify() {
        notifyText.text = "Attack is ineffective! Try a different weapon!";

        yield return new WaitForSeconds(2);

        notifyText.text = "";
    }

    /*private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "RedEnemy" && isSucking)
        {
            Destroy(dustEnemy);
        } 
    }*/

   
}
