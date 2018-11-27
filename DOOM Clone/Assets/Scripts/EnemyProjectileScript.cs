using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour {

    [SerializeField] float speed, destroyTimer;

    // Use this for initialization
    void Start () {
        StartCoroutine("DestroyTimer");
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed);
	}

    IEnumerator DestroyTimer() {
        yield return new WaitForSeconds(destroyTimer);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
