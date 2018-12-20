using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class KeyBindsScript : MonoBehaviour {

    public KeyCode forward, back, left, right, hoover, melee, ranged;
    public Button forwardButton, backButton, leftButton, rightButton, hooverButton;
    public Text forwardText, backText, leftText, rightText, hooverText, changeText, meleeText, rangedText;
    //public bool isForward = false;
    enum Enum {forward, back, left, right,vacuum};
    public bool forwardMap, backMap, leftMap, rightMap, hooverMap, meleeMap, rangedMap;
	// Use this for initialization
	void Start () {
        forward = KeyCode.W;
        back = KeyCode.S;
        left = KeyCode.A;
        right = KeyCode.D;
        hoover = KeyCode.F;
        
	}

    private void Update()
    {
        

        if(forwardMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    forward = kcode;
                    forwardText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    forwardMap = false;
                    changeText.enabled = false;
                }

            }
        }

        if(backMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    back = kcode;
                    backText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    backMap = false;
                    changeText.enabled = false;
                }

            }
        }
        
        if(leftMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    left = kcode;
                    leftText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    leftMap = false;
                    changeText.enabled = false;
                }

            }
        }

        if(rightMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    right = kcode;
                    rightText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    rightMap = false;
                    changeText.enabled = false;
                }

            }
        }

        if(meleeMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    melee = kcode;
                    meleeText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    meleeMap = false;
                    changeText.enabled = false;
                }

            }
        }

        if(rangedMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    ranged = kcode;
                    rangedText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    rangedMap = false;
                    changeText.enabled = false;
                }

            }
        }

        if(hooverMap)
        {
            changeText.enabled = true;
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                {
                    hoover = kcode;
                    hooverText.text = " " + kcode;
                    Debug.Log("KeyCode down: " + kcode);
                    hooverMap = false;
                    changeText.enabled = false;
                }

            }
        }
        //DontDestroyOnLoad(this.gameObject);

    }
    

    public void ForwardClick()
    {
        forwardMap = true;
    }
    
    public void Backwards()
    {
        backMap = true;
    }

    public void Left()
    {
        leftMap = true;
    }

    public void Right()
    {
        rightMap = true;
    }

    public void Hoover()
    {
        hooverMap = true;
    }

    public void Melee()
    {
        meleeMap = true;
    }

    public void Ranged()
    {
        rangedMap = true;
    }

    public void ChangeScene(int sceneIndex)
    {
        PrefabUtility.CreatePrefab("Assets/Resources/" + gameObject.name + ".prefab", gameObject);
        LevelManager.instance.uiHandler.StartFadeOut(sceneIndex);
        DontDestroyOnLoad(this.gameObject);
        
    }
}
