using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] UnityEvent enterEvent;
    [SerializeField] UnityEvent exitEvent;

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            enterEvent.Invoke();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            exitEvent.Invoke();
        }
    }

}
