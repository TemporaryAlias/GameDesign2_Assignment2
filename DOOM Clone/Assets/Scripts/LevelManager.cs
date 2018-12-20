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
    }

    public UIHandler uiHandler;

    PlayerScript player;

    void Start() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    
    void Update()
    {
        
    }
    
    public void ChangeScene(int newSceneIndex) {
        SceneManager.LoadScene(newSceneIndex);
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        uiHandler = FindObjectOfType<UIHandler>();
        player = FindObjectOfType<PlayerScript>();

        if (player != null) {
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        } 
    }

}
