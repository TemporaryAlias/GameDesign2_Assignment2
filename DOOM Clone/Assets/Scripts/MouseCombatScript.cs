﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCombatScript : MonoBehaviour
{
    private Animator playerAnim;
    [SerializeField] Animator swordAnim, gunAnim;
    [SerializeField] GameObject _blueWeapon, _redWeapon, _redEnemy, _blueEnemy, _vacuum, _reloader;
    GameObject dustEnemy, allDustBunnies;
    [SerializeField] ParticleSystem muzzleFlash;
    public float damage = 10f, attackCD;
    public float maxAmmo, currentAmmo, reloadTimer, suckTimer = 3, staticForce = 0, maxStatic;
    [SerializeField] Slider ammoBar, reloadBar, vacBar;
    private float cdTimer;

    private bool _isRed, _isBlue, _hasAttacked, _vaccuumOn, canHoover;
    public bool swingHit = false, isSucking, melee;
    public GameObject _impactEffect, dustImpactEffect;
    private GameObject keyFinder;

    [SerializeField] float shootRange, meleeRange;
    [SerializeField] MouseMovementScript player;

    [SerializeField] AudioClip meleeClip, rangedClip, vacClip, reloadClip;

    AudioSource audioSouce;

    //TEMP: Text to notify if wrong weapon was used
    public Text notifyText;

    // Use this for initialization
    void Start()
    {
        audioSouce = GetComponent<AudioSource>();

        keyFinder = Resources.Load<GameObject>("Rebind");
        playerAnim = GetComponent<Animator>();
        //swordAnim = GetComponentInChildren
        _isBlue = true;
        cdTimer = attackCD;
        currentAmmo = maxAmmo;
        ammoBar.value = AmmoCount();
        melee = false;
        vacBar.maxValue = maxStatic;
        vacBar.value = 0;

        //TEMP: Set notify text to nothing
        notifyText.text = "";
    }

    // Update is called once per frame
    void Update()
    {

        dustEnemy = GameObject.FindGameObjectWithTag("RedEnemy");

        ammoBar.value = AmmoCount();

        vacBar.value = staticForce;

        //float dustDist = Vector3.Distance(transform.position, dustEnemy.transform.position);

        /*Vector3 offset = dustEnemy.transform.position - transform.position;
        float dustDist = offset.sqrMagnitude;
        Debug.Log(dustDist);*/

        if (!player.dead)
        {

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, meleeRange))
            {

                DustEnemyScript target = hit.transform.GetComponent<DustEnemyScript>();
                StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();

                if (target != null)
                {
                    melee = true;
                }

                if(target2 != null && target2.tag == "Stain Enemy" && target2.stainHealth == 10)
                {
                    melee = true;
                }

                //swingHit = false;
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, 150f))
            {

                GermEnemyScript target = hit.transform.GetComponent<GermEnemyScript>();

                if (target != null)
                {
                    melee = false;
                }

                //swingHit = false;
            }
            //Switches between the two weapon types. Blue Weapon is the shotgun, RedWeapon is the sword/melee

            if (melee == false)
            {
                BlueWeapon();
                _isBlue = true;
                _isRed = false;
                _vaccuumOn = false;
            }
            if (melee)
            {
                RedWeapon();
                _isRed = true;
                _isBlue = false;
                _vaccuumOn = false;
            }
            if (canHoover)
            {
                Vacuum();
                _vaccuumOn = true;
                _isRed = false;
                _isBlue = false;
                staticForce = 0;
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

                swordAnim.SetTrigger("swing2");
                Swing();
            }

            if (Input.GetMouseButton(0) && _vaccuumOn == true && isSucking == false)
            {
                isSucking = true;
                suckTimer -= Time.deltaTime;
                SuckemUp();
            }
            else
            {
                isSucking = false;
            }

            //Starts the cooldown timer once Shoot function has been called

            if (_hasAttacked)
            {
                attackCD -= Time.deltaTime;
            }

            //Resets the cooldown and the shot status once the timer hits 0

            if (attackCD <= 0)
            {
                attackCD = cdTimer;
                _hasAttacked = false;
            }

            if (Physics.Raycast(transform.position, transform.forward, out hit, 5f) && Input.GetMouseButton(0))
            {
                Debug.Log(hit.transform.name);

                GameObject target = hit.transform.gameObject;

                if (target != null && target.name == "Reload Station")
                {
                    _reloader.SetActive(true);
                    reloadBar.value = AmmoCount();
                    reloadTimer -= Time.deltaTime;
                }
            } else
            {
                _reloader.SetActive(false);
            }

            if (reloadTimer <= 0 && currentAmmo <= 20)
            {
                currentAmmo += 2;
                reloadTimer = 0.5f;

                audioSouce.PlayOneShot(reloadClip);
            }
            else if (currentAmmo >= 20)
            {
                currentAmmo = 20;
                reloadTimer = 0.5f;
            }

            if (staticForce >= maxStatic)
            {
                canHoover = true;
            }

            if (suckTimer <= 0)
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
        audioSouce.PlayOneShot(rangedClip);

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
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

                GameObject impactGO = Instantiate(_impactEffect, hit.point, _impactEffect.transform.rotation);
                impactGO.GetComponent<ParticleSystem>().Play();
            }

            if (target2 != null && target2.tag == "Stain Enemy" && target2.stainHealth > 10)
            {
                staticForce += 1;
                target2.TakeDamage(damage);

                GameObject impactGO = Instantiate(_impactEffect, hit.point, _impactEffect.transform.rotation);
                impactGO.GetComponent<ParticleSystem>().Play();
            }
            else if (targetGO != null && targetGO.tag == "RedEnemy")
            {
                StartCoroutine("WrongWeaponNotify");
                LevelManager.instance.uiHandler.ClothNotif();
            }


        }


    }

    //swing function, works in the same way as the shoot function, instead looking for "Red Enemy".

    public void Swing()
    {

        audioSouce.PlayOneShot(meleeClip);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, meleeRange))
        {
            Debug.Log(hit.transform.name);

            DustEnemyScript target = hit.transform.GetComponent<DustEnemyScript>();
            StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();

            //TEMP: Get targets gameobject to test if wrong weapon used
            GameObject targetGO = hit.transform.gameObject;

            if (target != null && target.tag == "RedEnemy")
            {
                staticForce += 1;
                swingHit = true;

                //TEMP: Notify on wrong weapon used
            }

            if (targetGO != null && targetGO.tag == "BlueEnemy")
            {
                StartCoroutine("WrongWeaponNotify");
                LevelManager.instance.uiHandler.SprayNotif();
            }

            if (target2 != null && target2.tag == "Stain Enemy" && target2.stainHealth == 10)
            {
                swingHit = true;
                SendDamage2();
            }

            if (swingHit)
            {
                //swordAnim.SetTrigger("swing2");

                if (target != null && target.tag == "RedEnemy")
                {
                    SendDamage1();

                    GameObject impactGO = Instantiate(dustImpactEffect, hit.point, dustImpactEffect.transform.rotation);
                    impactGO.GetComponent<ParticleSystem>().Play();
                }

                if (target2 != null && target2.tag == "Stain Enemy")
                {
                    SendDamage2();

                    GameObject impactGO = Instantiate(dustImpactEffect, hit.point, dustImpactEffect.transform.rotation);
                    impactGO.GetComponent<ParticleSystem>().Play();
                }

                swingHit = false;
            }
        }

        if (swingHit == false)
        {
            //swordAnim.SetTrigger("swing");
        }

    }

    void SendDamage1()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 150))
        {

            DustEnemyScript target = hit.transform.GetComponent<DustEnemyScript>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            swingHit = false;
        }

    }

    void SendDamage2()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 150))
        {

            StainEnemyScript target2 = hit.transform.GetComponent<StainEnemyScript>();

            if (target2 != null)
            {
                target2.TakeDamage(damage);
            }

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

            if (target3 != null && target3.tag == "BlueEnemy")
            {
                target3.transform.position = Vector3.MoveTowards(target3.transform.position, transform.position, 0.8f);
            }

            if (targetGO != null && targetGO.tag == "BlueEnemy")
            {
                StartCoroutine("WrongWeaponNotify");
            }

        }
    }

    /*public void Reload()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            Debug.Log(hit.transform.name);

            GameObject target = hit.transform.gameObject;

            if (target != null && target.name == "Reload Station")
            {
                _reloader.SetActive(true);
                reloadBar.value = AmmoCount();
                reloadTimer -= Time.deltaTime;
            }
        }


    }*/

    //TEMP: Coroutine to handle notify text on wrong weapon used for playtest
    IEnumerator WrongWeaponNotify()
    {
        notifyText.text = "Attack is ineffective! Try a different weapon!";

        yield return new WaitForSeconds(2);

        notifyText.text = "";
    }
}
