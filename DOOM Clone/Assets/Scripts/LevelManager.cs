using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    private void Awake() {
        if (instance != null) {
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public UIHandler uiHandler;

    PlayerScript player;
    MouseMovementScript playerMouse;

    AudioSource audioSource;

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    
    void Update()
    {
        if (player != null) {
            //if (player.currentEnemies == 0) {
              //  ChangeScene(3);
            //}
        }
    }
    
    public void ChangeScene(int newSceneIndex) {
        SceneManager.LoadScene(newSceneIndex);
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        uiHandler = FindObjectOfType<UIHandler>();
        player = FindObjectOfType<PlayerScript>();
        audioSource = GetComponent<AudioSource>();
        playerMouse = FindObjectOfType<MouseMovementScript>();

        if (player != null || playerMouse != null) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        } 
    }

    public void PlaySound(AudioClip sound) {
        audioSource.PlayOneShot(sound);
    }

}
