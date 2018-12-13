using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public void PlayScene()
    {
        SceneManager.LoadScene("Game Scene", LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync("Menu Scene");
    }

    public void ControlScene()
    {
        SceneManager.LoadScene("Controls Scene", LoadSceneMode.Single);
        //SceneManager.UnloadSceneAsync("Game Scene");
    }
}
