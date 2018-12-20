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

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }
    
    public void ChangeScene(int newSceneIndex) {
        SceneManager.LoadScene(newSceneIndex);
    }

}
