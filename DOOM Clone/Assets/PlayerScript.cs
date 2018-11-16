using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Rigidbody _myRb;
    private bool _isRed, _isBlue;
    [SerializeField] float speed;
    [SerializeField] GameObject _blueWeapon, _redWeapon, _redEnemy, _blueEnemy;

	// Use this for initialization
	void Start () {
        _myRb = GetComponent<Rigidbody>();
        _isBlue = true;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {

        var xMove = Input.GetAxis("Horizontal");
        var yMove = Input.GetAxis("Vertical");

        //var movement = new Vector3(xMove, 0, yMove);

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(yMove > 0)
        {
            transform.Translate(Vector3.forward * speed);
        } else if (yMove < 0)
        {
            transform.Translate(Vector3.back * speed);
        }

        if(xMove < 0)
        {
            transform.Translate(Vector3.left * speed);
        }
        else if (xMove > 0)
        {
            transform.Translate(Vector3.right * speed);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            BlueWeapon();
            _isBlue = true;
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            RedWeapon();
            _isBlue = false;
        }

        if(_isBlue && Input.GetMouseButtonDown(0))
        {
            _blueEnemy.SetActive(false);
        } else if(_isBlue == false && Input.GetMouseButtonDown(0))
        {
            _redEnemy.SetActive(false);
        }

        //_myRb.velocity = movement * speed;
    }

    void BlueWeapon()
    {
        _blueWeapon.SetActive(true);
        _redWeapon.SetActive(false);
    }

    void RedWeapon()
    {
        _blueWeapon.SetActive(false);
        _redWeapon.SetActive(true);
    }
}
