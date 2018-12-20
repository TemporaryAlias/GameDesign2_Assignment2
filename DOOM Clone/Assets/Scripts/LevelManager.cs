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

    public int stainsKilled, germsKilled, dustKilled;
    public float enemiesLeft;

    public KeyBindsScript keyRebinds;

    public PlayerScript player;
    MouseMovementScript playerMouse;

    AudioSource audioSource;

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoad;

        uiHandler = FindObjectOfType<UIHandler>();
        player = FindObjectOfType<PlayerScript>();
        playerMouse = FindObjectOfType<MouseMovementScript>();

        if (player != null || playerMouse != null) {
            Cursor.lockState = CursorLockMode.Locked;

            stainsKilled = 0;
            germsKilled = 0;
            dustKilled = 0;
            enemiesLeft = 0;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    void Update()
    {
        if (audioSource == null && (player != null || playerMouse != null)) { 
            audioSource = GetComponent<AudioSource>();
        }

        if (player != null) {
            if (player.EnemyCount() <= 0) {
                uiHandler.StartFadeOut(3);
            }
        }
    }
    
    public void ChangeScene(int newSceneIndex) {
        SceneManager.LoadScene(newSceneIndex);
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Controls Scene" && keyRebinds != null) {
            Destroy(keyRebinds.gameObject);
        }

        uiHandler = FindObjectOfType<UIHandler>();
        player = FindObjectOfType<PlayerScript>();
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
