using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingHitBoxScript : MonoBehaviour {

    [SerializeField] GameObject _swordHitBox, Enemy;

    //initial use for swing hit detection, was not working so I switched to raycasting instead. Right now this script is unused

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.tag == "RedEnemy")
        {
            Enemy.GetComponent<EnemyScript>().EnemyHealth -= 10;
            Debug.Log("Hit");
        }
    }


    public void meleeHitBoxOff()
    {
        _swordHitBox.SetActive(false);
    }
}
